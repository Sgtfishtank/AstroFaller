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
	private float mMulti;
	private int mDis;
	private int mTotalBolts;
	private int mBolts;
	private FMOD.Studio.EventInstance mDisDown;
	private FMOD.Studio.EventInstance mCoinUp;
	private FMOD.Studio.EventInstance fmodDeathMusic;
	private bool runSound; 
	private int mDistance;
	private int mBoxes;
	private int mBolts2;
	//private ButtonManager mRestatButton;
	//private ButtonManager mMenuButton;

	void Awake()
	{
		DeatMenuGUI gui = InGameGUICanvas.Instance.DeathMenuGUI ();
		ButtonManager.CreateButton(gameObject, "button_1_base", "Restart", gui);
		ButtonManager.CreateButton(gameObject, "button_2_base", "MainMenu", gui);

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

		fmodDeathMusic = AudioManager.Instance.GetEvent("Music/Scrapscoremusic/ScrapScoreMusic");
		mDisDown = AudioManager.Instance.GetEvent("Sounds/RewardTickerBolts/TickerBolts");
		mCoinUp = AudioManager.Instance.GetEvent("Sounds/RewardTickerDistance/TickerDistance");
	}

	// Use this for initialization
	void Start()
	{
	}

	void OnEnable ()
	{
		mMulti = -1;
		mDis = -1;
		mTotalBolts = -1;
		mBolts = -1;
		mCalcT = Time.time + mCalcDuration;
		setBoxes();

		UpdateDistanceText(0);
		UpdateMultiplierText(0);
		UpdateTotalBoltsText(0);
		UpdateBoltsText(0);
	}

	public void Open(int distance, int bolts, int boxes)
	{
		mDistance = distance;
		mBoxes = boxes;
		mBolts2 = bolts;
		setBoxes();
		AudioManager.Instance.PlayMusic(fmodDeathMusic);
		InGameGUICanvas.Instance.DeathMenuGUI().setEnableDeathMenu(true);
	}
	
	public void Close()
	{
		AudioManager.Instance.StopMusic(fmodDeathMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Instance.StopSound(mCoinUp, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Instance.StopSound(mDisDown, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		InGameGUICanvas.Instance.DeathMenuGUI().setEnableDeathMenu(false);
	}

	public void Skip ()
	{
		mCalcT = 0;
	}

	void OnDisable()
	{
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

		if ((!runSound) && (deltaT > 0) && (deltaT < 1))
		{
			AudioManager.Instance.PlaySound(mCoinUp);
			AudioManager.Instance.PlaySound(mDisDown);
			runSound = true;
		}
		else if (runSound && (deltaT >= 1))
		{
			AudioManager.Instance.StopSound(mCoinUp, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.Instance.StopSound(mDisDown, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			runSound = false;
		}

		float multi = (int)(Mathf.Lerp(1, calculateMultiplier(), deltaT) * 100f);
		int dis = (int)Mathf.Lerp(mDistance, 0, deltaT);
		int totalBolts = (int)Mathf.Lerp(mBolts2, calculateMultiplier() * mBolts2, deltaT);

		UpdateDistanceText(dis);
		UpdateMultiplierText(multi);
		UpdateTotalBoltsText(totalBolts);
		UpdateBoltsText(mBolts2);
	}

	void UpdateTotalBoltsText(int totalBolts)
	{
		// avoid string allocations
		if (mTotalBolts != totalBolts) 
		{
			mTotalBolts = totalBolts;
			mTotalBoltsText.text = mTotalBolts.ToString();
		}
	}

	void UpdateMultiplierText(float multi)
	{
		// avoid string allocations
		if (mMulti != multi) 
		{
			mMulti = multi;
			mMultiText.text = mMulti.ToString();
		}
	}

	void UpdateDistanceText(int dis)
	{
		// avoid string allocations
		if (mDis != dis) 
		{
			mDis = dis;
			mDisText.text = mDis.ToString();
		}
	}
	
	void UpdateBoltsText(int bolts)
	{
		// avoid string allocations
		if (mBolts != bolts) 
		{
			mBolts = bolts;
			mBoltsText.text = mBolts.ToString();
		}
	}

	float calculateMultiplier()
	{
		return 1 + (mDistance / 5000f);
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
}
