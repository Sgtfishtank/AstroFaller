﻿using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
	TextMesh[] mTexts;
	Player mPlayer;
	public int boxes;
	public GameObject[] mBoxObj;
	public GameObject puff;
	public float mCalcT;
	public float mCalcDuration = 2f;
	
	private FMOD.Studio.EventInstance mDisDown;
	private FMOD.Studio.EventInstance mCoinUp;
	private bool runSound; 

	void Awake()
	{
		mDisDown = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/RewardTickerBolts/TickerBolts");
		mCoinUp = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/RewardTickerDistance/TickerDistance");
		mTexts = gameObject.GetComponentsInChildren<TextMesh> ();
	}

	// Use this for initialization
	void Start()
	{

	}

	void OnEnable ()
	{
		mCalcT = Time.time + mCalcDuration;
		mPlayer = InGame.Instance.mPlayer;
		boxes = mPlayer.CollectedPerfectDistances();
		setBoxes();
	}
	
	void OnDisable()
	{
		if (runSound)
		{
			AudioManager.Instance.StopSound(mCoinUp, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.Instance.StopSound(mDisDown, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			runSound = false;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			mCalcT = Time.time - mCalcDuration + 0.8f;
		}


		float deltaT = 1f - ((mCalcT + 0.8f - Time.time) / mCalcDuration);
		deltaT = Mathf.Clamp01(deltaT);

		if ((!runSound) && (deltaT > 0))
		{
			AudioManager.Instance.PlaySound(mCoinUp);
			AudioManager.Instance.PlaySound(mDisDown);
			runSound = true;
		}
		else if (runSound && (deltaT >= 1))
		{
			AudioManager.Instance.StopSound(mCoinUp, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.Instance.StopSound(mDisDown, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			runSound = true;
		}

		float multi = (int)(Mathf.Lerp(1, calculateMultiplier(), deltaT) * 100f);
		int dis = (int)Mathf.Lerp(mPlayer.distance(), 0, deltaT);
		int totl = (int)Mathf.Lerp(mPlayer.colectedBolts(), calculateMultiplier() * mPlayer.colectedBolts(), deltaT);

		for (int i = 0; i < mTexts.Length; i++)
		{
			if (mTexts[i].gameObject.name == "distance total")
				mTexts[i].text = dis.ToString();
			else if (mTexts[i].gameObject.name == "bolts gathered")
				mTexts[i].text = mPlayer.colectedBolts ().ToString();
			else if (mTexts[i].gameObject.name == "multiplier number")
				mTexts[i].text = multi.ToString() + "";
			else if (mTexts[i].gameObject.name == "bolts total")
				mTexts[i].text = totl.ToString();
		}
	}

	float calculateMultiplier()
	{
		return 1 + (mPlayer.distance() / 5000f);
	}

	public void setBoxes()
	{
		for(int i = 0; i < mBoxObj.Length; i++)
		{
			mBoxObj[i].SetActive(true);
		}
		for(int i = mPlayer.CollectedPerfectDistances(); i < mBoxObj.Length; i++)
		{
			mBoxObj[i].SetActive(false);
		}
		if(mPlayer.CollectedPerfectDistances() >4)
		{
			for(int i = 1; i < mBoxObj.Length; i++)
			{
				mBoxObj[i].SetActive(false);
			}
		}
		switch (mPlayer.CollectedPerfectDistances())
		{
		case 0:
			break;
		case 1:

			mBoxObj[0].transform.localPosition = new Vector3(0,-2.56f,0);
			break;
		case 2:
			mBoxObj[0].transform.localPosition = new Vector3(0.77f,-2.56f,0);
			mBoxObj[1].transform.localPosition = new Vector3(-0.77f,-2.56f,0);
			break;
		case 3:
			mBoxObj[0].transform.localPosition = new Vector3(1.14f,-2.56f,0);
			mBoxObj[1].transform.localPosition = new Vector3(0,-2.56f,0);
			mBoxObj[2].transform.localPosition = new Vector3(-1.14f,-2.56f,0);
			break;
		case 4:
			mBoxObj[0].transform.localPosition = new Vector3(1.35f,-2.56f,0);
			mBoxObj[1].transform.localPosition = new Vector3(0.48f,-2.56f,0);
			mBoxObj[2].transform.localPosition = new Vector3(-0.48f,-2.56f,0f);
			mBoxObj[3].transform.localPosition = new Vector3(-1.35f,-2.56f,0);
			break;
		default:
			mBoxObj[0].transform.localPosition = new Vector3(0.36f,-2.56f,0);
			break;
		}
	}
	public void removeBox(int id)
	{
		Instantiate(puff,new Vector3(mBoxObj[id-1].transform.position.x,mBoxObj[id-1].transform.position.y,-6),Quaternion.identity);
		mBoxObj[id-1].SetActive(false);

	}
}
