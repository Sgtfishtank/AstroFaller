using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour 
{
	public bool mTargetPlayer;

	private ParticleManager mShotsManager;
	private GameObject mShootEffect;
	private GameObject mBase;
	private float mRotation;
	private float mShootT;
	private AstroidSpawn mAS;
	private Player mPlayer;
	private Vector3 mBasePos;
	private bool mPayerDetected = false;

	void Awake()
	{
		mPlayer = InGame.Instance.Player ();
		mShootEffect = transform.Find("turret_explosion_effect").gameObject;
		mShotsManager = GetComponent<ParticleManager> ();
		mBase = transform.Find("Turret_anim/Base").gameObject;
		mShotsManager.Load(GlobalVariables.Instance.TURRET_MAX_BULLETS);
		mBasePos = mBase.transform.localPosition;
	}

	// Use this for initialization
	void Start () 
	{
		mAS = InGame.Instance.AstroidSpawn ();
	}

	void OnEnable()
	{
		mRotation = Random.Range(-1.0f, 1.0f);
	}
	
	void OnDisable()
	{
		mShotsManager.reset ();
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

		if (mPlayer.CenterPosition().y >= transform.position.y) 
		{
			RotateTowardsPlayer ();
			if(mPayerDetected)
			{
				if (mShootT < Time.time) 
				{
					mShootT = Time.time + GlobalVariables.Instance.TURRET_SHOOT_DELAY;
					Shoot();
				}
			}
		}

		float val = Mathf.Clamp(((mShootT - Time.time) / GlobalVariables.Instance.TURRET_SHOOT_DELAY), 0, 1);

		mBase.transform.localPosition = mBasePos - (mBase.transform.localRotation * Vector3.up * 0.2f * val);
	}

	void RotateTowardsPlayer()
	{
		float targetRotation = 0;
		if (mTargetPlayer) 
		{
			targetRotation = lookTowardsPlayer();
		}
		else
		{
			targetRotation = Mathf.Sin(Time.time);
		}

		mRotation = Mathf.Lerp(mRotation, targetRotation, Time.deltaTime * GlobalVariables.Instance.TURRET_ROTATION_SPEED);

		mBase.transform.localRotation = Quaternion.Euler (0, 0, 84 * mRotation);
	}

	float lookTowardsPlayer()
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

		return Mathf.Clamp (angle / -84, -1, 1);
	}

	void Shoot ()
	{
		Vector3 offset = mBase.transform.rotation * new Vector3(0, 3.27f * transform.localScale.x / 9, 0);

		mShootEffect.SetActive(false);
		mShootEffect.transform.position = transform.position + offset;
		mShootEffect.SetActive(true);

		GameObject shot = mShotsManager.Spawn(transform.position + offset);
		if (shot != null) 
		{
			shot.GetComponent<Rigidbody> ().velocity = offset.normalized * GlobalVariables.Instance.TURRET_BULLET_SPEED;
			shot.transform.rotation = mBase.transform.rotation;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if(col.transform.tag == "Player")
			mPayerDetected = true;
	}
	void OnTriggerExit(Collider col)
	{
		if(col.transform.tag == "Player")
			mPayerDetected = false;
	}
}
