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
	
	public GameObject mPlayerPrefab;
	public GameObject mAstroidSpawnPrefab;
	
	public int[] mTmies;
	public GameObject[] mSegments;
	public int[] mSegmentWeightDynamic;
	public int[] mSegmentWeightStatic;
	public int mSum;
	private GameObject mCurrentSegment = null;
	private GameObject mNextSegment;
	private float mCurrentPos;
	private float mOffset = 50;

	public Player mPlayer;
	public AstroidSpawn mAstroidSpawn;
	private string mCurrentLevel;
	
	private GameObject[] mBgSegments;
	private GameObject mCurrentBgSegment;
	private GameObject mNextBgSegment;
	private float mCurrentBgPos;
	private float mBgOffset = 600;

	public bool mIntroPhase;
	public float mIntroPhaseT;
	private GameObject mDirectionalLight;
	public GameObject mDirectionalLightPrefab;
	public float mStartTime = -1;

	public float mUsualShiftkingRailgun = 0;
	
	private FMOD.Studio.EventInstance fmodMusic;

	void Awake()
	{
		GameObject playerObj = GameObject.Find("Player");
		if (playerObj == null)
		{
			playerObj = GameObject.Instantiate(mPlayerPrefab);
			playerObj.name = mPlayerPrefab.name;
		}
		mPlayer = playerObj.GetComponent<Player>();

		GameObject astroidSpawnObj = GameObject.Find("AstroidSpawn");
		if (astroidSpawnObj == null)
		{
			astroidSpawnObj = GameObject.Instantiate(mAstroidSpawnPrefab);
			astroidSpawnObj.name = mAstroidSpawnPrefab.name;

		}
		mAstroidSpawn = astroidSpawnObj.GetComponent<AstroidSpawn>();

		GameObject dirLightObj = GameObject.Find("InGame DirectionalLight");
		if (dirLightObj == null)
		{
			dirLightObj = GameObject.Instantiate(mDirectionalLightPrefab);
			dirLightObj.name = mDirectionalLightPrefab.name;
		}
		mDirectionalLight = dirLightObj;
		fmodMusic = FMOD_StudioSystem.instance.GetEvent("event:/Music/DroneMenyMusic/SpaceDrone");

	}

	// Use this for initialization
	void Start ()
	{
	}
	
	public Player Player ()
	{
		return mPlayer;
	}
	
	public AstroidSpawn AstroidSpawn ()
	{
		return mAstroidSpawn;
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

				mPlayer.GetComponent<Player>().StartGame();

				mStartTime = Time.time;

				mUsualShiftkingRailgun = 0;

				SpawnStartSegments();
			}
		}
		else
		{
			if(mCurrentPos > mPlayer.transform.position.y)
			{
				NextSegment();
			}

			if (mCurrentBgPos > mPlayer.transform.position.y)
			{
				NextBgSegment();
			}

			if (mPlayer.transform.position.y < -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL)
			{
				ShiftBackWorld();
			}
		}
	}
	int PickSegment()
	{
		int valie = UnityEngine.Random.Range (0, mSum);

		int retval = -1;

		int valueToCompare = 0;
		for (int i = 0; i < mSegmentWeightDynamic.Length; i++)
		{
			valueToCompare += mSegmentWeightDynamic[i];
			if ((valueToCompare > valie) && (retval == -1))
			{
				mSum += mSegmentWeightStatic[i] - mSegmentWeightDynamic[i];
				mSegmentWeightDynamic[i] = mSegmentWeightStatic[i];
				retval = i;
			}
			else
			{
				mSegmentWeightDynamic[i] += mSegmentWeightStatic[i];
				mSum += mSegmentWeightStatic[i];
			}
		}

		mTmies[retval]++;
		return retval;
	}

	void NextSegment ()
	{
		print("next " + Time.time);
		Destroy(mCurrentSegment);
		mCurrentSegment = mNextSegment;
		mCurrentPos -= mOffset;
		GameObject segmentPrefab = mSegments [PickSegment ()];

		Vector3 pos = new Vector3 (0, mCurrentPos, 0);
		mNextSegment = Instantiate(segmentPrefab, pos, Quaternion.identity) as GameObject;
	}

	void NextBgSegment ()
	{
		Vector3 pos; 
		GameObject segmentPrefab;

		Destroy(mCurrentBgSegment);
		mCurrentBgSegment = mNextBgSegment;
		mCurrentBgPos -= mBgOffset;

		segmentPrefab = mBgSegments[UnityEngine.Random.Range(0, mBgSegments.Length)];
		pos = new Vector3 (0, mCurrentBgPos, 0);
		pos += segmentPrefab.transform.position;
		pos += Random.insideUnitSphere * 100;
		mNextBgSegment = Instantiate(segmentPrefab, pos, Quaternion.identity) as GameObject;
		mNextBgSegment.transform.localScale = segmentPrefab.transform.localScale * (0.75f + (Random.value * 0.5f));
	}

	void SpawnStartSegments()
	{
		mCurrentPos = mPlayer.transform.position.y - mOffset;
		Vector3 pos = new Vector3 (0, mCurrentPos, 0);
		mCurrentSegment = Instantiate(mSegments[1], pos, Quaternion.identity) as GameObject;
		
		mCurrentPos -= mOffset;
		GameObject segmentPrefab = mSegments[PickSegment()];
		pos = new Vector3 (0, mCurrentPos, 0);
		mNextSegment = Instantiate(segmentPrefab, pos, Quaternion.identity) as GameObject;
	}

	void SpawnBgStartSegments()
	{
		Vector3 pos; 
		GameObject segmentPrefab;

		mCurrentBgPos = mPlayer.transform.position.y;

		segmentPrefab = mBgSegments [1];
		pos = new Vector3 (0, mCurrentBgPos, 0);
		pos += segmentPrefab.transform.position;
		pos += Random.insideUnitSphere * 100;
		mCurrentBgSegment = Instantiate(segmentPrefab, pos, Quaternion.identity) as GameObject;
		mCurrentBgSegment.transform.localScale = segmentPrefab.transform.localScale * (0.75f + (Random.value * 0.5f));

		mCurrentBgPos -= mBgOffset;

		segmentPrefab = mBgSegments[UnityEngine.Random.Range(0, mBgSegments.Length)];
		pos = new Vector3 (0, mCurrentBgPos, 0);
		pos += segmentPrefab.transform.position;
		pos += Random.insideUnitSphere * 100;
		mNextBgSegment = Instantiate(segmentPrefab, pos, Quaternion.identity) as GameObject;
		mNextBgSegment.transform.localScale = segmentPrefab.transform.localScale * (0.75f + (Random.value * 0.5f));
	}

	public float LevelRunTime()
	{
		if (mStartTime < 0)
		{
			return -1;
		}

		return Time.time - mStartTime;
	}

	public float fallShift()
	{
		return mUsualShiftkingRailgun;
	}
	
	void ShiftBackWorld()
	{
		return;

		float shift = -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL;

		mUsualShiftkingRailgun -= shift;
		mCurrentBgPos -= shift;
		mCurrentPos -= shift;
		if(mCurrentSegment != null)
		{
			mCurrentSegment.transform.position -= new Vector3(0, shift, 0);
			mNextSegment.transform.position -= new Vector3(0, shift, 0);
		}
		if(mCurrentBgSegment != null)
		{
			mCurrentBgSegment.transform.position -= new Vector3(0, shift, 0);
			mNextBgSegment.transform.position -= new Vector3(0, shift, 0);
		}
		mPlayer.transform.position -= new Vector3(0, shift, 0);
		mPlayer.GetComponent<Player>().mAS.GetComponent<AstroidSpawn>().ShiftBack(shift);

		InGameCamera.Instance.GetComponent<FollowPlayer>().UpdatePosition ();
	}
	void DespawnSegments() 
	{
		if (mNextSegment != null)
		{
			Destroy(mNextSegment);
		}
		
		if (mCurrentSegment != null)
		{
			Destroy(mCurrentSegment);
		}
	}
	
	void DespawnBgSegments() 
	{
		if (mNextBgSegment != null)
		{
			Destroy(mNextBgSegment);
		}
		
		if (mCurrentBgSegment != null)
		{
			Destroy(mCurrentBgSegment);
		}
	}

	void ShowComponents(bool show)
	{
	
	}

	public void Disable() 
	{
		print("WorldGen Off");
		
		DespawnSegments ();
		DespawnBgSegments ();

		mBgSegments = null;
		mSegments = null;

//		showComponents(true);
		InGameCamera.Instance.gameObject.SetActive (false);
		GUICanvas.Instance.ShowInGameButtons(false);
		GUICanvas.Instance.ShowOptionButtons(false);
		gameObject.SetActive (false);
		mDirectionalLight.SetActive (false);
		
		AudioManager.Instance.StopMusic(fmodMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	
		mAstroidSpawn.gameObject.SetActive (false);

		if (mPlayer != null) 
		{
			mPlayer.GetComponent<Player>().DepositData();
			mPlayer.gameObject.SetActive (false);
		}

		mStartTime = -1;
	}
	
	public void Enable(string levelName)
	{
		print("WorldGen On");
		mCurrentLevel = levelName;
		
		mBgSegments = Resources.LoadAll<GameObject>("Parralax") as GameObject[];
		mSegments = Resources.LoadAll<GameObject>(mCurrentLevel) as GameObject[];
		mSegmentWeightStatic = new int[mSegments.Length];
		mSegmentWeightDynamic = new int[mSegments.Length];
		mTmies = new int[mSegments.Length];


		mAstroidSpawn.gameObject.SetActive (false);
		mPlayer.gameObject.SetActive (false);
		
		AudioManager.Instance.PlayMusic(fmodMusic);

		InGameCamera.Instance.gameObject.SetActive (true);
		GUICanvas.Instance.ShowInGameButtons(true);
		GUICanvas.Instance.ShowBackToMenuButton(false);
		if (mPlayer != null) 
		{
			mPlayer.gameObject.SetActive (true);
			mPlayer.GetComponent<Rigidbody>().useGravity = true;
			mPlayer.transform.position = Vector3.zero;
		}
		GUICanvas.Instance.ShowOptionButtons(false);
		
		mAstroidSpawn.gameObject.SetActive (false);
		mDirectionalLight.SetActive (true);

		gameObject.SetActive (true);

		SpawnBgStartSegments();

		mIntroPhase = true;
		mIntroPhaseT = 0;
		for (int i = 0; i < mSegmentWeightStatic.Length; i++)
		{
			if(mSegments[i].name == "Asteroids_trigger_area")
			{
				mSegmentWeightStatic[i] = 5;
				mSum += 5;
			}
			else
			{
				mSegmentWeightStatic[i] = 1;
				mSum += 1;
			}
		}

		for (int i = 0; i < mSegments.Length; i++) 
		{
			mSegmentWeightDynamic[i] = mSegmentWeightStatic[i];
			mTmies[i] = 0;
		}
	}
}
