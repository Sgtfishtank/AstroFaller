using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler// required interface when using the OnPointerDown method.
{
	public enum PressAction
	{
		MoveBack,
		ScaleDown
	}

	public PressAction mPressAction;
	public bool mUseSmoothStep;
	public float mMoveBackDistance;
	public float mScaleDownFactor;
	public GameObject mObj;
	
	private float mMoveT;
	private bool mPressed;
	private Vector3 mBaseScale;
	private Vector3 mBasePosition;

	void Start()
	{
		mBaseScale = mObj.transform.localScale;
		mBasePosition = mObj.transform.localPosition;
	}

	//Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerDown (PointerEventData eventData) 
	{
		mPressed = true;
	}

	//Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerUp (PointerEventData eventData) 
	{
		mPressed = false;
	}


	void Update()
	{
		if (mPressed) 
		{
			mMoveT = Mathf.Clamp01(mMoveT + (Time.deltaTime * GlobalVariables.Instance.BUTTON_PRESS_MOVE_SPEED));

		}
		else 
		{
			mMoveT = Mathf.Clamp01(mMoveT - (Time.deltaTime * GlobalVariables.Instance.BUTTON_PRESS_MOVE_SPEED));
		}	

		float movingTime = mMoveT;
		if (mUseSmoothStep)
		{
			movingTime = Mathf.SmoothStep(0, 1, mMoveT);
		}
		
		
		switch (mPressAction) 
		{
		case PressAction.MoveBack:
			mObj.transform.localPosition = Vector3.Lerp(mBasePosition, mBasePosition - new Vector3(0, 0, mMoveBackDistance), movingTime);
			break;
		case PressAction.ScaleDown:
			mObj.transform.localScale = Vector3.Lerp(mBaseScale, mBaseScale * mScaleDownFactor, movingTime);
			break;
		default:
			print("ERROR in Update PressAction" + mPressAction);
			break;
		}
	}
}