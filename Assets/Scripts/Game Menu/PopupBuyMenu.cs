﻿using UnityEngine;
using System.Collections;

public class PopupBuyMenu : MonoBehaviour 
{
	public GameObject mPrefab;

	private bool mOpen;
	private TextMesh mTitleText;
	private TextMesh mDescriptionText;
	private TextMesh mCurrentText;
	private TextMesh mNextText;
	private GameObject mObjToBuy;
	//private TextMesh mCostBoltsText;
	//private TextMesh mCostCrystalsText;

	void Awake()
	{
		GameObject a = GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		a.transform.localScale = mPrefab.transform.localScale;
		a.transform.localPosition = mPrefab.transform.localPosition;
		a.transform.rotation = mPrefab.transform.rotation;

		a = a.transform.Find ("Texts").gameObject;

		mTitleText = a.transform.Find ("Name_Text").GetComponent<TextMesh> ();
		mDescriptionText = a.transform.Find ("Description_Text").GetComponent<TextMesh> ();
		mCurrentText = a.transform.Find ("Current_description").GetComponent<TextMesh> ();
		mNextText = a.transform.Find ("Next_Description").GetComponent<TextMesh> ();
		//mCostBoltsText = a.transform.Find ("buy text 1").GetComponent<TextMesh> ();
		//mCostCrystalsText = a.transform.Find ("buy text 2").GetComponent<TextMesh> ();

		mOpen = false;
	}

	// Use this for initialization
	void Start()
	{
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public void Open(GameObject prefab)
	{
		mObjToBuy = GameObject.Instantiate(prefab);

		Transform[] obs = mObjToBuy.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < obs.Length; i++) 
		{
			obs[i].gameObject.SetActive(true);
		}

		mObjToBuy.transform.parent = transform;
		mObjToBuy.transform.localPosition = new Vector3(0,0,0);

		mOpen = true;
		gameObject.SetActive(true);

		GUICanvas.Instance.ShowPopupBuyButton (true);
	}

	public void Close()
	{
		Destroy (mObjToBuy);
		mOpen = false;
		gameObject.SetActive(false);
		GUICanvas.Instance.ShowPopupBuyButton (false);
	}

	public bool IsOpen ()
	{
		return mOpen;
	}

	public void updateData (string description, string current, string next, int costBolts, int nextCrystals)
	{
		mDescriptionText.text = description + "\n" + costBolts + " Bolts";
		mCurrentText.text = "CURRENT: " + current;
		mNextText.text = "NEXT: " + next;
		//mCostBoltsText.text = costBolts + " B";
		//mCostCrystalsText.text = nextCrystals + " C";
	}
}
