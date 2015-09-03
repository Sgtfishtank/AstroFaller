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
	private MovementControls mMovementControls;
	public SkinnedMeshRenderer[] skinnedMeshRenderer;

	private float mDashTime;
	private float mDashCDTime;
	public int mLife;

	public int mBoltsCollected;
	public int mCrystalsCollected;
	public GameObject boltParticles;
	private bool doShift = false;
	private float doShiftValue;
	public GameObject mDash;

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
		mInflateSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Inflate/Inflate");
		mDeflateSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Deflate/Deflate");
		
		// init internal scrips
		mMovementControls = new MovementControls(null, null, this, skinnedMeshRenderer);
		
		// reset air
		mAirAmount = GlobalVariables.Instance.PLAYER_MAX_AIR;
		
		// put at level start position, if any
		mIsDead = false;
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
		if(mIsDead)
		{
			return;
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
			doShift = false;
			ShiftBackLate();
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
		if(coll.transform.tag == "Enemy" )
		{
			if(mLife == 1)
			{
				PlayerDead();
			}
			else
			{
				mLife--;
			}
			AudioManager.Instance.PlaySoundOnce(mHurtHitSound);
		}
	}
	void OnCollisionExit(Collision coll)
	{
		if(coll.transform.tag == "Enemy" )
		{
			if(mLife == 1)
			{
				PlayerDead();
			}
			else
			{
				mLife--;
			}
			AudioManager.Instance.PlaySoundOnce(mHurtHitSound);
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
		int dist = (int)(transform.position.y - mStartYValue - WorldGen.Instance.FallShift());
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
		doShiftValue = shift;
		transform.position -= new Vector3(0, shift, 0);
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
				particles[j].position -= new Vector3(0, doShiftValue, 0);
			}
			
			sys[i].SetParticles(particles, size);
		}
	}

	void respawn ()
	{
		//ani.SetTrigger("Respawn");
		//GameObject res = (GameObject)GameObject.Instantiate(mRespawnEffectPrefab, transform.position + new Vector3(0, -0.5f, 0), mRespawnEffectPrefab.transform.rotation);
		//Destroy(res, 3);
	}
}