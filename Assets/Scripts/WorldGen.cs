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
	private GameObject mCurrentSegment;
	private GameObject mNextSegment;
	public GameObject mPlayer;
	private bool mFirstTime = true;
	private float mCurrentPos =-25;
	private float mOffset = 50;
	private string mCurrentLevel;

	public bool mIntroPhase;
	public float mIntroPhaseT;

	// Use this for initialization
	void Start ()
	{
		mPlayer = GameObject.Find("Player");

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

	public void Disable() 
	{
		print("WorldGen Off");

		InGameCamera.Instance.gameObject.SetActive (false);
		GUICanvas.Instance.ShowInGameButtons(false);
		gameObject.SetActive (false);

		if (mPlayer != null) 
		{
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
		}
		
		mSegments = Resources.LoadAll<GameObject>(mCurrentLevel) as GameObject[];
		
		mCurrentSegment = Instantiate(mSegments[1], new Vector3(0,mCurrentPos,0), Quaternion.identity) as GameObject;

		gameObject.SetActive (true);
		
		mIntroPhase = true;
		mIntroPhaseT = 0;
	}
}
