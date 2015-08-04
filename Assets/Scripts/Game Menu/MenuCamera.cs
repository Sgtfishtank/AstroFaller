using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour 
{
	public Vector3 mCameraOffset;
	public float mCameraMoveSpeed;

	private GameObject mTargetMenuPosition;
	private bool mMoving;

	// Use this for initialization
	void Start () 
	{
		mMoving = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mMoving) 
		{
			transform.position = Vector3.Lerp(transform.position, mTargetMenuPosition.transform.position + mCameraOffset, Time.deltaTime * mCameraMoveSpeed);

			// snap and finish movement
			if (Vector3.Distance(transform.position, mTargetMenuPosition.transform.position) < 0.1f)
			{
				transform.position = mTargetMenuPosition.transform.position;
				mMoving = false;
			}
		}
	}

	public void StartMove(GameObject menuPosition)
	{
		mTargetMenuPosition = menuPosition;
		mMoving = true;
	}
}
