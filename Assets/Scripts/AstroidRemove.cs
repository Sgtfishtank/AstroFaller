using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour 
{
	public GameObject mWarningPrefab;
	public GameObject mCollisionEffect1;
	public GameObject mCollisionEffect2;
	public GameObject[] mCollisionEffects1;
	public GameObject[] mCollisionEffects2;
	
	private GameObject mWarning;
	private Player mpl;
	private AstroidSpawn mAstroidSpawn;
	private float mHideT;
	private Rigidbody mRb;
	private FMOD.Studio.EventInstance mClash;

	void Awake()
	{
		mRb = GetComponent<Rigidbody> ();

		mClash = FMOD_StudioSystem.instance.GetEvent ("event:/Sounds/AsteroidColision/AsteroidColision");
		
		mWarning = GlobalVariables.Instance.Instanciate (mWarningPrefab, null, 0.05f);

		mHideT = Time.time + GlobalVariables.Instance.ASTEROID_WARNING_MAX_SHOW_TIME;

		mCollisionEffects1 = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES];
		mCollisionEffects2 = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES];
		for (int i = 0; i < mCollisionEffects1.Length; i++)
		{
			mCollisionEffects1[i] = (GameObject)GameObject.Instantiate(mCollisionEffect1);
			mCollisionEffects2[i] = (GameObject)GameObject.Instantiate(mCollisionEffect2);
			mCollisionEffects1[i].transform.parent = InGame.Instance.transform.Find("ParticlesGoesHere");
			mCollisionEffects2[i].transform.parent = InGame.Instance.transform.Find("ParticlesGoesHere");
			mCollisionEffects1[i].gameObject.SetActive(false);
			mCollisionEffects2[i].gameObject.SetActive(false);
		}
	}

	// Use this for initialization
	void Start ()
	{
		mpl = WorldGen.Instance.Player();
		mAstroidSpawn = WorldGen.Instance.AstroidSpawn ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (OutOfBound())
		{
			mWarning.SetActive(false);
			mAstroidSpawn.RemoveAstroid(gameObject);
		}
		
		if (mWarning.activeSelf)
		{
			UpdateWarning ();
		}
	}

	void UpdateWarning ()
	{
		Vector3 plVel = mpl.Rigidbody().velocity;
		Quaternion rot = Quaternion.LookRotation (mWarning.transform.forward, mRb.velocity - new Vector3(0, plVel.y, 0));
		mWarning.transform.rotation = rot * Quaternion.Euler (0, 0, 90);
		
		Vector3 a = InGameCamera.Instance.Camera().WorldToScreenPoint(transform.position);
		
		a.z = 1;
		
		if (((transform.position.x < 0) && (a.x > 10)) || ((transform.position.x > 0) && (a.x < Screen.width - 10)) || (mHideT < Time.time))
		{
			mWarning.SetActive(false);
		}
		else if (transform.position.x < 0)
		{
			a.x = 10;
		}
		else
		{
			a.x = Screen.width - 10;
		}
		
		mWarning.transform.position = InGameCamera.Instance.Camera().ScreenToWorldPoint(a);
	}

	void OnCollisionEnter(Collision coll)
	{
		if ((coll.gameObject != gameObject) && (coll.gameObject != mpl.gameObject))
		{
			AudioManager.Instance.PlaySoundOnce (mClash);
			int index  = PickCollisionEffect();
			if (index != -1) 
			{
				mCollisionEffects1[index].SetActive(true);
				mCollisionEffects2[index].SetActive(true);
				mCollisionEffects1[index].transform.position = coll.contacts[0].point;
				mCollisionEffects2[index].transform.position = coll.contacts[0].point;
				mCollisionEffects1[index].transform.rotation = Quaternion.identity;
				mCollisionEffects2[index].transform.rotation = Quaternion.identity;
			}
		}
	}

	int PickCollisionEffect()
	{
		for (int i = 0; i < mCollisionEffects1.Length; i++) 
		{
			if (!mCollisionEffects1[i].activeSelf) 
			{
				return i;
			}
		}

		return -1;
	}

	public bool OutOfBound()
	{
		return (!(transform.position.x < (GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f) && 
		          transform.position.x > -(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f) &&
			transform.position.y < (mpl.transform.position.y + 25) && 
		            transform.position.y > (mpl.transform.position.y - 50)));
	}
}
