using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Used by: Player
public class MovementControls
{
	private bool mHoverActive = false;
	private Animator mAni;
	private Player mPlayer;
	private SkinnedMeshRenderer[] skinnedMeshRenderer;
	public float AccelerometerUpdateInterval = 1.0f / 60.0f;
	public float LowPassKernelWidthInSeconds = 1.0f;
	
	private float LowPassFilterFactor;
	int blendSpeed = 5;

	int blendOne = 0;
	bool first;

 // tweakable
	private float lowPassValue = 0;
	public bool temp = false;

	public MovementControls (Animator ani, Transform mesh, Player player, SkinnedMeshRenderer[] skinmesh)
	{
		mPlayer = player;
		LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
		mAni = ani;
		//mMesh = mesh;
		skinnedMeshRenderer = skinmesh;
	}

	
	private float LowPassFilterAccelerometer()
	{
		lowPassValue = Mathf.Lerp(lowPassValue, Input.acceleration.x, LowPassFilterFactor);
		return lowPassValue;
	}
	public float JumpAndHover (Rigidbody mRb, float mAirAmount)
	{
		if (Input.GetButton("Jump") || Input.touchCount >= 1)//checks if the player wants to jump/hover
		{
			while (blendOne < 100.1f)
			{
				for(int i = 0; i < skinnedMeshRenderer.Length; i++)
				{
					skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);

				}
				blendOne += blendSpeed;
			}
			mAni.SetBool("Hover",true);
			first = true;
			if(mAirAmount > 0)//hoverfunction
			{

				mHoverActive = true;
				//mAirAmount -= Time.deltaTime * mPlayer.mAirDrain;
			}
			/*else
			{
				float x =UnityEngine.Random.Range(-10f,10f);
				float y = UnityEngine.Random.Range(-10f,10f);
				mRb.velocity = new Vector3(mRb.velocity.x + x, mRb.velocity.y + y ,0);
				Debug.Log("x " + x + " y " + y);
			}*/

			// clamp to min air
			mAirAmount = Mathf.Max(mAirAmount, 0);
			
			if(mAirAmount <= 0)
			{
				mHoverActive = false;
			}
		}
		else//refill air and reset funktionality
		{
			if(mAirAmount < mPlayer.mAirMax && mPlayer.mAirReg)
			{
				mAirAmount += Time.deltaTime * mPlayer.mAirRegFalling;
				// clamp to max air
				mAirAmount = Mathf.Min(mAirAmount, mPlayer.mAirMax);
			}
			if(first)
			{
				float tamp =  UnityEngine.Random.Range(0f,1f);
				Debug.Log (tamp);
				mAni.CrossFade("Chubby_Tumblin",0,0,tamp);
				mAni.SetBool("Hover", false);
				first= false;
			}
			while(blendOne>-0.1f)
			{
				for(int i = 0; i< skinnedMeshRenderer.Length;i++)
				{
					skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);
				}
				blendOne -= blendSpeed;
			}

			mHoverActive = false;
		}

		return mAirAmount;
	}

	public void Hover(Rigidbody mRb, float airamount)
	{
		if (mHoverActive)
		{
			if(mRb.velocity.y < 0)//slows down the player to hover
			{
				// revese the polarity of the gravity sigularity cap'n!
				mRb.AddForce (new Vector2 (0, -mRb.mass * Physics.gravity.y * 1f));

				// slow down the fall speed
				float force = mRb.mass * (Mathf.Abs(mRb.velocity.y) / Time.fixedDeltaTime);

				mRb.AddForce (new Vector2 (0, Mathf.Clamp(force * 1f, 0, GlobalVariables.Instance.PLAYER_HOVER_FORCE)));

				// stop at alomst standstill
				if(mRb.velocity.y > -0.5f)
				{
					mRb.velocity = new Vector2(mRb.velocity.x, 0);
				}
			}
		}
	}
	
	public void Move(Rigidbody rb)
	{

		//check if the speed is in between the set interval
		if (rb.velocity.x < GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.GetAxisRaw("Horizontal") == 1) 
		{
			rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD * Time.deltaTime), 0));
		}
		if (rb.velocity.x > -GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.GetAxisRaw("Horizontal") == -1)
		{
			rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD * Time.deltaTime), 0));
		}
		if (rb.velocity.x < GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.acceleration.x < 0)
		{
			rb.AddForce (new Vector2((LowPassFilterAccelerometer() * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED * Time.deltaTime), 0));
		}
		if (rb.velocity.x > -GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.acceleration.x > 0)
		{
			rb.AddForce (new Vector2((LowPassFilterAccelerometer() * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED * Time.deltaTime), 0));
		}

		if(rb.velocity.y <= -mPlayer.mMaxCurrentFallSpeed)//max fall speed
		{
			rb.velocity = new Vector2(rb.velocity.x,-mPlayer.mMaxCurrentFallSpeed);
		}
		if(Input.GetAxisRaw("Horizontal") == 0 || LowPassFilterAccelerometer() == 0)//stops player movment on key release
		{
			rb.AddForce( new Vector3(-rb.velocity.x * 60 * Time.deltaTime, 0, 0));
		}
	}

	public bool isHovering ()
	{
		return mHoverActive;
	}

}
