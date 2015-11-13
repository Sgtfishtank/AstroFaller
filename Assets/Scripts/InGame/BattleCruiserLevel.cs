﻿using UnityEngine;
using System.Collections;

public class BattleCruiserLevel : MonoBehaviour 
{
	public float mSpeed;
	public float mWrapValue;
	public GameObject[] mParts;

	void Awake()
	{
		mParts = new GameObject[3];
		
		for (int i = 0; i < mParts.Length; i++) 
		{
			mParts[i] = transform.GetChild(i).gameObject;
		}
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < mParts.Length; i++) 
		{
			mParts[i].transform.position += new Vector3(mSpeed, 0, 0);

			if (mParts[i].transform.position.x >= mWrapValue) 
			{
				mParts[i].transform.position -= new Vector3(mWrapValue * 2, 0, 0);
			}
		}
	}
}