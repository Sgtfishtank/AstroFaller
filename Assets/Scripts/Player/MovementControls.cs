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
	public float AccelerometerUpdateInterval = 0.25f;
	public float LowPassKernelWidthInSeconds = 1.0f;
	
	private float LowPassFilterFactor;
	float blendSpeed = 400f;

	float blendOne = 0;
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

	
	private void LowPassFilterAccelerometer()
	{
		lowPassValue = Mathf.Lerp(lowPassValue, Input.acceleration.x, LowPassFilterFactor);

	}
	public float JumpAndHover (Rigidbody mRb, float mAirAmount)
	{
		if (Input.GetButton("Jump") || Input.touchCount >= 1)//checks if the player wants to jump/hover
		{
			if (blendOne < 100.1f)
			{
				blendOne =Mathf.Clamp(blendOne+blendSpeed * Time.deltaTime,0,100);
				for(int i = 0; i < skinnedMeshRenderer.Length; i++)
				{
					skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);

				}

			}
			mAni.SetBool("Hover",true);
			first = true;
			if(mAirAmount > 0)//hoverfunction
			{
				if (mHoverActive == false)
				{
					AudioManager.Instance.PlaySoundOnce(FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Inflate/Inflate"));
				}
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
				if (mHoverActive == true)
				{
					AudioManager.Instance.PlaySoundOnce(FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Deflate/Deflate"));
				}
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
				mAni.CrossFade("Chubby_Tumblin",0.1f,0,tamp);
				mAni.SetBool("Hover", false);
				first= false;
			}
			if(blendOne>-0.1f)
			{
				blendOne =Mathf.Clamp(blendOne-blendSpeed * Time.deltaTime,0,100);
				for(int i = 0; i< skinnedMeshRenderer.Length;i++)
				{
					skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);
				}

			}
			
			if (mHoverActive == true)
			{
				AudioManager.Instance.PlaySoundOnce(FMOD_StudioSystem.instance.GetEvent("event:/Sounds/Deflate/Deflate"));
			}
			mHoverActive = false;
		}

		return mAirAmount;
	}

	public void Hover(Rigidbody mRb, float airamount)
	{
		if (mHoverActive)
		{
			if(mRb.velocity.y < 0) //slows down the player to hover
			{
				// revese the polarity of the gravity sigularity cap'n!
				mRb.AddForce(-Physics.gravity * mRb.mass);

				// slow down the fall speed
				float force = mRb.mass * (Mathf.Abs(mRb.velocity.y + GlobalVariables.Instance.PLAYER_MIN_HOVER_SPEED) / Time.fixedDeltaTime);
				//        f = m        *               a

				if(mRb.velocity.y <= -GlobalVariables.Instance.PLAYER_MIN_HOVER_SPEED)
				{
					mRb.AddForce(new Vector3(0, force * GlobalVariables.Instance.PLAYER_HOVER_FORCE, 0));
				}
			}
		}
	}
	
	public void Move(Rigidbody rb)
	{
		LowPassFilterAccelerometer ();
		
		float force = Input.acceleration.x;
		float speed = GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED;

		if (Input.acceleration.x == 0)
		{
			force = Input.GetAxisRaw("Horizontal");
			speed = GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD;
		}

		rb.velocity -= new Vector3 (rb.velocity.x, 0, 0) * 0.5f;

		//check if the speed is in between the set interval
		//if (/*rb.velocity.x < GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED &&*/ (force > 0)) 
		{
		//	rb.position += new Vector3(force * speeed, 0, 0) * Time.deltaTime;
		}

		//if (/*rb.velocity.x > -GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED &&*/ (force < 0))

			if(mHoverActive)
				rb.velocity += new Vector3(force * (speed/2), 0, 0) * Time.deltaTime;
			else
				rb.velocity += new Vector3(force * speed, 0, 0) * Time.deltaTime;

		
		// max horisotal speed
		if (rb.velocity.x > GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED)
		{
			rb.velocity = new Vector2(GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED, rb.velocity.y);
		}
		else if (rb.velocity.x < -GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED)
		{
			rb.velocity = new Vector2(-GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED, rb.velocity.y);
		}

		// max fall speed
		if(rb.velocity.y < -mPlayer.mMaxCurrentFallSpeed)
		{
			rb.velocity = new Vector2(rb.velocity.x, -mPlayer.mMaxCurrentFallSpeed);
		}

		//if(Input.GetAxisRaw("Horizontal") == 0 || Input.acceleration.x == 0)//stops player movment on key release
		{
			//rb.AddForce( new Vector3(-rb.velocity.x * 90 * Time.deltaTime, 0, 0));
		}

		// clamp position in x axis
		Vector3 pl2 = rb.transform.position + (rb.transform.rotation * rb.centerOfMass);
		pl2.x = Mathf.Clamp (pl2.x, -GlobalVariables.Instance.PLAYER_MINMAX_X, GlobalVariables.Instance.PLAYER_MINMAX_X);
		rb.transform.position = pl2 - (rb.transform.rotation * rb.centerOfMass);
	}

	public bool isHovering ()
	{
		return mHoverActive;
	}

}
