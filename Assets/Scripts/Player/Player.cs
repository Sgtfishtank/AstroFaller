using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	public int mAccelerationSpeedHorizontal = 10;
	public float mHoverForce = 15;
	public float mHorizontalMaxSpeedAir = 10;
	public int mAirMax;
	public int mAirDrain;
	public float mMaxFallSpeed = 10;
	public bool mAirReg = true;
	public bool mInvulnerable = false;
	public bool mSpeedHack = false;
	public Animator ani;
	//public Transform mMeshTrans;
	public int mAirRegFalling;
	public int mMapParts = 100;
	public bool levelEnd=false;
	public int mMultiplier = 1;
	public Vector2 mNextMul;
	public GameObject mAS;

	public float mSwipeSpeed =1;

	private float mAirAmount;
	private Rigidbody mRb;
	private bool mIsDead = false;
	private MovementControls mMovementControls;
	public SkinnedMeshRenderer[] skinnedMeshRenderer;

	
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
		mAS = GameObject.Find("AstroidSpawn");
		mAS.SetActive (false);
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
	public void Hover()
	{
		mMovementControls.Hover(mRb,10);
	}
	public void Dash()
	{
		mRb.AddForce(0,-mSwipeSpeed,0);
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
		mAirAmount = mMovementControls.JumpAndHover(mRb, 10);

		// move player
		mMovementControls.Move(mRb, mSpeedHack);

		if(transform.position.y < mNextMul.y)
		{
			mMultiplier++;
			mNextMul.y -= mMapParts;
		}
		mMovementControls.Hover(mRb,10);

	}

	public int collectedFire()
	{
		return 0;
	}

	public int fallMultiplier ()
	{
		return mMultiplier;
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

			mMultiplier = 1;

			mRb.velocity = new Vector2(0, 0);
			gameObject.SetActive(false);
		}
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "SpawnAstroid")
		{
			mAS.SetActive(true);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.tag == "SpawnAstroid")
		{
			mAS.SetActive(false);
		}
	}

	void respawn ()
	{
		//ani.SetTrigger("Respawn");
		//GameObject res = (GameObject)GameObject.Instantiate(mRespawnEffectPrefab, transform.position + new Vector3(0, -0.5f, 0), mRespawnEffectPrefab.transform.rotation);
		//Destroy(res, 3);
	}
}