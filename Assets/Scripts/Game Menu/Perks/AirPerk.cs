using UnityEngine;
using System.Collections;

public class AirPerk : Perk 
{
	public string mPerkName;
	public GameObject mPrefab;
	
	private GameObject mObj;
	private int mUnlockedLevel;
	private	TextMesh mTitleText;
	private	Animator mAnimator;
	private	GameObject m1p;
	private	GameObject m2p;
	private	GameObject[] m3p = new GameObject[3];
    private GameObject[] mObjParts;
    private ButtonManager mPerkButton;

	void Awake ()
	{
		mObj = GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);

		mTitleText = mObj.transform.Find ("air_text").GetComponent<TextMesh> ();
		mAnimator = mObj.transform.Find ("Anim_AirPerk").GetComponent<Animator> ();
		
		m1p = mAnimator.transform.Find("perk_air_1").gameObject;
		m2p = mAnimator.transform.Find("perk_airBS2").gameObject;
		m3p[0] = mAnimator.transform.Find("polySurface16").gameObject;
		m3p[1] = mAnimator.transform.Find("polySurface18").gameObject;
		m3p[2] = mAnimator.transform.Find("polySurface19").gameObject;
		//m1p.SetActive(false);
		m2p.SetActive(false);
		m3p[0].SetActive(false);
		m3p[1].SetActive(false);
		m3p[2].SetActive(false);

		mObjParts = new GameObject[3];
		for (int i = 0; i < mObjParts.Length; i++) 
		{
			mObjParts[i] = mObj.transform.Find("upgrade_air/buy_orb " + (i + 1)).gameObject;
			mObjParts[i].SetActive(false);
		}

		if (mPerkName.Length < 1)
		{
			mPerkName = gameObject.name;
        }

        mPerkButton = ButtonManager.CreateButton(gameObject, "perk_air/Anim_AirPerk");
	}

	public override bool CanUnlockPart ()
	{
		return (mUnlockedLevel < GlobalVariables.Instance.PERKS_MAX_LEVEL);
	}

	public override GameObject PreviewObject ()
	{
		return mAnimator.gameObject;
	}

	// Use this for initialization
	void Start ()
    {
        GUICanvasBase gui = MenuGUICanvas.Instance.PerksGUI();
        mPerkButton.LoadButtonPress("LifePerk/Button 4", gui);
	}

	// Update is called once per frame
	void Update () 
	{
		mTitleText.text = mPerkName;
	}
	
	public override bool UnlockPart()
	{
		switch (mUnlockedLevel) 
		{
		case 0:
			m1p.SetActive(true);
			break;
		case 1:
			m2p.SetActive(true);
			break;
		case 2:
			m3p[0].SetActive(true);
			m3p[1].SetActive(true);
			m3p[2].SetActive(true);
			break;
		default:
			print("Error part in UnlockPart: " + mUnlockedLevel);
			return false;
		}
		
		mObjParts[mUnlockedLevel].SetActive(true);
		mUnlockedLevel++;
		PlayerData.Instance.mAirPerkUnlockedLevel = mUnlockedLevel;
		mAnimator.Play(PlayerData.Instance.mAirPerkUnlockedLevel.ToString());
		return true;
	}

	public override int BuyCostBolts()
	{
		return GlobalVariables.Instance.AIR_PERK_COST_BOLTS[mUnlockedLevel];
	}
	
	public override int BuyCostCrystals()
	{
		return GlobalVariables.Instance.AIR_PERK_COST_CRYSTALS[mUnlockedLevel];
	}

	public override string BuyDescription()
	{
		return GlobalVariables.Instance.AIR_PERK_DESCRIPTION[mUnlockedLevel];
	}
	
	public override string BuyCurrent()
	{
		return "---";
		//return GlobalVariables.Instance.AIR_PERK_LEVELS[mUnlockedLevel] + GlobalVariables.Instance.AIR_PERK_MAIN_LEVELS_UNIT;
	}

	public override string BuyNext()
	{
		if ((mUnlockedLevel + 1) >= GlobalVariables.Instance.PERKS_MAX_LEVEL) 
		{
			return "---";
		}
		
		return "---";
		//return GlobalVariables.Instance.AIR_PERK_LEVELS[mUnlockedLevel + 1] + GlobalVariables.Instance.AIR_PERK_MAIN_LEVELS_UNIT;
	}
}
