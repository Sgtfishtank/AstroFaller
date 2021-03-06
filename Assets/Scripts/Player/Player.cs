using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
	public float mMaxFallSpeed = 10;
	public float mMaxCurrentFallSpeed = 10;
	public bool mInvulnerable = false;
	public Animator mAni;
	public Transform mMeshTrans;
	public bool mUseAirDrain;
	public bool mUseAirReg;

	private SpawnerBase mAS;
	private Collider mCurrentAsterodSpawnCollider;

	private float mAirAmount;
	private bool mHover;

	private FollowPlayer mfp;
	private Rigidbody mRb;
	private float mStartYValue;
	private bool mIsDead = false;
	private bool mPlaying;
	private MovementControls mMovementControls;
	private SkinnedMeshRenderer[] skinnedMeshRenderer;

	private float mDashTime;
	private float mDashCDTime;
	
	private bool doShift = false;
	private float shiftAmount;

	public int mLife;
	public int mBoltsCollected;
	public int mCrystalsCollected;
    public int mPerfectDistanceCollected = 1;
    public int mHighestPerfectDistanceCollected = 1;
    public int mScrapCollected;

	private ParticleManager mBoltParticleManager;
	private ParticleManager mPickupTextManager;

	private GameObject mDash;
	private Collider mLastDmgCollider;
	private float mPerfectDistanceY;
	
	//private float mLastDmgTime;
	//private bool mLastDmgGetLife;
	private int mBurstsLeft;
	private float mInvulnerableTime;
	private float blendSpeed = 400f;
    private float blendOne = 0;

	private AudioInstanceData mDownSwipeSound;
	private AudioInstanceData mHurtHitSound;
	private AudioInstanceData mCoinPickUpSound;
	private AudioInstanceData mInflateSound;
	private AudioInstanceData mDeflateSound;
	private LensFlare mAntenLensFlare;

	// Use this for initialization
	void Awake() 
	{
		// keep player along levels
		mBoltParticleManager = GetComponents<ParticleManager>()[1];
		mPickupTextManager = GetComponents<ParticleManager>()[0];
		// init compoments
		mRb = GetComponent<Rigidbody>();
		mAntenLensFlare = GetComponentInChildren<LensFlare>();
		mAni = transform.Find ("Chubby_Hover").GetComponent<Animator> ();
		mMovementControls = new MovementControls(this);
		mDash = transform.Find("Burst_Trail").gameObject;

		skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>().Where(x => x.name != "Backpack").ToArray();

        mDownSwipeSound = AudioManager.Instance.GetSoundsEvent("DownSwipe/DownSwipe", true, 4);
        mHurtHitSound = AudioManager.Instance.GetSoundsEvent("TakeDamage/TakeDamage", true, 3);
        mCoinPickUpSound = AudioManager.Instance.GetSoundsEvent("ScewsPling/ScrewsPling", true, 4);
        mInflateSound = AudioManager.Instance.GetSoundsEvent("Inflate/Inflate", true, 4);
        mDeflateSound = AudioManager.Instance.GetSoundsEvent("Deflate/Deflate", true, 4);
		mInflateSound.mVolume = 100;
		mDeflateSound.mVolume = 100;

        transform.position = Vector3.zero;
	}

	// Use this for initialization
	void Start()
    {
        int maxParticles = GlobalVariables.Instance.SPAWN_COLLISON_MAX_PARTICLES;
        Transform parent = InGame.Instance.transform.Find("ParticlesGoesHere").transform;
        mPickupTextManager.Load(maxParticles, parent);
        mBoltParticleManager.Load(maxParticles, parent);

        mAS = WorldGen.Instance.BaseSpawner();
        mfp = InGameCamera.Instance.GetComponent<FollowPlayer>();
        mAS = WorldGen.Instance.BaseSpawner();

        Reset();
	}

    public void Reset()
    {
        mDash.SetActive(false);
        mRb.angularVelocity = UnityEngine.Random.insideUnitSphere * mRb.maxAngularVelocity;
        mRb.useGravity = true;
        mRb.isKinematic = false;
        mCurrentAsterodSpawnCollider = null;
        mAntenLensFlare.color = Color.green;

        mPickupTextManager.reset();
        mBoltParticleManager.reset();
    }

	public void StartGame()
    {
        Reset();

        mIsDead = false;
        gameObject.SetActive(true);

        mPlaying = true;
        mAirAmount = PlayerData.Instance.MaxAirTime();
		mBurstsLeft = PlayerData.Instance.MaxBursts();
		mBoltsCollected = 0;
		mCrystalsCollected = 0;
		mPerfectDistanceCollected = 1;
        mHighestPerfectDistanceCollected = 1;
		mLife = PlayerData.Instance.MaxLife();
		mStartYValue = CenterPosition().y;

		transform.position = new Vector3(0, transform.position.y, 0);
		
		UpdatePerfectDistance (false);
		mAS.StopSpawning();
	}

	public void Dash()
	{
		mBurstsLeft--;
		mAni.SetTrigger("Burst");
		mDash.SetActive(true);
		AudioManager.Instance.PlaySoundOnce(mDownSwipeSound);
		mfp.Dash();
		mMaxCurrentFallSpeed = mMaxFallSpeed + GlobalVariables.Instance.PLAYER_DASH_SPEED;
		mDashTime = Time.time + GlobalVariables.Instance.PLAYER_DASH_SPEED_DELAY;
		mRb.velocity += new Vector3(0, -GlobalVariables.Instance.PLAYER_DASH_SPEED, 0);

		// out of bursts 
		if (mBurstsLeft == 0) 
		{
			// start cooldown and reset
			mDashCDTime = Time.time + PlayerData.Instance.BurstCooldown();
			mBurstsLeft = PlayerData.Instance.MaxBursts();
		}
	}

	public void Hover(bool h)
	{
		mHover = h;
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

	public bool CanDash ()
	{
		return (mDashTime < Time.time) && (mDashCDTime < Time.time);
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
		mAirAmount = mMovementControls.JumpAndHover(mRb, mAirAmount, mHover);
		
		// move player
		mMovementControls.Move(mRb);
	}

	void Update()
	{
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			transform.position = new Vector3(0, transform.position.y, 0);
			return;
		}

		/*if (mLastDmgGetLife && (mLastDmgTime < Time.time))
		{
			mLastDmgGetLife = false;
			mLife++;
		}*/

		LifePerk.UpdatePerkValueAnimation(mAni);

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

		// perfect ditsner check
		if (CenterPosition().y < mPerfectDistanceY)
		{
			if (PlayerData.Instance.RegLifeAtPerfectDistance()) 
			{
				mLife++;
			}

			mPerfectDistanceCollected++;
            if (mPerfectDistanceCollected > mHighestPerfectDistanceCollected)
            {
                mHighestPerfectDistanceCollected = mPerfectDistanceCollected;
            }

            SpawnText(transform.position, "x" + mPerfectDistanceCollected);

            UpdatePerfectDistance(true);
		}

		if ((mMaxCurrentFallSpeed > mMaxFallSpeed && mDashTime < Time.time))
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

	public bool isBursting()
	{
		return (mMaxCurrentFallSpeed > mMaxFallSpeed || mDashTime > Time.time);
	}

	public bool IsPlaying()
	{
		return mPlaying;
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

			mBoltParticleManager.Spawn(col.transform.position);

            int boltCollect = GlobalVariables.Instance.BOLT_VALUE * mPerfectDistanceCollected;

            if (isBursting())
                boltCollect = boltCollect * PlayerData.Instance.BurstMultiplier();

            SpawnText(col.transform.position, "+" + boltCollect);

			mBoltsCollected += boltCollect;
			col.gameObject.SetActive(false);
			AudioManager.Instance.PlaySoundOnce(mCoinPickUpSound);
		}
		else if(col.tag == "BoltCluster")
		{
			mBoltsCollected += GlobalVariables.Instance.BOLT_CLUSTER_VALUE;
		}
		else if(col.tag == "SpawnAstroid")
        {
            if (col.GetComponent<ActivateStuff>() != null)
            {
                col.GetComponent<ActivateStuff>().enabled = true;
            }
            else
            {
                mAS.StartSpawning();
                mCurrentAsterodSpawnCollider = col;
            }
		}
		else if(col.tag == "Enemy")
        {
            print("e");
            UpdatePerfectDistance(false);
			PlayerDamage(1);
		}
	}

    private void SpawnText(Vector3 pos, string text)
    {
        GameObject textP = mPickupTextManager.Spawn(pos);
        if (textP != null)
        {
            textP.GetComponentsInChildren<TextMesh>(true)[0].text = text;
            textP.GetComponent<ParticelCleanUp>().Activate(GlobalVariables.Instance.BOLT_TEXT_SHOW_TIME);
        }
    }

	void OnTriggerExit(Collider col)
	{
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

		if(col == mCurrentAsterodSpawnCollider)
		{
			mAS.StopSpawning ();
			mCurrentAsterodSpawnCollider = null;
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		// do nothing if dead
		if ((mIsDead) || (!mPlaying))
		{
			return;
		}

		if (coll.transform.tag == "Enemy")
		{
            if (mLastDmgCollider != coll.collider)
            {
                print("e2");
                mLastDmgCollider = coll.collider;
                //mLastDmgTime = Time.time + 3.0f;
                //mLastDmgGetLife = PlayerData.Instance.RegenerateLifeAfterHit(); 

                UpdatePerfectDistance(false);

                int liveslost = (int)(coll.relativeVelocity.magnitude * 0.5f);
                liveslost = Mathf.Max(1, 1);
                PlayerDamage(liveslost); 
            }
		}
        else if (coll.transform.tag == "Death")
        {
            PlayerDead();
        }
	}

	public Rigidbody Rigidbody()
	{
		return mRb;
	}

	void UpdatePerfectDistance (bool triggerParticles)
	{
        // reset if got hit
        if (!triggerParticles)
        {
            mPerfectDistanceCollected = 1;
        }

		mPerfectDistanceY = CenterPosition().y - GlobalVariables.Instance.PERFECT_DISTANCE_SIZE;
		InGame.Instance.UpdatePerfectDistance(mPerfectDistanceY, triggerParticles);
	}

	void OnCollisionExit(Collision coll)
	{
		if(coll.transform.tag == "Enemy" )
		{
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
	public int LifeRemaining()
	{
		return mLife;
	}
	
	public int colectedCrystals()
	{
		return mCrystalsCollected;
	}

	public int CollectedPerfectDistances ()
	{
		return mPerfectDistanceCollected;
	}

    public int CollectedScraps()
    {
        return mScrapCollected;
    }
	
	public Vector3 CenterPosition()
	{
		return mRb.worldCenterOfMass;
	}

	public int Distance()
	{
		int dist = (int)(CenterPosition().y - mStartYValue - WorldGen.Instance.FallShift());
		return -dist;
	}

	public float AirAmount()
	{
		return mAirAmount;
	}

	public bool isDead ()
	{
		return mIsDead;
	}

	public void PlayerHeal (int heal)
	{
		mLife = Math.Min(mLife + heal, PlayerData.Instance.MaxLife());
	}
	
	public void PlayerDamage(int dmg)
	{
		if ((!mInvulnerable) && (mInvulnerableTime < Time.time))
		{
			mInvulnerableTime = Time.time + PlayerData.Instance.TimeInvurnerableAfterHit();

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
            mPickupTextManager.reset();
			mIsDead = true;
			mRb.isKinematic = true;
			mRb.velocity = new Vector2(0, 0);

			InGame.Instance.PlayerDied();
			InGame.Instance.DeathMenu().Open(Distance(), mBoltsCollected, mScrapCollected, transform.position);
			
			mAS.StopSpawning();

            PlayerData.Instance.depositBolts((int)(PlayerData.Instance.CalculateMultiplier(Distance()) * mBoltsCollected));
            PlayerData.Instance.depositCrystals(colectedCrystals());
            PlayerData.Instance.depositDistance(Distance());

			gameObject.SetActive(false);
		}
	}

	public void ShiftBack (float shift)
	{
		doShift = true;
		shiftAmount = shift;
		transform.position -= new Vector3(0, shift, 0);
		mPerfectDistanceY -= shift;

		mBoltParticleManager.ShiftBack(shift);
		mPickupTextManager.ShiftBack(shift);
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
}