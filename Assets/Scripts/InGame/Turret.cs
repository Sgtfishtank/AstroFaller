using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour 
{
	public bool mTargetPlayer;

	private ParticleManager mShotsManager;
	private GameObject[] mShots;

	public float mRotationSpeed;
	public float mShootDelay;
	public float mShotSpeed;

	private GameObject mShootEffect;
	
	private GameObject mBase;

	private float mRotation;
	private float mShootT;
	private AstroidSpawn mAS;
	private Player mPlayer;

	void Awake()
	{
		mPlayer = InGame.Instance.Player ();
		mShootEffect = transform.Find("turret_explosion_effect").gameObject;
		mShotsManager = GetComponent<ParticleManager> ();
		mBase = transform.Find("Turret_anim/Base").gameObject;
		mShotsManager.Load(15);
	}

	// Use this for initialization
	void Start () 
	{
		mAS = InGame.Instance.AstroidSpawn ();
	}

	void OnEnable()
	{
		mRotation = (Random.value * 2) - 1;
	}
	
	void OnDisable()
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < mShotsManager.getParticles().Length; i++) 
		{
			if ((mShotsManager.getParticles()[i].activeSelf) && (mAS.OutOfBound(mShotsManager.getParticles()[i])))
			{
				mShotsManager.DespawnParticle(i);
			}
		}

		if (mPlayer.CenterPosition().y < transform.position.y) 
		{
			return;
		}

		RotateTowardsPlayer ();


		if (mShootT < Time.time) 
		{
			mShootT = Time.time + mShootDelay;
			Shoot();
		}
	}

	void RotateTowardsPlayer()
	{
		if (mTargetPlayer) 
		{
			lookTowardsPlayer();
		}
		else
		{
			mRotation = Mathf.Sin(Time.time * mRotationSpeed);
		}

		mBase.transform.localRotation = Quaternion.Euler (0, 0, 84 * mRotation);
	}

	void lookTowardsPlayer()
	{
		// look towards player
		Vector3 relativePos = mPlayer.CenterPosition() - mBase.transform.position;
		if ((relativePos.x < 0.1f) && (relativePos.x >= 0))
		{
			relativePos.x = 0.1f;
		}
		else if ((relativePos.x > -0.1f) && (relativePos.x <= 0))
		{
			relativePos.x = -0.1f;
		}
		float angle = Quaternion.LookRotation(new Vector3(0, 0, 1), relativePos).eulerAngles.z;

		while (angle > 180) 
		{
			angle -= 360;
		}
		mRotation = Mathf.Clamp (angle / -84, -1, 1);
	}

	void Shoot ()
	{
		mShootEffect.SetActive(false);
		mShootEffect.SetActive(true);
		Vector3 offset = mBase.transform.rotation * new Vector3(0, 3.27f, 0);
		GameObject shot = mShotsManager.Spawn(transform.position + offset);

		if (shot != null) 
		{
			shot.GetComponent<Rigidbody> ().velocity = offset.normalized * mShotSpeed;
			shot.transform.rotation = mBase.transform.rotation;
		}
	}


}
