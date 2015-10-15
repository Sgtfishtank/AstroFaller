using UnityEngine;
using System.Collections;

public class SwipeScript : MonoBehaviour
{
	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
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
			else if (fingerStartPos.y < Input.touches[0].position.y) 
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

		/*if (Input.touchCount > 0)
		{			
			foreach (Touch touch in Input.touches)
			{
				switch (touch.phase)
				{
				case TouchPhase.Began :
					/* this is a new touch *
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;
					
				case TouchPhase.Canceled :
					/* The touch is being canceled *
					isSwipe = false;
					break;
					
				case TouchPhase.Ended :
					
					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;
					
					if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
					{
						Vector2 direction = touch.position - fingerStartPos;
						Vector2 swipeType = Vector2.zero;
						
						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
						{
							// the swipe is horizontal:
							swipeType = Vector2.right * Mathf.Sign(direction.x);
						}
						else
						{
							// the swipe is vertical:
							swipeType = Vector2.up * Mathf.Sign(direction.y);
						}
						
						if(swipeType.x != 0.0f)
						{
							if(swipeType.x > 0.0f)
							{
								// MOVE RIGHT
							}
							else
							{
								// MOVE LEFT
							}
						}
						
						if(swipeType.y != 0.0f )
						{
							if(swipeType.y > 0.0f)
							{
								// MOVE UP
							}
							else
							{
								// MOVE DOWN
								if (mPlayer.CanDash())
								{
									mPlayer.Dash();
								}
							}
						}
						
					}
					else
					{
						//mPlayer.Hover();
					}
					
					break;
				}
			}
		}*/
	}
}
