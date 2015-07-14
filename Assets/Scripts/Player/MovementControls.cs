using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Used by: Player
public class MovementControls
{
	private bool mHoverActive = false;
	//private Animator mAni;
	private Player mPlayer;
	/*private Transform mMesh;
	private int defaultRotation = 180;
	private float currentRotation = 180f;
	private int rotationVelocity = 2000;*/
	
	public MovementControls (Animator ani, Transform mesh, Player player)
	{
		mPlayer = player;
		//mAni = ani;
		//mMesh = mesh;
	}
	
	public float JumpAndHover (Rigidbody mRb, float mAirAmount)
	{
		if (Input.GetButtonDown("Jump"))//checks if the player wants to jump/hover
		{

			if(mAirAmount > 0)//hoverfunction
			{
				mHoverActive = true;
				if(mRb.velocity.y < 0)//slows down the player to hover
				{
				}
				//mAirAmount -= Time.deltaTime * mPlayer.mAirDrain * mPlayer.getCurrentTalisman().getAirDrain();
			}
			else
			{
				float x =UnityEngine.Random.Range(-10f,10f);
				float y = UnityEngine.Random.Range(-10f,10f);
				mRb.velocity = new Vector3(mRb.velocity.x + x, mRb.velocity.y + y ,0);
				Debug.Log("x " + x + " y " + y);
			}

			// clamp to min air
			mAirAmount = Mathf.Max(mAirAmount, 0);
			
			if(mAirAmount <= 0)
			{
				mHoverActive = false;
			}
		}
		else//refill air and reset funktionality
		{
			/*if(mAirAmount < mPlayer.mAirMax && mPlayer.mAirReg)
			{
				if(!mOnGround)
				{
					mAirAmount += Time.deltaTime * mPlayer.mAirRegFalling * mPlayer.getCurrentTalisman().getAirMultiplier();
				}
				else
				{
					mAirAmount += Time.deltaTime * mPlayer.mAirRegGround * mPlayer.getCurrentTalisman().getAirMultiplier();
				}

				// clamp to max air
				mAirAmount = Mathf.Min(mAirAmount, mPlayer.mAirMax);
			}*/

			mHoverActive = false;
		}
//		mAni.SetBool("Hovering", mHoverActive);

		return mAirAmount;
	}
	
	// Thism2 created 2015-05-12 : do physics hover action
	public void Hover(Rigidbody mRb, float airamount)
	{
		if (mHoverActive)
		{
			if(mRb.velocity.y < 0)//slows down the player to hover
			{
				// revese the polarity of the gravity sigularity cap'n!
				mRb.AddForce (new Vector2 (0, -mRb.mass * Physics2D.gravity.y * 1f));

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

	// Sgtfishtank, Thism2 created 2015-04-23 : move player
	public void Move(Rigidbody rb, bool speedHack)
	{
		//Quaternion degrees = Quaternion.Euler(0, CalculateRotation(onGround), 0);
		//mMesh.localRotation = Quaternion.Slerp(mMesh.localRotation, degrees, rotationVelocity * Time.deltaTime);

		//check if the speed is in between the set interval
		if(true)
		{
			if (speedHack)
			{
				if (rb.velocity.x < (mPlayer.mHorizontalMaxSpeedAir * 10) && Input.GetAxisRaw("Horizontal") == 1) 
				{
					rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime) * 10, 0));
				}
				if (rb.velocity.x > (-mPlayer.mHorizontalMaxSpeedAir * 10) && Input.GetAxisRaw("Horizontal") == -1)
				{
					rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime) * 10, 0));
				}
			}
			else
			{
				if (rb.velocity.x < mPlayer.mHorizontalMaxSpeedAir && Input.GetAxisRaw("Horizontal") == 1) 
				{
					rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime), 0));
				}
				if (rb.velocity.x > -mPlayer.mHorizontalMaxSpeedAir && Input.GetAxisRaw("Horizontal") == -1)
				{
					rb.AddForce (new Vector2((Input.GetAxisRaw ("Horizontal") * mPlayer.mAccelerationSpeedHorizontal * Time.deltaTime), 0));
				}
			}
		}

		if(rb.velocity.y <= -mPlayer.mMaxFallSpeed)//max fall speed
		{
			rb.velocity = new Vector2(rb.velocity.x,-mPlayer.mMaxFallSpeed);
		}
		/*if(Input.GetAxisRaw("Horizontal") == 0 && !onGround)//stops player movment on key release
		{
			rb.AddForce( new Vector2(-rb.velocity.x * 60 * Time.deltaTime, 0));
		}
		else if(Input.GetAxisRaw("Horizontal") == 0 && onGround)
		{
			rb.velocity = new Vector2(0,rb.velocity.y);
		}*/
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
