using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Used by: Player
public class MovementControls
{
	private bool mHoverActive = false;
	private bool mHoverFailed = false;
	private float mHoverFailedT;

	//private Animator mAni;
	private Player mPlayer;
	//private SkinnedMeshRenderer[] skinnedMeshRenderer;

	//public float AccelerometerUpdateInterval = 0.25f;
	//public float LowPassKernelWidthInSeconds = 1.0f;
	
	//private float LowPassFilterFactor;

	//bool first;

 // tweakable
	//private float lowPassValue = 0;
	//public bool temp = false;

	public MovementControls (Animator ani, Transform mesh, Player player, SkinnedMeshRenderer[] skinmesh)
	{
		mPlayer = player;
		//LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
		//skinnedMeshRenderer = skinmesh;
	}

	
	private void LowPassFilterAccelerometer()
	{
		//lowPassValue = Mathf.Lerp(lowPassValue, Input.acceleration.x, LowPassFilterFactor);
	}

	public float JumpAndHover (Rigidbody mRb, float mAirAmount, bool hover)
	{
		if (hover)//checks if the player wants to jump/hover
		{
			//first = true;

			if (!mHoverActive)
			{
				if (mAirAmount > 0)  // hoverfunction
				{
					StartHover();
				}
			}
			else if (!mHoverFailed)
			{
				if (mAirAmount <= 0)
				{
					FailHover();
				}
			}
		}
		else//refill air and reset funktionality
		{
			if (mHoverActive)
			{
				StopHover();
			}
		}

		// drain and clamp air amount
		if (IsHovering() && mPlayer.DrainAir())
		{
			mAirAmount -= GlobalVariables.Instance.PLAYER_AIR_DRAIN * Time.deltaTime;
		}
		else if ((!IsHovering()) && mPlayer.RegAir())
		{
			mAirAmount += GlobalVariables.Instance.PLAYER_AIR_REG * Time.deltaTime;
		}
		mAirAmount = Mathf.Clamp(mAirAmount, 0, PlayerData.Instance.MaxAirTime());

		return mAirAmount;
	}

	public void Hover(Rigidbody mRb)
	{
		if (IsHovering())
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
		float moveSpeed = GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED;

		if (Input.acceleration.x == 0)
		{
			force = Input.GetAxisRaw("Horizontal");
			moveSpeed = GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD;
		}

		// dampen speed
		if (!IsHoverFailWiggling())
		{
			rb.velocity -= new Vector3 (rb.velocity.x, 0, 0) * 0.5f;
		}

		// add movement
		if (IsHoverFailWiggling())
		{
			// no player monent if hover failed
		}
		else if (IsHovering())
		{
			rb.velocity += new Vector3(force * moveSpeed, 0, 0) * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_HOVER_FACTOR * Time.deltaTime;
		}
		else 
		{
			rb.velocity += new Vector3(force * moveSpeed, 0, 0) * Time.deltaTime;
		}

		Vector3 plVel = rb.velocity;

		float maxSpeed = GlobalVariables.Instance.PLAYER_MAX_HORIZONTAL_MOVESPEED;

		// max horisotal speed
		if (IsHovering()) 
		{
			maxSpeed *= GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_HOVER_FACTOR;
		}

		plVel.x = Mathf.Clamp (plVel.x, -maxSpeed, maxSpeed);

		// max fall speed
		plVel.y = Mathf.Clamp(plVel.y, -mPlayer.mMaxCurrentFallSpeed, 10);

		rb.velocity = plVel;

		Vector3 plPos = rb.worldCenterOfMass;
		
		// clamp position in x axis
		plPos.x = Mathf.Clamp (plPos.x, -GlobalVariables.Instance.PLAYER_MINMAX_X, GlobalVariables.Instance.PLAYER_MINMAX_X);

		rb.transform.position = plPos + (rb.transform.position - rb.worldCenterOfMass);
	}

	public bool IsHovering ()
	{
		return (mHoverActive && (!mHoverFailed));
	}

	public bool IsHoveringFailed()
	{
		return (mHoverActive && mHoverFailed);
	}
	
	public bool IsHoverFailWiggling()
	{
		return (mHoverActive && mHoverFailed && (mHoverFailedT > Time.time));
	}

	void StartHover ()
	{
		mHoverActive = true;
		mPlayer.Inflate ();
	}
	
	void FailHover ()
	{
		Vector3 vel = UnityEngine.Random.insideUnitCircle;
		vel.z = 0;
		vel.Normalize();
		mPlayer.GetComponent<Rigidbody>().velocity += vel * GlobalVariables.Instance.PLAYER_HOVER_FAILED_FORCE * Time.deltaTime;

		mHoverFailed = true;
		mHoverFailedT = Time.time + GlobalVariables.Instance.PLAYER_HOVER_FAILED_TIME;
		mPlayer.Deflate();
	}

	void StopHover ()
	{
		if (!mHoverFailed)
		{
			mPlayer.Deflate();
		}

		mHoverActive = false;
		mHoverFailed = false;
	}
}
