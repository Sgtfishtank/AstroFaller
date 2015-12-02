using UnityEngine;
using System.Collections;

public class SwipeScript : MonoBehaviour
{
	private Vector2 fingerStartPos = Vector2.zero;
	private Player mPlayer;
	
	void Start()
	{
		mPlayer = InGame.Instance.Player();
	}
	// Update is called once per frame
	void Update ()
	{	
		if(Input.touchCount > 0)
		{
			switch (Input.touches[0].phase)
			{
			case TouchPhase.Began:
				fingerStartPos = Input.touches[0].position;
				mPlayer.Hover(true);
				break;
			case TouchPhase.Ended:
				mPlayer.Hover(false);
				break;
			}
			if((fingerStartPos - Input.touches[0].position).magnitude < (Screen.height * 0.2f))
			{
				mPlayer.Hover(true);
			}
			else if (fingerStartPos.y > Input.touches[0].position.y) 
			{
				mPlayer.Hover(false);
				if (mPlayer.CanDash())
				{
					mPlayer.Dash();
				}
			}
		}
		else // keyboard controls
		{
			mPlayer.Hover(Input.GetButton("Jump"));

			if (Input.GetButton("Fire1")) 
			{
				if (mPlayer.CanDash())
				{
					mPlayer.Dash();
				}
			}
		}

	}
}
