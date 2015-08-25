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
	private Vector3 mOffset;
	private float mScale;

	void Start()
	{
	}

	public void Init()
	{
		ButtonPress[] bp = GetComponents<ButtonPress>();
		if (bp.Length < 2)
		{
			mObj = GUICanvas.Instance.GUIObject(name);
		}
		else
		{
			for (int i = 0; i < bp.Length; i++) 
			{
				if (bp[i] == this) 
				{
					mObj = GUICanvas.Instance.GUIObject(name + " " + i);
				}
			}
		}

		if (mObj != null)
		{
			mBaseScale = mObj.transform.localScale;
			mBasePosition = mObj.transform.localPosition;
		}
	}

	public void OnPointerDown (PointerEventData eventData) 
	{
		mPressed = true;
	}

	public bool IsPressed() 
	{
		return mPressed;
	}

	void OnDisable() 
	{
	}
	
	void OnEnable() 
	{
		/*if (mBaseScale == Vector3.zero)
		{
			mBaseScale = mObj.transform.localScale;
			mBasePosition = mObj.transform.localPosition;
		}*/
		
		mPressed = false;
		mMoveT = 0;
		UpdateObj (mMoveT);
	}

	public void OnPointerUp (PointerEventData eventData) 
	{
		mPressed = false;
	}

	public Vector3 PositionOffset ()
	{
		return mOffset;
	}
	
	public float ScaleFactor ()
	{
		return mScale;
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
			mOffset = Vector3.Lerp (Vector3.zero, Vector3.zero - new Vector3(0, 0, mMoveBackDistance), movingTime);
			mScale = 1;
			break;
		case PressAction.ScaleDown:
			mOffset = Vector3.zero;
			mScale = Mathf.Lerp (1, mScaleDownFactor, movingTime);
			break;
		default:
			print ("ERROR in Update PressAction" + mPressAction);
			break;
		}

		if (mObj != null)
		{
			mObj.transform.localPosition = mBasePosition + mOffset;
			mObj.transform.localScale = mBaseScale * mScale;
		}
	}
}