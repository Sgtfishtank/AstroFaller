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
	/*private Transform mMesh;
	private int defaultRotation = 180;
	private float currentRotation = 180f;
	private int rotationVelocity = 2000;*/
	public float AccelerometerUpdateInterval = 1.0f / 60.0f;
	public float LowPassKernelWidthInSeconds = 1.0f;
	
	private float LowPassFilterFactor;
	int blendSpeed = 2;

	int blendOne = 0;

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
			for(int i = 0; i < skinnedMeshRenderer.Length; i++)
			{
				if (blendOne < 100.1f)
				{
					skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);
					blendOne += blendSpeed;
				}
			}
			mAni.SetBool("Hover",true);
			if(mAirAmount > 0)//hoverfunction
			{

				mHoverActive = true;
				if(mRb.velocity.y < 0)//slows down the player to hover
				{
				}
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
			mAni.SetBool("Hover", false);
			for(int i = 0; i< skinnedMeshRenderer.Length;i++)
			{
				if (blendOne > -0.1f)
				{
					skinnedMeshRenderer[i].SetBlendShapeWeight (0, blendOne);
					blendOne -= blendSpeed;
				}
			}

			mHoverActive = false;
		}
//		mAni.SetBool("Hovering", mHoverActive);

		return mAirAmount;
	}

	public void Hover(Rigidbody mRb, float airamount)
	{
		if (mHoverActive)
		{
			Debug.Log("hej");
			if(mRb.velocity.y < 0)//slows down the player to hover
			{
				// revese the polarity of the gravity sigularity cap'n!
				mRb.AddForce (new Vector2 (0, -mRb.mass * Physics.gravity.y * 1f));

				// slow down the fall speed
				float force = mRb.mass * (Mathf.Abs(mRb.velocity.y) / Time.fixedDeltaTime);

				mRb.AddForce (new Vector2 (0, Mathf.Clamp(force * 1f, 0, mPlayer.mHoverForce)));

				// stop at alomst standstill
				if(mRb.velocity.y > -0.5f)
				{
					mRb.velocity = new Vector2(mRb.velocity.x, 0);
				}
			}
		}
	}
	
	public void Move(Rigidbody rb, bool speedHack)
	{
		//Quaternion degrees = Quaternion.Euler(0, CalculateRotation(onGround), 0);
		//mMesh.localRotation = Quaternion.Slerp(mMesh.localRotation, degrees, rotationVelocity * Time.deltaTime);

		//check if the speed is in between the set interval
		if(true)
		{
			if (rb.velocity.x < mPlayer.mHorizontalMaxSpeedAir && Input.GetAxisRaw("Horizontal") == 1) 
			{
				rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime), 0));
			}
			if (rb.velocity.x > -mPlayer.mHorizontalMaxSpeedAir && Input.GetAxisRaw("Horizontal") == -1)
			{
				rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime), 0));
			}
			if (rb.velocity.x > -mPlayer.mHorizontalMaxSpeedAir && Input.acceleration.x < 0)
			{
				rb.AddForce (new Vector2((LowPassFilterAccelerometer() * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime), 0));
			}
			if (rb.velocity.x > -mPlayer.mHorizontalMaxSpeedAir && Input.acceleration.x > 0)
			{
				rb.AddForce (new Vector2((LowPassFilterAccelerometer() * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime), 0));
			}
		}

		if(rb.velocity.y <= -mPlayer.mMaxFallSpeed)//max fall speed
		{
			rb.velocity = new Vector2(rb.velocity.x,-mPlayer.mMaxFallSpeed);
		}
		if(Input.GetAxisRaw("Horizontal") == 0)//stops player movment on key release
		{
			rb.AddForce( new Vector2(-rb.velocity.x * 60 * Time.deltaTime, 0));
		}
	}

	public bool isHovering ()
	{
		return mHoverActive;
	}
	
/*	private float CalculateRotation(bool onGround)
	{
		int speed = 25;
		int rotationOffset = 15;

		if(currentRotation<=270 && currentRotation >= 90 && onGround)// on ground rotation
		{
			currentRotation -= Input.GetAxisRaw("Horizontal") * speed;
		}

		if(currentRotation <= 195 && currentRotation >= 165 && !onGround && Input.GetAxisRaw("Horizontal") != 0)//in air rotation
		{
			currentRotation -= Input.GetAxisRaw("Horizontal") * (speed-22);
		}
		//rotates the player to a semi defalut position in air
		else if(currentRotation > 195  && !onGround && Input.GetAxisRaw("Horizontal") == -1)
		{
			currentRotation -=speed-22;
		}
		//rotates the player to a semi defalut position in air
		else if(currentRotation < 165  && !onGround && Input.GetAxisRaw("Horizontal") == -1)
		{
			currentRotation +=speed-22;
		}
		//rotate player to deafalut position unless one of the animations are playing
		else if(!onGround && currentRotation != defaultRotation && !(mAni.GetCurrentAnimatorStateInfo(0).IsName("Jump1")
		   || mAni.GetCurrentAnimatorStateInfo(0).IsName("Jump2") || mAni.GetCurrentAnimatorStateInfo(0).IsName("StartFalling")
		   || mAni.GetCurrentAnimatorStateInfo(0).IsName("Landing")|| mAni.GetCurrentAnimatorStateInfo(0).IsName("Idel")
		   || mAni.GetCurrentAnimatorStateInfo(0).IsName("Running")) && Input.GetAxisRaw("Horizontal") == 0)
		{
			if (currentRotation > (180 - rotationOffset) && currentRotation < (180 + rotationOffset))
			{
				// do nothing
			}
			else if(currentRotation>defaultRotation)
			{
				currentRotation -= speed-22;
			}
			else
			{
				currentRotation += speed-22;
			}
		}

		if(currentRotation >270)
		{
			currentRotation = 270;
		}
		else if(currentRotation <90)
		{
			currentRotation = 90;
		}

		return currentRotation;
	}*/
}
