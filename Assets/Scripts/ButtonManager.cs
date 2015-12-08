using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{	
	public static ButtonManager CreateButton(GameObject refObj, string path)
    {
        if (refObj == null)
        {
            throw new NullReferenceException("NO refObj FOUND. path: " + path);
        }

		ButtonManager x = refObj.AddComponent<ButtonManager> ();

        if (refObj.transform.Find(path) == null)
        {
            throw new NullReferenceException("NO mObj FOUND. path: " + path);
        }

		x.mObj = refObj.transform.Find(path).gameObject;

        x.enabled = false;
        return x;
	}

	public GameObject mObj;
	
    private string mGUIName;
    private GUICanvasBase mGUIBase;
	private ButtonPress mButtonPress;
	private Vector3 mBaseScale = Vector3.zero;
    private Vector3 mBasePosition;
    private float mFocusLevel = -1;
	
	void Awake()
	{
	}
	
	// Use this for initialization
	void Start ()
    {
        if (mObj == null)
        {
            throw new NullReferenceException("ERROR mObj NOT FOUND.");
        }

		mBasePosition = mObj.transform.localPosition;
		mBaseScale = mObj.transform.localScale;
		if (mBaseScale.magnitude < 0.001f) 
		{
            throw new NotImplementedException("OBJECT SCALE TO SMALL");
		}
	}

    public void LoadButtonPress(string guiName, GUICanvasBase guiBase)
    {
        mGUIName = guiName;
        mGUIBase = guiBase;

        mButtonPress = mGUIBase.FindButton(mGUIName);

        if (mButtonPress == null)
        {
            throw new NullReferenceException("NO ButtonPress FOUND. mGUIBase: " + mGUIBase + ", mGUIName: " + mGUIName);
        }

        enabled = ((mObj != null) && (mButtonPress != null));
    }

	// Update is called once per frame
	void Update ()
    {
		if (!mObj.activeInHierarchy) 
		{
            return;
		}

        float f = mButtonPress.PressValue();
        if (mFocusLevel == f)
        {
            return;
        }

        if (Mathf.Abs(mFocusLevel - f) < 0.02f)
        {
            return;
        }

        mFocusLevel = f;
        UpdateObj();
	}

    private void UpdateObj()
    {
        mObj.transform.localPosition = mBasePosition;
        if (PlayerData.Instance.CurrentScene() == PlayerData.Scene.IN_GAME)
        {
            mObj.transform.position -= (InGameCamera.Instance.transform.rotation * mButtonPress.PositionOffset());
        }
        else
        {
            mObj.transform.position -= (MenuCamera.Instance.transform.rotation * mButtonPress.PositionOffset());
        }
        mObj.transform.localScale = mBaseScale * mButtonPress.ScaleFactor();
    }

    public void SetBaseOffset(Vector3 mOffset)
    {
        mBasePosition = mOffset;
        mFocusLevel = -1;
    }
}
