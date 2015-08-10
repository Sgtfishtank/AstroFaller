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

			mHoverActive = false;
		}

		return mAirAmount;
	}

	public void Hover(Rigidbody mRb, float airamount)
	{
		if (mHoverActive)
		{
			// revese the polarity of the gravity sigularity cap'n!
			mRb.AddForce(new Vector3(0, -mRb.mass * Physics.gravity.y * GlobalVariables.Instance.PLAYER_HOVER, 0), ForceMode.Force);
			if(mRb.velocity.y < 0)//slows down the player to hover
			{

				// slow down the fall speed
				float force = mRb.mass * (Mathf.Abs(mRb.velocity.y) / Time.fixedDeltaTime);

				//if(mRb.velocity.y <= -0.5f)
				{
					mRb.AddForce(new Vector3(0, force * GlobalVariables.Instance.PLAYER_HOVER_FORCE, 0));
				}

				// stop at alomst standstill
				if(mRb.velocity.y > -0.5f)
				{
					//mRb.velocity = new Vector2(mRb.velocity.x, 0);
				}
			}
			Debug.Log(mRb.velocity.y);
		}
	}
	
	public void Move(Rigidbody rb)
	{
		LowPassFilterAccelerometer ();
		//check if the speed is in between the set interval
		if (rb.velocity.x < GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.GetAxisRaw("Horizontal") == 1) 
		{
			if(rb.velocity.x < 0)
			{
				rb.velocity += (new Vector3((Input.GetAxisRaw("Horizontal") * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD * Time.deltaTime- rb.velocity.x *.10f), 0,0));
			}
			else 
			rb.velocity += (new Vector3((Input.GetAxisRaw ("Horizontal") * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD * Time.deltaTime), 0,0));
		}
		if (rb.velocity.x > -GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.GetAxisRaw("Horizontal") == -1)
		{
			if(rb.velocity.x > 0)
			{
				rb.velocity += (new Vector3((Input.GetAxisRaw("Horizontal") * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD * Time.deltaTime- rb.velocity.x *.10f), 0,0));
			}
			else 
			rb.velocity += (new Vector3((Input.GetAxisRaw ("Horizontal") * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_KEYBORD * Time.deltaTime), 0,0));
		}
		if (rb.velocity.x < GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.acceleration.x < 0)
		{
			if(rb.velocity.x > 0)
			{
				rb.velocity += (new Vector3((Input.acceleration.x * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED * Time.deltaTime- rb.velocity.x *.10f), 0,0));
			}
			else 
				rb.velocity += (new Vector3((Input.acceleration.x * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED * Time.deltaTime), 0,0));
		}
		if (rb.velocity.x > -GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED && Input.acceleration.x > 0)
		{
			if(rb.velocity.x < 0)
			{
				rb.velocity += (new Vector3((Input.acceleration.x * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED * Time.deltaTime- rb.velocity.x *.10f), 0,0));
			}
			else 
				rb.velocity +=  (new Vector3((Input.acceleration.x * GlobalVariables.Instance.PLAYER_HORIZONTAL_MOVESPEED * Time.deltaTime), 0,0));
		}

		if(rb.velocity.y <= -mPlayer.mMaxCurrentFallSpeed)//max fall speed
		{
			rb.velocity = new Vector2(rb.velocity.x,-mPlayer.mMaxCurrentFallSpeed);
		}
		if(Input.GetAxisRaw("Horizontal") == 0 || Input.acceleration.x == 0)//stops player movment on key release
		{
			rb.AddForce( new Vector3(-rb.velocity.x * 90 * Time.deltaTime, 0, 0));
		}
	}

	public bool isHovering ()
	{
		return mHoverActive;
	}

}
