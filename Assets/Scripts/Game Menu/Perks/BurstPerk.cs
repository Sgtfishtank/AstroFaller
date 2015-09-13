using UnityEngine;
using System.Collections;

public class BurstPerk : Perk 
{
	public string mPerkName;
	public GameObject mPrefab;
	
	private bool mMainUnlocked;
	private bool mLeftUnlocked;
	private bool mRightUnlocked;
	private	TextMesh mTitleText;
	//private	GameObject mRight3;
	//private	GameObject mLeft4;
	//private	GameObject mMain5;
	private	Animator mAnimator;
	private	GameObject m1p;
	private	GameObject m2p;
	private	GameObject m3p;
	
	void Awake ()
	{
		GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		
		mTitleText = transform.Find ("perk_burst/burst_text").GetComponent<TextMesh> ();
		
		mAnimator = transform.Find ("perk_burst/Anim_BurstPerk").GetComponent<Animator> ();
		
		m1p = mAnimator.transform.Find("middlerocket").gameObject;
		m2p = mAnimator.transform.Find("middlerocket/group2").gameObject;
		m3p = mAnimator.transform.Find("perk_burst_3").gameObject;
		m1p.SetActive (false);
		m2p.SetActive (false);
		m3p.SetActive (false);
		
		if (mPerkName.Length < 1)
		{
			mPerkName = gameObject.name;
		}
		
		//mRight3.SetActive (false);
		//mLeft4.SetActive (false);
		//mMain5.SetActive (false);
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
	
	public override bool UnlockPart(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			if (!mMainUnlocked)
			{
				mMainUnlocked = true;
				PlayerData.Instance.mAirPerkUnlockedLevel = 1;
				mAnimator.SetTrigger("Upgrade");
				m1p.SetActive(true);
				return true;
			}
			break;
		case PerkPart.Left:
			if (mMainUnlocked && (!mLeftUnlocked))
			{
				mLeftUnlocked = true;
				PlayerData.Instance.mAirPerkUnlockedLevel = 2;
				mAnimator.SetTrigger("Upgrade");
				m2p.SetActive(true);
				return true;
			}
			break;
		case PerkPart.Right:
			if (mMainUnlocked && mLeftUnlocked && (!mRightUnlocked))
			{
				mRightUnlocked = true;
				PlayerData.Instance.mAirPerkUnlockedLevel = 3;
				mAnimator.SetTrigger("Upgrade");
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

	public override bool IsPartUnlocked(PerkPart perkPart)
	{
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
	
	public override bool CanUnlockPart(Perk.PerkPart perkPart)
	{
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
	
	public override int BuyCostBolts(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.BURST_PERK_MAIN_COST_BOLTS;
		case PerkPart.Left:
			return GlobalVariables.Instance.BURST_PERK_LEFT_COST_BOLTS;
		case PerkPart.Right:
			return GlobalVariables.Instance.BURST_PERK_RIGHT_COST_BOLTS;
		default:
			print ("Error perkPart in BuyCost " + perkPart);
			break;
		}
	
		return -1;
	}
	
	public override int BuyCostCrystals(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.BURST_PERK_MAIN_COST_CRYSTALS;
		case PerkPart.Left:
			return GlobalVariables.Instance.BURST_PERK_LEFT_COST_CRYSTALS;
		case PerkPart.Right:
			return GlobalVariables.Instance.BURST_PERK_RIGHT_COST_CRYSTALS;
		default:
			print ("Error perkPart in BuyCost " + perkPart);
			break;
		}

		return -1;
	}

	public override string BuyDescription(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.BURST_PERK_MAIN_DESCRIPTION;
		case PerkPart.Left:
			return GlobalVariables.Instance.BURST_PERK_LEFT_DESCRIPTION;
		case PerkPart.Right:
			return GlobalVariables.Instance.BURST_PERK_RIGHT_DESCRIPTION;
		default:
			print ("Error perkPart in BuyDescription " + perkPart);
			break;
		}
		
		return "---";
	}
	
	public override string BuyCurrent(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.BURST_PERK_MAIN_LEVELS[0] + GlobalVariables.Instance.BURST_PERK_MAIN_LEVELS_UNIT;
		case PerkPart.Left:
			return GlobalVariables.Instance.BURST_PERK_LEFT_LEVELS[0] + GlobalVariables.Instance.BURST_PERK_LEFT_LEVELS_UNIT;
		case PerkPart.Right:
			return GlobalVariables.Instance.BURST_PERK_RIGHT_LEVELS[0] + GlobalVariables.Instance.BURST_PERK_RIGHT_LEVELS_UNIT;
		default:
			print ("Error perkPart in BuyCurrent " + perkPart);
			break;
		}
		
		return "---";
	}
	
	public override string BuyNext(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			return GlobalVariables.Instance.BURST_PERK_MAIN_LEVELS[1] + GlobalVariables.Instance.BURST_PERK_MAIN_LEVELS_UNIT;
		case PerkPart.Left:
			return GlobalVariables.Instance.BURST_PERK_LEFT_LEVELS[1] + GlobalVariables.Instance.BURST_PERK_LEFT_LEVELS_UNIT;
		case PerkPart.Right:
			return GlobalVariables.Instance.BURST_PERK_RIGHT_LEVELS[1] + GlobalVariables.Instance.BURST_PERK_RIGHT_LEVELS_UNIT;
		default:
			print ("Error perkPart in BuyNext " + perkPart);
			break;
		}
		
		return "---";
	}
}
