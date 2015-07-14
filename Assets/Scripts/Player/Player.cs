using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	public int mAccelerationSpeedHorizontal = 10;
	public float mHoverForce = 15;
	public float mHorizontalMaxSpeedAir = 10;
	public float mHorizontalMaxSpeedGround = 10;
	public int mAirMax;
	public int mAirDrain;
	public float mMaxFallSpeed = 10;
	public bool mAirReg = true;
	public bool mInvulnerable = false;
	public bool mSpeedHack = false;
	//public Animator ani;
	//public Transform mMeshTrans;
	public int mAirRegGround;
	public int mAirRegFalling;
	public int mMapParts = 100;
	public bool levelEnd=false;
	public int mMultiplier = 1;
	public Vector2 mNextMul;

	public float mSwipeSpeed =1;

	private float mAirAmount;
	private Rigidbody mRb;
	private bool mIsDead = false;
	private MovementControls mMovementControls;
	private float mOffGroundTime;

	
	// Use this for initialization
	void Start()
	{

		// keep player along levels
		DontDestroyOnLoad(transform.gameObject);
		
		// init compoments
		mRb = GetComponent<Rigidbody>();
		
		// init internal scrips
		mMovementControls = new MovementControls(null, null, this);

		// finally extra init
		safeInit();
	}
	
	// Thism2 created 2015-04-17 : trigger as level specific initaliation for when the level loads 
	public void safeInit()
	{
		// reset air
		mAirAmount = mAirMax;

		// put at level start position, if any
		//transform.position = GameManager.Instance().mPlayerStartPosition.transform.position;
		mIsDead = false;
		//mNextMul = GameManager.Instance().mPlayerStartPosition.transform.position;
		mNextMul.y -= mMapParts;
	}
	
	void FixedUpdate()
	{
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

		// jump and hover player
		mAirAmount = mMovementControls.JumpAndHover(mRb, 0);
		if(Input.GetButtonDown("Fire1"))
		{
			mRb.velocity = new Vector3(mRb.velocity.x, /*mRb.velocity.y +*/ mSwipeSpeed);//TODO byt tecken
		}

		// move player
		mMovementControls.Move(mRb, mSpeedHack);

		if(transform.position.y < mNextMul.y)
		{
			mMultiplier++;
			mNextMul.y -= mMapParts;
		}
	}
	
	// Thism2 created 2015-05-21 : returns the amount of collected fire
	public int collectedFire()
	{
		return 0;
	}
	
	// Thism2 created 2015-05-21 : returns the falling multiplier
	public int fallMultiplier ()
	{
		return mMultiplier;
	}

	// Thism2 created 2015-05-21 : returns the amount of air
	public float airAmount()
	{
		return mAirAmount;
	}
	
	// Thism2 created 2015-05-19 : checks if the player is dead
	public bool isDead ()
	{
		return mIsDead;
	}

	// ...
	public void PlayerDead()
	{
		if(!mInvulnerable && !mIsDead)
		{
			mIsDead = true;

			mMultiplier = 1;

			mRb.velocity = new Vector2(0, 0);
			gameObject.SetActive(false);
		}
	}
	
	public void resetFromLastCheckpoint()
	{
		mIsDead = false;
		gameObject.SetActive(true);
		mNextMul.y -= mMapParts;
		mAirAmount = mAirMax;

		respawn();
	}

	
	// Thism2 created 2015-05-12 : respawn effect
	void respawn ()
	{
		//ani.SetTrigger("Respawn");
		//GameObject res = (GameObject)GameObject.Instantiate(mRespawnEffectPrefab, transform.position + new Vector3(0, -0.5f, 0), mRespawnEffectPrefab.transform.rotation);
		//Destroy(res, 3);
	}
}
