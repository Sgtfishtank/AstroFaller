using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour 
{
	private WarningArrow mWarning;
	private Player mPlayer;
	private Rigidbody mRb;
	private FMOD.Studio.EventInstance mClash;

	void Awake()
	{
		mRb = GetComponent<Rigidbody> ();

		mClash = AudioManager.Instance.GetSoundsEvent("AsteroidColision/AsteroidColision");

	}

	// Use this for initialization
	void Start ()
	{
		mPlayer = WorldGen.Instance.Player();
	}
	
	void OnDisable()
	{
	}

	void OnEnable()
	{
		float mRotationSpeed = GlobalVariables.Instance.ASTROID_SPAWN_ROTATION_SPEED;
		
		//add torque
		mRb.AddTorque(
			new Vector3(UnityEngine.Random.Range(-mRotationSpeed, mRotationSpeed),
		            UnityEngine.Random.Range(-mRotationSpeed, mRotationSpeed),
		            UnityEngine.Random.Range(-mRotationSpeed, mRotationSpeed)));
	}

	// Update is called once per frame
	void Update ()
	{
	}

	void OnCollisionEnter(Collision coll)
	{
		if ((coll.gameObject != gameObject) && (coll.gameObject != mPlayer.gameObject))
		{
			AudioManager.Instance.PlaySoundOnce (mClash);
			InGame.Instance.SpawnAstCollisionEffects(coll.contacts[0].point);
		}
	}
}
