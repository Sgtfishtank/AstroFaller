using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
	//Player mPlayer;
	//public int boxes;
	public GameObject[] mBoxObj;
	public GameObject puff;
	public float mCalcT;
	public float mCalcDuration = 2f;

	private TextMesh[] mTexts;
	private TextMesh mMultiText;
	private TextMesh mDisText;
	private TextMesh mTotalBoltsText;
	private TextMesh mBoltsText;

    private int mMultiT;
	private int mDisT;
	private int mTotalBoltsT;
	private int mBoltsT;

	private AudioInstanceData mDisDown;
	private AudioInstanceData mCoinUp;
	private AudioInstanceData fmodDeathMusic;
	private bool mRunSound; 

	private int mDistance;
    private int mBoxes;
    private int mBolts;

	private ButtonManager[] mRestatButton = new ButtonManager[3];
    private ButtonManager[] mMenuButton = new ButtonManager[3];
    private bool mOpen;

	void Awake()
	{
        mRestatButton[0] = ButtonManager.CreateButton(gameObject, "button_1_base");
        mMenuButton[0] = ButtonManager.CreateButton(gameObject, "button_2_base");
        mRestatButton[1] = ButtonManager.CreateButton(gameObject, "button_1_core");
        mMenuButton[1] = ButtonManager.CreateButton(gameObject, "button_2_core");
        mRestatButton[2] = ButtonManager.CreateButton(gameObject, "Text/restart");
        mMenuButton[2] = ButtonManager.CreateButton(gameObject, "Text/main menu");

		mTexts = gameObject.GetComponentsInChildren<TextMesh> ();
		
		for (int i = 0; i < mTexts.Length; i++)
		{
			if (mTexts[i].gameObject.name == "bolts gathered")
				mBoltsText = mTexts[i];
			if (mTexts[i].gameObject.name == "distance total")
				mDisText = mTexts[i];
			else if (mTexts[i].gameObject.name == "multiplier number")
				mMultiText = mTexts[i];
			else if (mTexts[i].gameObject.name == "bolts total")
				mTotalBoltsText = mTexts[i];
		}

        fmodDeathMusic = AudioManager.Instance.GetMusicEvent("ScrapScoreMusic/ScrapScoreMusic", false);
        mDisDown = AudioManager.Instance.GetSoundsEvent("ScrapScoreTicker/RewardTickerBoTH", false);
        //mCoinUp = AudioManager.Instance.GetSoundsEvent("RewardTickerDistance/TickerDistance");
	}

	// Use this for initialization
	void Start()
    {
        DeatMenuGUI gui = InGameGUICanvas.Instance.DeathMenuGUI();
        mRestatButton[0].LoadButtonPress("Restart", gui);
        mMenuButton[0].LoadButtonPress("MainMenu", gui);
        mRestatButton[1].LoadButtonPress("Restart", gui);
        mMenuButton[1].LoadButtonPress("MainMenu", gui);
        mRestatButton[2].LoadButtonPress("Restart", gui);
        mMenuButton[2].LoadButtonPress("MainMenu", gui);
	}

	void OnEnable ()
	{
        mRunSound = false;
		mCalcT = Time.time + mCalcDuration;
		setBoxes();

        // reset text values
        mMultiT = -1;
        mDisT = -1;
        mTotalBoltsT = -1;
        mBoltsT = -1;
		UpdateDistanceText(0);
		UpdateMultiplierText(0);
		UpdateTotalBoltsText(0);
		UpdateBoltsText(0);
	}

    void OnDisable()
    {
    }

	public void Open(int distance, int bolts, int boxes, Vector3 playerPos)
    {
        if (mOpen)
        {
            Close();
        }

        Vector3 a = playerPos;
        a.x = 0;
        a.y = InGameCamera.Instance.transform.position.y + 3.5f;
        a.z = transform.position.z;
        transform.position = a;

        mDistance = distance;
        mBoxes = distance;
        mBolts = bolts;

        gameObject.SetActive(true);
		setBoxes();
		AudioManager.Instance.PlayMusic(fmodDeathMusic);
        InGameGUICanvas.Instance.DeathMenuGUI().setEnableDeathMenu(true);
        mOpen = true;

        UpdateBoltsText(mBolts);
	}
	
	public void Close()
	{
        if (!mOpen)
        {
            return;
        }

		AudioManager.Instance.StopMusic(fmodDeathMusic);
		AudioManager.Instance.StopSound(mCoinUp);
		AudioManager.Instance.StopSound(mDisDown);
        InGameGUICanvas.Instance.DeathMenuGUI().setEnableDeathMenu(false);
        gameObject.SetActive(false);
        mOpen = false;
	}

	public void Skip ()
	{
		mCalcT = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		float deltaT = 1f - ((mCalcT + 0.8f - Time.time) / mCalcDuration);
		deltaT = Mathf.Clamp01(deltaT);

		if ((!mRunSound) && (deltaT > 0) && (deltaT < 1))
		{
			AudioManager.Instance.PlaySound(mCoinUp);
			AudioManager.Instance.PlaySound(mDisDown);
			mRunSound = true;
		}
		else if (mRunSound && (deltaT >= 1))
		{
			AudioManager.Instance.StopSound(mCoinUp);
			AudioManager.Instance.StopSound(mDisDown);
			mRunSound = false;
		}

        int multi = (int)(Mathf.Lerp(1, PlayerData.Instance.CalculateMultiplier(mDistance), deltaT) * 100f);
		int dis = (int)Mathf.Lerp(mDistance, 0, deltaT);
		int totalBolts = (int)Mathf.Lerp(mBolts, PlayerData.Instance.CalculateMultiplier(mDistance) * mBolts, deltaT);

		UpdateDistanceText(dis);
		UpdateMultiplierText(multi);
		UpdateTotalBoltsText(totalBolts);
	}

	void UpdateTotalBoltsText(int totalBolts)
	{
		// avoid string allocations
		if (mTotalBoltsT != totalBolts) 
		{
			mTotalBoltsT = totalBolts;
			mTotalBoltsText.text = mTotalBoltsT.ToString();
		}
	}

	void UpdateMultiplierText(int multi)
	{
		// avoid string allocations
		if (mMultiT != multi) 
		{
			mMultiT = multi;
			mMultiText.text = mMultiT.ToString();
		}
	}

	void UpdateDistanceText(int dis)
	{
		// avoid string allocations
		if (mDisT != dis) 
		{
			mDisT = dis;
			mDisText.text = mDisT.ToString();
		}
	}
	
	void UpdateBoltsText(int bolts)
	{
		// avoid string allocations
		if (mBoltsT != bolts) 
		{
			mBoltsT = bolts;
			mBoltsText.text = mBoltsT.ToString();
		}
	}

	public void setBoxes()
	{
		for(int i = 0; i < mBoxObj.Length; i++)
		{
			mBoxObj[i].SetActive(true);
		}

		for(int i = mBoxes; i < mBoxObj.Length; i++)
		{
			mBoxObj[i].SetActive(false);
		}

		if(mBoxes > 4)
		{
			for(int i = 1; i < mBoxObj.Length; i++)
			{
				mBoxObj[i].SetActive(false);
			}
		}
		switch (mBoxes)
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

    public bool IsOpen()
    {
        return mOpen;
    }
}
