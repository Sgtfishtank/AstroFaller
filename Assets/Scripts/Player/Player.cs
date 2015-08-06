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
	public GameObject mAS;

	private float mAirAmount;
	private Rigidbody mRb;
	private bool mIsDead = false;
	private MovementControls mMovementControls;
	public SkinnedMeshRenderer[] skinnedMeshRenderer;
	private float mDashTime;

	
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
	}
	public void Hover()
	{
		mMovementControls.Hover(mRb,10);
	}
	public void Dash()
	{

		mMaxCurrentFallSpeed = mMaxFallSpeed + GlobalVariables.Instance.PLAYER_DASH_SPEED;
		mDashTime = Time.time + GlobalVariables.Instance.PLAYER_DASH_SPEED_DELAY;
		mRb.AddForce(0,-GlobalVariables.Instance.PLAYER_DASH_SPEED,0);
	}
	
	void FixedUpdate()
	{
		// hover physics
		mMovementControls.Hover(mRb,10);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			Dash();
		}
		//transform.RotateAround(mMeshTrans.position, new Vector3(1,0,0), transform.rotation.z + 80 * Time.deltaTime);
		//transform.RotateAround(mMeshTrans.position, new Vector3(0,1,0), transform.rotation.z + 100 * Time.deltaTime);
		//transform.RotateAround(mMeshTrans.position, new Vector3(0,0,1), transform.rotation.z + 75 * Time.deltaTime);

		// do nothing if dead
		if(mIsDead)
		{
			return;
		}


		// jump and hover player
		mAirAmount = mMovementControls.JumpAndHover(mRb, 10);

		// move player
		mMovementControls.Move(mRb);
		mMovementControls.Hover(mRb,10);
		if(mMaxCurrentFallSpeed > mMaxFallSpeed && mDashTime < Time.time)
		{
			mMaxCurrentFallSpeed -= GlobalVariables.Instance.PLAYER_VERTICAL_SPEED_FALLOF;
		}

	}

	public int colectedBolts()
	{
		return 0;
	}

	public int disance()
	{
		return (int)Mathf.Abs(transform.position.y);
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