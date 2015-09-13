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
	public int mPerfectDistanceCollected;
	public GameObject boltParticles;
	public GameObject mDash;
	private Collider mLastDmgCollider;
	private float mPerfectDistanceY;
	
	private float mLastDmgTime;
	private bool mLastDmgGetLife;
	private float blendSpeed = 400f;
	private float blendOne = 0;

	private FMOD.Studio.EventInstance mDownSwipeSound;
	private FMOD.Studio.EventInstance mHurtHitSound;
	private FMOD.Studio.EventInstance mCoinPickUpSound;
	private FMOD.Studio.EventInstance mInflateSound;
	private FMOD.Studio.EventInstance mDeflateSound;
	public LensFlare mAntenLensFlare;
	
	// Use this for initialization
	void Awake() 
	{
		// keep player along levels 
		DontDestroyOnLoad(transform.gameObject);
		
		// init compoments
		mRb = GetComponent<Rigidbody>();
		mAntenLensFlare = GetComponentInChildren<LensFlare>();
		mAni = transform.Find ("Chubby_Hover").GetComponent<Animator> ();
		mDownSwipeSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Downswipe/DownSwipe");
		mHurtHitSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/TakeDamage/TakeDamage1");
		mCoinPickUpSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Screws/ScrewsPling2");
		mInflateSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Inflate/Inflate");
		mDeflateSound = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Deflate/Deflate");
		
		// init internal scrips
		mMovementControls = new MovementControls(null, null, this, skinnedMeshRenderer);
		
		// reset air
		mDash = transform.Find("Burst_Trail").gameObject;

		// put at level start position, if any
		mIsDead = false;
		mPlaying = false;
	}

	// Use this for initialization
	void Start()
	{
		mAS = WorldGen.Instance.AstroidSpawn ();
		mfp = InGameCamera.Instance.GetComponent<FollowPlayer>();
		mDash.SetActive(false);
	}

	public void StartGame()
	{
		mAirAmount = PlayerData.Instance.MaxAirTime();
		mRb.isKinematic = false;
		mIsDead = false;

		mBoltsCollected = 0;
		mCrystalsCollected = 0;
		mPerfectDistanceCollected = 0;
		mLife = PlayerData.Instance.MaxLife();
		mStartYValue = CenterPosition().y;
		mPlaying = true;
		mPerfectDistanceY = CenterPosition().y - GlobalVariables.Instance.PERFECT_DISTANCE_SIZE;
		InGame.Instance.UpdatePerfectDistance(mPerfectDistanceY);
	}

	public void Dash()
	{
		if(mDashCDTime < Time.time)
		{
			mAni.SetTrigger("Burst");
			mDash.SetActive(true);
			AudioManager.Instance.PlaySoundOnce(mDownSwipeSound);
			mfp.Dash();
			mMaxCurrentFallSpeed = mMaxFallSpeed + GlobalVariables.Instance.PLAYER_DASH_SPEED;
			mDashTime = Time.time + PlayerData.Instance.BurstDelay();
			mRb.velocity += new Vector3(0,-GlobalVariables.Instance.PLAYER_DASH_SPEED, 0);
			mDashCDTime = Time.time + PlayerData.Instance.BurstCooldown();
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
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

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

		if (mLastDmgGetLife && (mLastDmgTime < Time.time))
		{
			mLastDmgGetLife = false;
			mLife++;
		}

		if ((mMovementControls.IsHovering()) && (blendOne < 100))
		{
			blendOne = Mathf.Clamp(blendOne + (blendSpeed * Time.deltaTime), 0, 100);
			UpdateMeshBlend();
		}
		else if ((!mMovementControls.IsHovering()) && (blendOne > 0))
		{
			blendOne = Mathf.Clamp(blendOne - (blendSpeed * Time.deltaTime), 0, 100);
			UpdateMeshBlend();
		}

		if (CenterPosition().y < mPerfectDistanceY)
		{
			UpdatePerfectDistance();
			mPerfectDistanceCollected++;
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
	
	void UpdateMeshBlend()
	{
		for(int i = 0; i< skinnedMeshRenderer.Length;i++)
		{
			skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);
		}
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
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

		if (col.tag == "Bolts")
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
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

		if(col.tag == "SpawnAstroid")
		{
			mAS.gameObject.SetActive(false);
		}
	}
	void OnCollisionEnter(Collision coll)
	{
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

		if ((coll.transform.tag == "Enemy") && (mLastDmgCollider != coll.collider))
		{

			mLastDmgCollider = coll.collider;
			mLastDmgTime = Time.time + 3.0f;
			mLastDmgGetLife = PlayerData.Instance.RegenerateLifeAfterHit(); 

			UpdatePerfectDistance();

			int liveslost = (int)(coll.relativeVelocity.magnitude * 0.5f);
			liveslost = Mathf.Max(1, 1);
			PlayerDamage(liveslost);
		}
	}

	void UpdatePerfectDistance ()
	{
		mPerfectDistanceY = CenterPosition().y - GlobalVariables.Instance.PERFECT_DISTANCE_SIZE;
		InGame.Instance.UpdatePerfectDistance(mPerfectDistanceY);
	}

	void OnCollisionExit(Collision coll)
	{
		if(coll.transform.tag == "Enemy" )
		{
			//PlayerDamage(1);
		}
	}
	
	public bool DrainAir()
	{
		bool unlimitedAir = (PlayerData.Instance.UnlimitedAirOneLife() && (mLife == 1));
		return (mUseAirDrain && (!unlimitedAir));
	}

	public bool RegAir()
	{
		return mUseAirReg;
	}

	public int colectedBolts()
	{
		return mBoltsCollected;
	}
	
	public int colectedCrystals()
	{
		return mCrystalsCollected;
	}

	public int CollectedPerfectDistances ()
	{
		return mPerfectDistanceCollected;
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
		if (mLife >= 3)
		{
			mAntenLensFlare.color = Color.green;
		}
		else if (mLife >= 2)
		{
			mAntenLensFlare.color = Color.yellow;
		}
		else if (mLife <= 1)
		{
			mAntenLensFlare.color = Color.red;
		}
		AudioManager.Instance.PlaySoundOnce(mHurtHitSound);
	}

	public void PlayerDead()
	{
		if(!mInvulnerable && !mIsDead)
		{
			mIsDead = true;
			mRb.isKinematic = true;
			mRb.velocity = new Vector2(0, 0);
			InGame.Instance.mDeathMenu.SetActive(true);
			GUICanvas.Instance.setEnableDeathMenu(true);

			Vector3 a = gameObject.transform.position;
			a.x = 0;
			a.y = InGameCamera.Instance.transform.position.y +3.5f;
			a.z = InGame.Instance.mDeathMenu.transform.position.z;
			InGame.Instance.mDeathMenu.transform.position = a;



			//gameObject.SetActive(false);

			Invoke("DepositData",0.1f);
		}
	}

	public void DepositData()
	{
		PlayerData.Instance.depositBolts(colectedBolts());
		mBoltsCollected = 0;
		
		PlayerData.Instance.depositCrystals(colectedCrystals());
		mCrystalsCollected = 0;
		
		PlayerData.Instance.depositDistance(distance());
		mStartYValue = CenterPosition().y;
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