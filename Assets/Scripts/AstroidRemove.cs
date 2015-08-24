using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour {

	public int xSize;
	public GameObject mWarningPrefab;
	public GameObject mWarning;

	private Player mpl;
	private AstroidSpawn mAstroidSpawn;
	public float mHideTime = 2;
	private float mHideT;

	private FMOD.Studio.EventInstance mClash;

	// Use this for initialization
	void Start ()
	{
		mClash = FMOD_StudioSystem.instance.GetEvent ("event:/Sounds/AsteroidColision/AsteroidColision");
		
		mWarning = GlobalVariables.Instance.Instanciate (mWarningPrefab, null, 0.05f);
		
		mWarning.SetActive (true);

		mpl = WorldGen.Instance.Player();
		mAstroidSpawn = WorldGen.Instance.AstroidSpawn ();

		mHideT = Time.time + mHideTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (OutOfBound())
		{
			Destroy(mWarning);
			mAstroidSpawn.RemoveAstroid(gameObject);
		}

		if (!mWarning.activeSelf)
		{
			return;
		}

		GameObject ast = gameObject;
		Player ply = WorldGen.Instance.Player();
		
		//Vector3 diff = ply.CenterPosition() - ast.GetComponent<Rigidbody>().position;
		
		Vector3 plVel = ply.GetComponent<Rigidbody> ().velocity;
		Quaternion rot = Quaternion.LookRotation (mWarning.transform.forward, ast.GetComponent<Rigidbody>().velocity - new Vector3(0, plVel.y, 0));
		mWarning.transform.rotation = rot * Quaternion.Euler (0, 0, 90);
		
		Vector3 a = InGameCamera.Instance.GetComponent<Camera>().WorldToScreenPoint(ast.transform.position);

		a.z = 1;

		if (((ast.transform.position.x < 0) && (a.x > 10)) || ((ast.transform.position.x > 0) && (a.x < Screen.width - 10)) || (mHideT < Time.time))
		{
			mWarning.SetActive(false);
		}
		else if (ast.transform.position.x < 0)
		{
			a.x = 10;
		}
		else
		{
			a.x = Screen.width - 10;
		}

		mWarning.transform.position = InGameCamera.Instance.GetComponent<Camera> ().ScreenToWorldPoint(a);
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject != gameObject)
		{
			AudioManager.Instance.PlaySoundOnce (mClash);
		}
	}

	public bool OutOfBound()
	{
		return (!(transform.position.x < (GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f) && 
		          transform.position.x > -(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f) &&
			transform.position.y < (mpl.transform.position.y + 25) && 
		            transform.position.y > (mpl.transform.position.y - 50)));
	}
}
