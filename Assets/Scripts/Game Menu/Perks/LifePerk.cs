using UnityEngine;
using System.Collections;

public class LifePerk : Perk 
{
	public string mPerkName;
	public GameObject mPrefab;
	
	private GameObject mObj;
	private bool mMainUnlocked;
	private bool mLeftUnlocked;
	private bool mRightUnlocked;
	private	TextMesh mTitleText;
	private	Animator mAnimator;
	private	GameObject m1p;
	private	GameObject m2p;
	private	GameObject m3p;
	public GameObject[] mObjParts;
	
	void Awake ()
	{
		mObj = GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		
		mTitleText = mObj.transform.Find ("life_text").GetComponent<TextMesh> ();
		
		mAnimator = mObj.transform.Find ("Anim_LifePerk").GetComponent<Animator> ();
		
		m1p = mAnimator.transform.Find("box").gameObject;
		m2p = mAnimator.transform.Find("arm").gameObject;
		m3p = mAnimator.transform.Find("shelf").gameObject;
		m1p.SetActive (false);
		m2p.SetActive (false);
		m3p.SetActive (false);
		
		mObjParts = new GameObject[3];
		for (int i = 0; i < mObjParts.Length; i++) 
		{
			mObjParts[i] = mObj.transform.Find("upgrade_life/buy_orb " + (i + 1)).gameObject;
			mObjParts[i].SetActive(false);
		}

		if (mPerkName.Length < 1)
		{
			mPerkName = gameObject.name;
		}
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		mTitleText.text = mPerkName;
	}
	
