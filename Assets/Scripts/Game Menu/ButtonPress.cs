using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
	private Vector3 mBaseScale = Vector3.zero;
	private Vector3 mBasePosition;

	void Start()
	{
		if (mObj != null)
		{
			mBaseScale = mObj.transform.localScale;
			mBasePosition = mObj.transform.localPosition;
		}
		else
		{
			enabled = false;	
		}
	}

	public void OnPointerDown (PointerEventData eventData) 
	{
		mPressed = true;
	}

	void OnDisable() 
	{
	}
	
	void OnEnable() 
	{
		if (mObj == null)
		{
			return;
		}
		
		if (mBaseScale == Vector3.zero)
		{
			mBaseScale = mObj.transform.localScale;
			mBasePosition = mObj.transform.localPosition;
		}
		
		mPressed = false;
		mMoveT = 0;
		UpdateObj (mMoveT);
	}

	public void OnPointerUp (PointerEventData eventData) 
	{
		mPressed = false;
	}
	
	void Update()
	{
		if (mPressed) 
		{
			mMoveT = Mathf.Clamp01(mMoveT + (Time.deltaTime / GlobalVariables.Instance.BUTTON_PRESS_MOVE_TIME));
		}
		else 
		{
			mMoveT = Mathf.Clamp01(mMoveT - (Time.deltaTime / GlobalVariables.Instance.BUTTON_PRESS_MOVE_TIME));
		}

		UpdateObj (mMoveT);
	}

	void UpdateObj(float moveT)
	{
		float movingTime = moveT;

		if (mUseSmoothStep)
		{
			movingTime = Mathf.SmoothStep(0, 1, mMoveT);
		}

		switch (mPressAction) 
		{
		case PressAction.MoveBack:
			mObj.transform.localPosition = Vector3.Lerp (mBasePosition, mBasePosition - new Vector3 (0, 0, mMoveBackDistance), movingTime);
			break;
		case PressAction.ScaleDown:
			mObj.transform.localScale = Vector3.Lerp (mBaseScale, mBaseScale * mScaleDownFactor, movingTime);
			break;
		default:
			print ("ERROR in Update PressAction" + mPressAction);
			break;
		}
	}
}