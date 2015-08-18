using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform mplayer;
	public float zdist;
	public float ydist;
	public float mDashDelay;
	
	private bool mDash =  false;

	// Use this for initialization
	void Start ()
	{
		if (mplayer == null)
		{
			mplayer = WorldGen.Instance.mPlayer.transform;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdatePosition();

		if(mDash)
		{
			if(ydist > 7.1f)
			{
				ydist -= 0.5f;
			}
			else if (mDashDelay == 0)
			{
				mDashDelay = Time.time + GlobalVariables.Instance.PLAYER_DASH_SPEED_DELAY;
			}
			if (mDashDelay < Time.time && mDashDelay != 0)
			{
				mDash = false;
			}
		}
		else
		{
			if (ydist < 9.0f)
			{
				ydist += 0.1f;
			}
			else if( mDashDelay != 0)
			{
				mDashDelay = 0;
			}
		}
	}

	public void Dash()
	{
		mDash = true;
	}
	
	public void UpdatePosition ()
	{
		Vector3 pos = transform.position;
		pos.z = -zdist;
		pos.y = mplayer.position.y + (mplayer.transform.rotation * mplayer.GetComponent<Rigidbody>().centerOfMass).y;
		transform.position = pos;
	}
}
