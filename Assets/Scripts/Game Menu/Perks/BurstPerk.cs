using UnityEngine;
using System.Collections;

public class BurstPerk : Perk 
{
	public string mPerkName;
	public GameObject mPrefab;
	
	private GameObject mObj;
	private int mUnlockedLevel;
	private	TextMesh mTitleText;
	private	Animator mAnimator;
	private	GameObject m1p;
	private	GameObject m2p;
	private	GameObject m3p;
    private GameObject[] mObjParts;
    private ButtonManager mPerkButton;

	void Awake ()
	{
		mObj = GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		
		mTitleText = mObj.transform.Find ("burst_text").GetComponent<TextMesh> ();
		
		mAnimator = mObj.transform.Find ("Anim_BurstPerk").GetComponent<Animator> ();
		
		m1p = mAnimator.transform.Find("middlerocket").gameObject;
		m2p = mAnimator.transform.Find("middlerocket/group2").gameObject;
		m3p = mAnimator.transform.Find("perk_burst_3").gameObject;
		//m1p.SetActive (false);
		m2p.SetActive (false);
		m3p.SetActive (false);
		
		mObjParts = new GameObject[3];
		for (int i = 0; i < mObjParts.Length; i++) 
		{
			mObjParts[i] = mObj.transform.Find("upgrade_burst/buy_orb " + (i + 1)).gameObject;
			mObjParts[i].SetActive(false);
		}

		if (mPerkName.Length < 1)
		{
			mPerkName = gameObject.name;
        }
        mPerkButton = ButtonManager.CreateButton(gameObject, "perk_burst/Anim_BurstPerk");
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
			m3p.SetActive(true);
			break;
		default:
			print("Error part in UnlockPart: " + mUnlockedLevel);
			return false;
		}
		
		mObjParts[mUnlockedLevel].SetActive(true);
		mUnlockedLevel++;
		PlayerData.Instance.mBurstPerkUnlockedLevel = mUnlockedLevel;
		mAnimator.Play(PlayerData.Instance.mBurstPerkUnlockedLevel.ToString());
		return true;
	}

	void OnDisable()
	{
		mAnimator.StopPlayback();
	}
	
	public override bool CanUnlockPart()
	{
		return (mUnlockedLevel < GlobalVariables.Instance.PERKS_MAX_LEVEL);
	}

	public override int BuyCostBolts()
	{
		return GlobalVariables.Instance.BURST_PERK_COST_BOLTS[mUnlockedLevel];
	}
	
	public override int BuyCostCrystals()
	{
		return GlobalVariables.Instance.BURST_PERK_COST_CRYSTALS[mUnlockedLevel];
	}

	public override string BuyDescription()
	{
		return GlobalVariables.Instance.BURST_PERK_DESCRIPTION[mUnlockedLevel];
	}
	
	public override string BuyCurrent()
	{
		return "---";
	}
	
	public override string BuyNext()
	{
		return "---";
	}
}
