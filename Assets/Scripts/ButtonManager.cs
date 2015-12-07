using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{	
	public static ButtonManager CreateButton(GameObject refObj, string path, string guiName, GUICanvasBase guiBase)
	{
		if (guiBase == null) 
		{
			throw new NullReferenceException();
		}

		ButtonManager x = refObj.AddComponent<ButtonManager> ();
		x.mGUIName = guiName;
		x.mObj = refObj.transform.Find(path).gameObject;
		x.mGUIBase = guiBase;
		return x;
	}

	public GameObject mObj;
	public string mGUIName;
	public GUICanvasBase mGUIBase;

	private ButtonPress mButtonPress;
	private Vector3 mBaseScale = Vector3.zero;
	private Vector3 mBasePosition;
	
	void Awake()
	{
	}
	
	// Use this for initialization
	void Start () 
	{
		mButtonPress = mGUIBase.FindButton(mGUIName);

		if (mObj != null) 
		{
			mBasePosition = mObj.transform.localPosition;
			mBaseScale = mObj.transform.localScale;
			if (mBaseScale.magnitude < 0.001f) 
			{
				throw new NotImplementedException();
			}
		}
		
		if (mObj == null)
		{
			print("ERROR mObj");
			enabled = false;
		}

		if (mButtonPress == null)
		{
			print("ERROR mButtonPress");
			enabled = false;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (!mObj.activeInHierarchy) 
		{
			//mButtonPress.Reset ();
		}
		else 
		{
			mObj.transform.localPosition = mBasePosition;
			mObj.transform.position	+= mButtonPress.PositionOffset();
			mObj.transform.localScale = mBaseScale * mButtonPress.ScaleFactor();
		}
	}
}
