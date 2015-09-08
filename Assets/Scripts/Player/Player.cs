using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	public float mMaxFallSpeed = 10;
	public float mMaxCurrentFallSpeed = 10;
	public bool mInvulnerable = false;
	public Animator mAni;
	public Transform mMeshTrans;
	private AstroidSpawn mAS;

	public bool mUseAirDrain;
	public bool mUseAirReg;
	private float mAirAmount;

	private FollowPlayer mfp;
	private Rigidbody mRb;
	private float mStartYValue;
	private bool mIsDead = false;
	private bool mPlaying;
	private MovementControls mMovementControls;
	public SkinnedMeshRenderer[] skinnedMeshRenderer;

	private float mDashTime;
	private float mDashCDTime;
	
	private bool doShift = false;
	private float shiftAmount;
	
	public int mLife;
	public int mBoltsCollected;
	public int mCrystalsCollected;
	public GameObject boltParticles;
	public GameObject mDash;
	private Collider mLastDmgCollider;
	private float mPerfectDistanceY;

	private FMOD.Studio.EventInstance mDownSwipeSound;
	private FMOD.Studio.EventInstance mHurtHitSound;
	private FMOD.Studio.EventInstance mCoinPickUpSound;
	private FMOD.Studio.EventInstance mInflateSound;
	private FMOD.Studio.EventInstance mDeflateSound;
	
	// Use this for initialization
	void Awake() 
	{
		// keep player along levels 
		DontDestroyOnLoad(transform.gameObject);
		
		// init compoments
		mRb = GetComponent<Rigidbody>();
		mAni = transform.Find ("Chubby_Hover").GetComponent<Animator> ();
		mDownSwipeSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Downswipe/DownSwipe");
		mHurtHitSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/TakeDamage/TakeDamage1");
		mCoinPickUpSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Screws/ScrewsPling2");
		mInflateSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Inflate/Inflate");
		mDeflateSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Deflate/Deflate");
		
		// init internal scrips
		mMovementControls = new MovementControls(null, null, this, skinnedMeshRenderer);
		
		// reset air
		mAirAmount = GlobalVariables.Instance.PLAYER_MAX_AIR;
		
		// put at level start position, if any
		mIsDead = false;
		mPlaying = false;
	}

	// Use this for initialization
	void Start()
	{
		mAS = WorldGen.Instance.AstroidSpawn ();
		mfp = InGameCamera.Instance.GetComponent<FollowPlayer>();
		mLife = GlobalVariables.Instance.PLAYER_MAX_LIFE;
		mDash = GameObject.Find("Burst_Trail");
		mDash.SetActive(false);
	}

	public void StartGame()
	{
		mBoltsCollected = 0;
		mCrystalsCollected = 0;
		mStartYValue = transform.position.y;
		mPlaying = true;
		mPerfectDistanceY = CenterPosition().y - GlobalVariables.Instance.PERFECT_DISTANCE_SIZE;
		InGame.Instance.UpdatePerfectDistance(mPerfectDistanceY);
	}

	public void Dash()
	{
		if(mDashCDTime < Time.time)
		{
			mDash.SetActive(true);
			AudioManager.Instance.PlaySoundOnce(mDownSwipeSound);
			mfp.Dash();
			mMaxCurrentFallSpeed = mMaxFallSpeed + GlobalVariables.Instance.PLAYER_DASH_SPEED;
			mDashTime = Time.time + GlobalVariables.Instance.PLAYER_DASH_SPEED_DELAY;
			mRb.velocity += new Vector3(0,-GlobalVariables.Instance.PLAYER_DASH_SPEED,0);
			mDashCDTime = Time.time + GlobalVariables.Instance.PLAYER_DASH_CD;
		}
	}
	
	public void Inflate()
	{
		mAni.SetBool("Hover", true);
		AudioManager.Instance.PlaySoundOnce(mInflateSound);
	}
	
	public void Deflate()
	{
		mAni.SetBool("Hover", false);
		mAni.CrossFade("Chubby_Tumblin", 0.1f, 0, UnityEngine.Random.value);
		AudioManager.Instance.PlaySoundOnce(mDeflateSound);
	}

	void FixedUpdate()
	{
		// hover physics
		mMovementControls.Hover(mRb);
		
		// jump and hover player
		mAirAmount = mMovementControls.JumpAndHover(mRb, mAirAmount);
		
		// move player
		mMovementControls.Move(mRb);
	}

	void Update()
	{
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

		if (CenterPosition().y < mPerfectDistanceY)
		{
			mPerfectDistanceY = CenterPosition().y - GlobalVariables.Instance.PERFECT_DISTANCE_SIZE;
			InGame.Instance.UpdatePerfectDistance(mPerfectDistanceY);
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			Dash();
		}

		if(mMaxCurrentFallSpeed > mMaxFallSpeed && mDashTime < Time.time)
		{
			mMaxCurrentFallSpeed -= GlobalVariables.Instance.PLAYER_VERTICAL_SPEED_FALLOF * Time.deltaTime;
			if(mMaxCurrentFallSpeed < mMaxFallSpeed+0.1f)
			{
				mDash.SetActive(false);
			}
		}
		mMaxCurrentFallSpeed = Mathf.Max(mMaxFallSpeed, mMaxCurrentFallSpeed);
	}

	void LateUpdate()
	{
		if (doShift)
		{
			ShiftBackLate();
			doShift = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Bolts")
		{
			Instantiate(boltParticles, col.transform.parent.position, Quaternion.identity);
			mBoltsCollected += GlobalVariables.Instance.BOLT_VALUE;
			col.gameObject.SetActive(false);
			AudioManager.Instance.PlaySoundOnce(mCoinPickUpSound);

		}
		else if(col.tag == "BoltCluster")
		{
			mBoltsCollected += GlobalVariables.Instance.BOLT_CLUSTER_VALUE;
		}
		else if(col.tag == "SpawnAstroid")
		{
			mAS.gameObject.SetActive(true);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "SpawnAstroid")
		{
			mAS.gameObject.SetActive(false);
		}
	}
	void OnCollisionEnter(Collision coll)
	{
		if ((coll.transform.tag == "Enemy") && (mLastDmgCollider != coll.collider))
		{
			mPerfectDistanceY = CenterPosition().y - GlobalVariables.Instance.PERFECT_DISTANCE_SIZE;
			InGame.Instance.UpdatePerfectDistance(mPerfectDistanceY);

			mLastDmgCollider = coll.collider;
			PlayerDamage(1);
		}
	}
	void OnCollisionExit(Collision coll)
	{
		if(coll.transform.tag == "Enemy" )
		{
			//PlayerDamage(1);
		}
	}

	public int colectedBolts()
	{
		return mBoltsCollected;
	}
	
	public int colectedCrystals()
	{
		return mCrystalsCollected;
	}
	
	public Vector3 CenterPosition()
	{
		return mRb.worldCenterOfMass;
	}

	public int distance()
	{
		if (!mPlaying)
		{
			return 0;
		}

		int dist = (int)(CenterPosition().y - mStartYValue - WorldGen.Instance.FallShift());
		return -dist;
	}

	public float airAmount()
	{
		return mAirAmount;
	}

	public bool isDead ()
	{
		return mIsDead;
	}
	
	public void PlayerDamage(int dmg)
	{
		if (!mInvulnerable)
		{
			mLife -= dmg;
			if (mLife <= 0)
			{
				PlayerDead();
			}
		}
		AudioManager.Instance.PlaySoundOnce(mHurtHitSound);
	}

	public void PlayerDead()
	{
		if(!mInvulnerable && !mIsDead)
		{
			mIsDead = true;

			mRb.velocity = new Vector2(0, 0);
			gameObject.SetActive(false);

			DepositData();
		}
	}

	public void DepositData()
	{
		PlayerData.Instance.depositBolts(colectedBolts());
		mBoltsCollected = 0;
		
		PlayerData.Instance.depositCrystals(colectedCrystals());
		mCrystalsCollected = 0;
		
		PlayerData.Instance.depositDistance(distance());
		mStartYValue = transform.position.y;
	}

	public void ShiftBack (float shift)
	{
		doShift = true;
		shiftAmount = shift;
		transform.position -= new Vector3(0, shift, 0);
		mPerfectDistanceY -= shift;
	}
	
	void ShiftBackLate()
	{
		ParticleSystem[] sys = GetComponentsInChildren<ParticleSystem> ();
		for (int i = 0; i < sys.Length; i++) 
		{
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[sys[i].maxParticles];
			
			int size = sys[i].GetParticles(particles);

			for (int j = 0; j < particles.Length; j++) 
			{
				particles[j].position -= new Vector3(0, shiftAmount, 0);
			}
			
			sys[i].SetParticles(particles, size);
		}
	}

	void respawn ()
	{
	}
}