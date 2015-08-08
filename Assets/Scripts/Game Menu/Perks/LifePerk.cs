using UnityEngine;
using System.Collections;

public class LifePerk : Perk 
{
	public string mPerkName;
	
	private bool mMainUnlocked;
	private bool mLeftUnlocked;
	private bool mRightUnlocked;
	private	TextMesh mTitleText;
	private	GameObject mRight3;
	private	GameObject mLeft4;
	private	GameObject mMain5;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	public override void Init()
	{
		if (mPerkName.Length < 1)
		{
			mPerkName = gameObject.name;
		}
		
		mTitleText = transform.Find ("Life/Life+ text").GetComponent<TextMesh> ();
		mRight3 = transform.Find ("Life/perks_air 3").gameObject;
		mLeft4 = transform.Find ("Life/perks_air 4").gameObject;
		mMain5 = transform.Find ("Life/perks_air 5").gameObject;
		
		mRight3.SetActive (false);
		mLeft4.SetActive (false);
		mMain5.SetActive (false);
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
				mMain5.SetActive(true);
				return true;
			}
			break;
		case PerkPart.Left:
			if (mMainUnlocked && (!mLeftUnlocked))
			{
				mLeftUnlocked = true;
				mLeft4.SetActive(true);
				return true;
			}
			break;
		case PerkPart.Right:
			if (mMainUnlocked && (!mRightUnlocked))
			{
				mRightUnlocked = true;
				mRight3.SetActive(true);
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

	public override int BuyCostCrystals(PerkPart perkPart)
	{
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
	
	public override string BuyDescription(PerkPart perkPart)
	{
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
	
	public override string BuyCurrent(PerkPart perkPart)
	{
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
	
	public override string BuyNext(PerkPart perkPart)
	{
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
}