	public override bool UnlockPart()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			if (!mMainUnlocked)
			{
				mMainUnlocked = true;
				PlayerData.Instance.mAirPerkUnlockedLevel = 1;
				//mAnimator.SetTrigger("Upgrade");
				mAnimator.Play(PlayerData.Instance.mAirPerkUnlockedLevel.ToString());
				mObjParts[0].SetActive(true);
				m1p.SetActive(true);
				return true;
			}
			break;
		case PerkPart.Left:
			if (mMainUnlocked && (!mLeftUnlocked))
			{
				mLeftUnlocked = true;
				PlayerData.Instance.mAirPerkUnlockedLevel = 2;
				//mAnimator.SetTrigger("Upgrade");
				mAnimator.Play(PlayerData.Instance.mAirPerkUnlockedLevel.ToString());
				mObjParts[1].SetActive(true);
				m2p.SetActive(true);
				return true;
			}
			break;
		case PerkPart.Right:
			if (mMainUnlocked && mLeftUnlocked && (!mRightUnlocked))
			{
				mRightUnlocked = true;
				PlayerData.Instance.mAirPerkUnlockedLevel = 3;
				//mAnimator.SetTrigger("Upgrade");
				mAnimator.Play(PlayerData.Instance.mAirPerkUnlockedLevel.ToString());
				mObjParts[2].SetActive(true);
				m3p.SetActive(true);
				return true;
			}
			break;
		default:
			print("Error part in UnlockPart: " + perkPart);
			break;
		}
		
		return false;
	}

	public override bool IsPartUnlocked()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return mMainUnlocked;
		case PerkPart.Left:
			return mMainUnlocked && mLeftUnlocked;
		case PerkPart.Right:
			return mMainUnlocked && mRightUnlocked;
		default:
			print("Error part in IsPartUnlocked: " + perkPart);
			break;
		}
		
		return false;
	}
	
	public override bool CanUnlockPart()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return (!mMainUnlocked);
		case PerkPart.Left:
			return mMainUnlocked && (!mLeftUnlocked);
		case PerkPart.Right:
			return mMainUnlocked && (!mRightUnlocked);
		default:
			print("Error part in CanUnlockPart: " + perkPart);
			break;
		}
		
		return false;
	}
	
	public override int BuyCostBolts()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.LIFE_PERK_MAIN_COST_BOLTS;
		case PerkPart.Left:
			return GlobalVariables.Instance.LIFE_PERK_LEFT_COST_BOLTS;
		case PerkPart.Right:
			return GlobalVariables.Instance.LIFE_PERK_RIGHT_COST_BOLTS;
		default:
			print ("Error perkPart in BuyCost " + perkPart);
			break;
		}
		
		return -1;
	}

	public override int BuyCostCrystals()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.LIFE_PERK_MAIN_COST_CRYSTALS;
		case PerkPart.Left:
			return GlobalVariables.Instance.LIFE_PERK_LEFT_COST_CRYSTALS;
		case PerkPart.Right:
			return GlobalVariables.Instance.LIFE_PERK_RIGHT_COST_CRYSTALS;
		default:
			print ("Error perkPart in BuyCost " + perkPart);
			break;
		}

		return -1;
	}
	
	public override string BuyDescription()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.LIFE_PERK_MAIN_DESCRIPTION;
		case PerkPart.Left:
			return GlobalVariables.Instance.LIFE_PERK_LEFT_DESCRIPTION;
		case PerkPart.Right:
			return GlobalVariables.Instance.LIFE_PERK_RIGHT_DESCRIPTION;
		default:
			print ("Error perkPart in BuyDescription " + perkPart);
			break;
		}
		
		return "---";
	}
	
	public PerkPart GetNextPerkPart()
	{
		if (!mMainUnlocked) 
		{
			return PerkPart.Main;
		}
		else if( mMainUnlocked && (!mLeftUnlocked))
		{
			return PerkPart.Left;
		}
		else if (mMainUnlocked && mLeftUnlocked && (!mRightUnlocked))
		{
			return PerkPart.Right;
		}
		
		return PerkPart.Main;
	}

	public override string BuyCurrent()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.LIFE_PERK_MAIN_LEVELS[0] + GlobalVariables.Instance.LIFE_PERK_MAIN_LEVELS_UNIT;
		case PerkPart.Left:
			return GlobalVariables.Instance.LIFE_PERK_LEFT_LEVELS[0] + GlobalVariables.Instance.LIFE_PERK_LEFT_LEVELS_UNIT;
		case PerkPart.Right:
			return GlobalVariables.Instance.LIFE_PERK_RIGHT_LEVELS[0] + GlobalVariables.Instance.LIFE_PERK_RIGHT_LEVELS_UNIT;
		default:
			print ("Error perkPart in BuyCurrent " + perkPart);
			break;
		}
		
		return "---";
	}
	
	public override string BuyNext()
	{
		PerkPart perkPart = GetNextPerkPart();
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.LIFE_PERK_MAIN_LEVELS[1] + GlobalVariables.Instance.LIFE_PERK_MAIN_LEVELS_UNIT;
		case PerkPart.Left:
			return GlobalVariables.Instance.LIFE_PERK_LEFT_LEVELS[1] + GlobalVariables.Instance.LIFE_PERK_LEFT_LEVELS_UNIT;
		case PerkPart.Right:
			return GlobalVariables.Instance.LIFE_PERK_RIGHT_LEVELS[1] + GlobalVariables.Instance.LIFE_PERK_RIGHT_LEVELS_UNIT;
		default:
			print ("Error perkPart in BuyNext " + perkPart);
			break;
		}
		
		return "---";
	}
	
	public static void UpdatePerkValueAnimation(Animator currPerk)
	{
		if ((currPerk == null) || (currPerk.transform.parent == null))
		{
			return;
		}
		
		if (LifePerk.mCrkTrxx[0] > 0)
		{
			float phase = (Time.time - LifePerk.mCrkTrxx[0]) / 0.1f;
			if (((int)phase + 1) >= LifePerk.mCrkTrxx.Length)
			{
				LifePerk.mCrkTrxx[0] = -1;
				currPerk.transform.position = currPerk.transform.parent.position;
				return;
			}
			currPerk.transform.position = new Vector3(Mathf.Lerp(LifePerk.mCrkTrxx[(int)phase + 1] * 0.5f, LifePerk.mCrkTrxx[(((int)phase + 1) % (LifePerk.mCrkTrxx.Length - 1)) + 1] * 0.5f, (phase - (int)phase)), currPerk.transform.parent.position.y, currPerk.transform.parent.position.z);
		}
		else if (Input.GetKeyDown(KeyCode.X))
		{
			LifePerk.mCrkTrxx[0] = Time.time;
		}
	}

	// crack traxxxxxxxxxxx
	private static float[] mCrkTrxx = new float[]{-1f, 0f, 
		-16.55498f, 
		2.987686f, 
		-1.33795f, 
		-11.90307f, 
		-5.102466f, 
		6.434298f, 
		-0.4092716f, 
		5.075924f, 
		12.92926f, 
		-2.183581f, 
		13.48824f, 
		10.2534f, 
		-4.408308f, 
		8.820399f, 
		-7.15671f, 
		-0.8441398f, 
		-12.04759f, 
		-2.427303f, 
		-4.496984f, 
		-3.296658f, 
		9.931433f, 
		-8.038988f, 
		-2.442341f, 
		5.371328f, 
		-4.369855f, 
		4.982644f, 
		-8.181417f, 
		-10.63474f, 
		-10.08368f, 
		1.175876f, 
		-9.483768f, 
		-10.07234f, 
		-9.046327f, 
		6.252417f, 
		7.874692f, 
		7.739073f, 
		9.155975f, 
		12.74861f, 
		11.35024f, 
		13.60696f, 
		10.41316f, 
		11.66177f, 
		3.332133f, 
		-10.32205f, 
		-14.18721f, 
		-2.574914f, 
		-6.075199f, 
		6.033282f, 
		-4.612247f, 
		3.930206f, 
		-9.987414f, 
		-10.0073f, 
		-14.93391f, 
		14.34313f, 
		14.35369f, 
		8.843215f, 
		-0.8887575f, 
		-1.796101f, 
		-7.773324f, 
		-14.27127f, 
		-0.2497224f, 
		-14.76353f, 
		2.108237f, 
		-10.67515f, 
		0.2957506f, 
		8.895916f, 
		9.263322f, 
		10.19396f, 
		10.29634f, 
		12.12667f, 
		10.94956f, 
		-2.481542f, 
		-20.61042f, 
		-18.96432f, 
		-15.46589f, 
		-17.58788f, 
		-16.32197f, 
		15.65633f, 
		7.418498f, 
		7.528269f, 
		-12.99468f, 
		-2.729963f, 
		-0.806717f, 
		-5.007302f, 
		8.47156f, 
		-8.379662f, 
		-17.66834f, 
		-19.32645f, 
		-1.285802f, 
		-8.430658f, 
		4.692305f, 
		10.17042f, 
		12.41629f, 
		-4.140206f, 
		-11.27337f, 
		-17.89552f, 
		-5.869627f, 
		8.015057f, 
		10.84724f, 
		9.379851f, 
		-13.35014f, 
		-16.19646f, 
		-10.07743f, 
		-8.738546f, 
		5.601331f, 
		-3.338188f, 
		6.223926f, 
		-13.73552f, 
		-19.05461f, 
		-20.81397f, 
		-5.238799f, 
		3.461226f, 
		17.67637f, 
		10.26376f, 
		1.316778f, 
		-8.157659f, 
		-7.917262f, 
		-18.18575f, 
		-3.757711f, 
		-11.36719f, 
		6.970145f, 
		2.080391f, 
		4.596775f, 
		5.441692f, 
		11.43944f, 
		10.26868f, 
		6.317919f, 
		12.05046f, 
		9.342562f, 
		2.849952f, 
		-9.267926f, 
		-13.13691f, 
		4.415356f, 
		-5.613404f, 
		-6.202354f, 
		-13.60093f, 
		-11.60677f, 
		3.315025f, 
		0.3628771f, 
		9.311277f, 
		10.83346f, 
		11.50845f, 
		-1.836023f, 
		-15.23904f, 
		-17.72567f, 
		-7.348854f, 
		-18.57339f, 
		2.057387f, 
		2.41471f, 
		15.16716f, 
		2.917899f, 
		-10.31272f, 
		-16.73335f, 
		-16.41105f, 
		-8.483263f, 
		-1.301847f, 
		5.918805f, 
		-2.18731f, 
		-0.752563f, 
		-10.32251f, 
		-10.26883f, 
		-12.12485f, 
		-7.051932f, 
		-0.4851706f, 
		10.9775f, 
		11.27675f, 
		11.27759f, 
		11.54358f, 
		11.62109f, 
		11.21848f, 
		10.9181f, 
		10.8179f, 
		10.98816f, 
		10.37389f, 
		0.5614235f
	};
}
