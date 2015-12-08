using UnityEngine;
using System;
using System.Collections;

public class InGameCamera : MonoBehaviour 
{
	// snigleton
	private static InGameCamera instance = null;

	public static InGameCamera Instance
	{
		get
        {
			if (instance == null)
			{
                if (PlayerData.Instance.CurrentScene() != PlayerData.Scene.IN_GAME)
                {
                    throw new NotImplementedException();
                }

				instance = Singleton<InGameCamera>.CreateInstance("Prefab/InGame/InGame Camera");
			}
			return instance;
		}
	}
	
	private TextMesh mDistnceText;
	private TextMesh mBoltsText;
	private TextMesh mBoxesText;
	private TextMesh mLifeText;
	private Camera mCamera;
	private int mDistnce;
	private int mBolts;
	private int mBoxes;
	private int mLife;
    public GameObject crash = null;

	void Awake()
	{
		mCamera = GetComponent<Camera> ();
		mBoltsText = transform.Find ("UI/Bolt_Count_text").GetComponent<TextMesh> ();
		mDistnceText = transform.Find ("UI/Distance_Text").GetComponent<TextMesh> ();
		mBoxesText = transform.Find ("UI/Perfect_Distance_Boxes/Amount_Of_Boxes_Text").GetComponent<TextMesh> ();
		mLifeText = transform.Find ("UI/Life_Count_text").GetComponent<TextMesh> ();

	}

	void OnEnable()
	{
		GetComponent<FollowPlayer> ().ydist = 0;
	}
	
	void OnDisable()
	{
		//crash.transform.position = Vector3.zero;
	}

	// Use this for initialization
	void Start () 
	{
        mDistnce = -1;
        mBolts = -1;
        mBoxes = -1;
        mLife = -1;

        UpdateBoltsText();
        UpdateDistnceText();
        UpdateBoxesText();
        UpdateLifeText();
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateBoltsText();
		UpdateDistnceText();
		UpdateBoxesText();
		UpdateLifeText();

	}

    private void UpdateDistnceText()
	{
		// avoid string allocations
		if (mDistnce != InGame.Instance.Player().Distance()) 
		{
			mDistnce = InGame.Instance.Player().Distance();
			if (mDistnce >= 1000000)
				mDistnceText.text = (mDistnce / 1000000).ToString() + "M";
			else if (mDistnce >= 10000)
				mDistnceText.text = (mDistnce / 1000).ToString() + "K";
			else
				mDistnceText.text = mDistnce.ToString();
		}
    }

    private void UpdateBoltsText()
	{
		// avoid string allocations
		if (mBolts != InGame.Instance.Player().colectedBolts()) 
		{
			mBolts = InGame.Instance.Player().colectedBolts();
			if (mBolts >= 10000)
				mBoltsText.text = (mBolts / 1000).ToString() + "K";
			else
				mBoltsText.text = mBolts.ToString();
		}
    }

    private void UpdateBoxesText()
	{
		// avoid string allocations
		if (mBoxes != InGame.Instance.Player().CollectedPerfectDistances()) 
		{
			mBoxes = InGame.Instance.Player().CollectedPerfectDistances();
			if (mBoxes >= 1000)
				mBoxesText.text = (mBoxes / 1000).ToString() + " K";
			else
				mBoxesText.text = mBoxes.ToString();
		}
    }

    private void UpdateLifeText()
	{
		// avoid string allocations
		if (mLife != InGame.Instance.Player().LifeRemaining())
		{
			mLife = InGame.Instance.Player().LifeRemaining();
			mLifeText.text = InGame.Instance.Player().LifeRemaining().ToString();
		}
    }

	public Camera Camera()
	{
		return mCamera;
	}
}
