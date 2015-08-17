using UnityEngine;
using System.Collections;

public class WorldGen : MonoBehaviour
{
	private static WorldGen instance = null;
	public static WorldGen Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("Game");
				instance = thisObject.GetComponent<WorldGen>();
			}
			return instance;
		}
	}

	public GameObject[] mSegments;
	private GameObject mCurrentSegment = null;
	private GameObject mNextSegment;
	public GameObject mPlayer;
	private bool mFirstTime = true;
	private float mCurrentPos =-25;
	private float mOffset = 50;
	private string mCurrentLevel;

	public bool mIntroPhase;
	public float mIntroPhaseT;
	private GameObject mDirectionalLight;
	public GameObject mDirectionalLightPrefab;
	public GameObject mPlayerPrefab;
	public float mStartTime = -1;

	// Use this for initialization
	void Start ()
	{
		mPlayer = GameObject.Find("Player");
		if (mPlayer == null)
		{
			mPlayer = GameObject.Instantiate(mPlayerPrefab);
		}

		mDirectionalLight = GameObject.Instantiate (mDirectionalLightPrefab);
		mDirectionalLight.SetActive (false);

		WorldGen.Instance.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (mIntroPhase)
		{
			mIntroPhaseT = Mathf.Clamp01(mIntroPhaseT + (Time.deltaTime / GlobalVariables.Instance.WORLD_GEN_INTRO_TIME));

			Color fadeColor = Color.black;
			fadeColor.a = 1f - mIntroPhaseT;
			GUICanvas.Instance.SetFadeColor(fadeColor);

			if (mIntroPhaseT >= 1f)
			{
				GUICanvas.Instance.SetFadeColor(new Color(0, 0, 0, 0));
				mIntroPhase = false;

				mSegments = Resources.LoadAll<GameObject>(mCurrentLevel) as GameObject[];

				mCurrentPos = mPlayer.transform.position.y - mOffset;
				mPlayer.GetComponent<Player>().StartGame();

				mStartTime = Time.time;

				mCurrentSegment = Instantiate(mSegments[1], new Vector3(0,mCurrentPos,0), Quaternion.identity) as GameObject;
			}
		}
		else
		{
			if(mCurrentPos > mPlayer.transform.position.y && !mFirstTime)
			{
				Destroy(mCurrentSegment);
				mCurrentSegment = mNextSegment;
				mNextSegment = Instantiate(mSegments[UnityEngine.Random.Range(0,mSegments.Length)],
				                           new Vector3 (0,mCurrentPos-mOffset,0),Quaternion.identity) as GameObject;

				mCurrentPos -= mOffset;
			}

			else if (mCurrentPos > mPlayer.transform.position.y)
			{
				mFirstTime = false;
				mNextSegment = Instantiate(mSegments[UnityEngine.Random.Range(0,mSegments.Length)],
				                           new Vector3 (0,mCurrentPos -mOffset,0),Quaternion.identity) as GameObject;
				

				mCurrentPos -= mOffset;
			}
		}
	}

	public float LevelRunTime()
	{
		if (mStartTime < 0)
		{
			return -1;
		}

		return Time.time - mStartTime;
	}

	public void Disable() 
	{
		print("WorldGen Off");

		InGameCamera.Instance.gameObject.SetActive (false);
		GUICanvas.Instance.ShowInGameButtons(false);
		gameObject.SetActive (false);
		mDirectionalLight.SetActive (false);

		mStartTime = -1;

		if (mPlayer != null) 
		{
			mPlayer.GetComponent<Player>().DepositData();
			mPlayer.gameObject.SetActive (false);
		}
	}
	
	public void Enable(string levelName)
	{
		print("WorldGen On");
		mCurrentLevel = levelName;

		InGameCamera.Instance.gameObject.SetActive (true);
		GUICanvas.Instance.ShowInGameButtons(true);
		GUICanvas.Instance.ShowBackToMenuButton (true);
		if (mPlayer != null) 
		{
			mPlayer.gameObject.SetActive (true);
			mPlayer.GetComponent<Rigidbody>().useGravity = true;
			mPlayer.transform.position = Vector3.zero;
		}

		mDirectionalLight.SetActive (true);
		gameObject.SetActive (true);
		
		mIntroPhase = true;
		mIntroPhaseT = 0;
	}
}
