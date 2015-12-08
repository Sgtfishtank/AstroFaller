using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
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
	
	public float mMoveT;
	private bool mPressed;
	private Vector3 mOffset = Vector3.zero;
	private float mScale = 1;
	private FMOD.Studio.EventInstance mPressSound;

	void Awake()
	{
		mPressSound = AudioManager.Instance.GetSoundsEvent("MenuQuestionMark/QuestionMark");
	}

	void Start()
	{
	}

	public void OnPointerDown (PointerEventData eventData) 
	{
		mPressed = true;
	}

	public void OnPointerClick(PointerEventData eventData) 
	{
		AudioManager.Instance.PlaySoundOnce (mPressSound);
	}

	public bool IsPressed() 
	{
		return mPressed;
	}

	public void Reset()
	{
		if (mMoveT > 0) 
		{
			mPressed = false;
			mMoveT = 0;
			UpdateObj (mMoveT);
		}
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
	}

    public float PressValue()
    {
        return mMoveT;
    }
}