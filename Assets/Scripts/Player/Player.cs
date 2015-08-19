using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	public int mAirMax;
	public int mAirDrain;
	public float mMaxFallSpeed = 10;
	public float mMaxCurrentFallSpeed = 10;
	public bool mAirReg = true;
	public bool mInvulnerable = false;
	public Animator ani;
	public Transform mMeshTrans;
	public int mAirRegFalling;
	public AstroidSpawn mAS;

	private FollowPlayer mfp;
	private float mAirAmount;
	private Rigidbody mRb;
	private bool mIsDead = false;
	private MovementControls mMovementControls;
	public SkinnedMeshRenderer[] skinnedMeshRenderer;
	private float mDashTime;
	private float mDashCDTime;

	private float mStartYValue;
	public int mBoltsCollected;
	public int mCrystalsCollected;

	
	// Use this for initialization
	void Start()
	{
			// keep player along levels
		DontDestroyOnLoad(transform.gameObject);
		
		// init compoments
		mRb = GetComponent<Rigidbody>();
		
		// init internal scrips
		mMovementControls = new MovementControls(ani, null, this, skinnedMeshRenderer);

		// finally extra init
		safeInit();
		mAS = WorldGen.Instance.AstroidSpawn ();
		mfp = InGameCamera.Instance.GetComponent<FollowPlayer>();
	}

	public void StartGame()
	{
		mBoltsCollected = 0;
		mCrystalsCollected = 0;
		mStartYValue = transform.position.y;
	}
	
	// Thism2 created 2015-04-17 : trigger as level specific initaliation for when the level loads 
	public void safeInit()
	{
		// reset air
		mAirAmount = mAirMax;

		// put at level start position, if any
		//transform.position = GameManager.Instance().mPlayerStartPosition.transform.position;
		mIsDead = false;
	}

	public void Dash()
	{
		if(mDashCDTime < Time.time)
		{
			mfp.Dash();
			mMaxCurrentFallSpeed = mMaxFallSpeed + GlobalVariables.Instance.PLAYER_DASH_SPEED;
			mDashTime = Time.time + GlobalVariables.Instance.PLAYER_DASH_SPEED_DELAY;
			mRb.velocity = new Vector3(0,-GlobalVariables.Instance.PLAYER_DASH_SPEED,0);
			mDashCDTime = Time.time + GlobalVariables.Instance.PLAYER_DASH_CD;
		}
	}
	
	void FixedUpdate()
	{
		// jump and hover player
		mAirAmount = mMovementControls.JumpAndHover(mRb, 10);
		
		// move player
		mMovementControls.Move(mRb);

		// hover physics
		mMovementControls.Hover(mRb,10);
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
			mMaxCurrentFallSpeed -= GlobalVariables.Instance.PLAYER_VERTICAL_SPEED_FALLOF;
		}
		mMaxCurrentFallSpeed = Mathf.Max(mMaxFallSpeed, mMaxCurrentFallSpeed);

	}
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Bolts")
		{
			mBoltsCollected += GlobalVariables.Instance.BOLT_VALUE;
			col.gameObject.SetActive(false);
		}
		else if(col.tag == "BoltCluster")
		{
			mBoltsCollected += GlobalVariables.Instance.BOLT_CLUSTER_VALUE;
		}
		else if(col.tag == "SpawnAstroid")
		{
			mAS.gameObject.SetActive(true);
			InGameCamera.Instance.showWarning(true);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.tag == "SpawnAstroid")
		{
			if (false) 
			{
				mAS.gameObject.SetActive(false);
				InGameCamera.Instance.showWarning(false);
			}
		}
	}

	public int colectedBolts()
	{
		return mBoltsCollected;
	}
	
	public int colectedCrystals()
	{
		return mBoltsCollected;
	}
	
	public Vector3 CenterPosition()
	{
		return mRb.worldCenterOfMass;
	}

	public int distance()
	{
		int dist = (int)(transform.position.y - mStartYValue - WorldGen.Instance.fallShift());
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

	void respawn ()
	{
		//ani.SetTrigger("Respawn");
		//GameObject res = (GameObject)GameObject.Instantiate(mRespawnEffectPrefab, transform.position + new Vector3(0, -0.5f, 0), mRespawnEffectPrefab.transform.rotation);
		//Destroy(res, 3);
	}
}