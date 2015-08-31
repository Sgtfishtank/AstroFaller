﻿using UnityEngine;
using System.Collections;

public class InGame : MonoBehaviour 
{
	private static InGame instance = null;
	public static InGame Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("Game");
				instance = thisObject.GetComponent<InGame>();
			}
			return instance;
		}
	}
	
	public GameObject mPlayerPrefab;
	public GameObject mAstroidSpawnPrefab;
	
	public Player mPlayer;
	public AstroidSpawn mAstroidSpawn;
	private string mCurrentLevel;
	
	public float mUsualShiftkingRailgun = 0;
	
	private FMOD.Studio.EventInstance fmodMusic;
	
	public bool mIntroPhase;
	public float mIntroPhaseT;
	private GameObject mDirectionalLight;
	public GameObject mDirectionalLightPrefab;
	public float mStartTime = -1;

	private WorldGen mWorldGen;
	private WorldGen mBgGen;

	void Awake()
	{
		mWorldGen = transform.Find("WorldGen").GetComponent<WorldGen>();
		mBgGen = transform.Find("BgGen").GetComponent<WorldGen>();

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
			astroidSpawnObj.transform.parent = transform;
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
	
	public Player Player ()
	{
		return mPlayer;
	}
	
	public AstroidSpawn AstroidSpawn ()
	{
		return mAstroidSpawn;
	}
	
	public int CurrentLevel()
	{
		return 1;
	}

	// Use this for initialization
	void Start () 
	{
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
				
				mPlayer.StartGame();
				
				mStartTime = Time.time;
				
				mWorldGen.StartSpawnSegments (mPlayer.transform.position.y - 25);
			}
		}
		else
		{
			if (mPlayer.transform.position.y < -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL)
			{
				ShiftBackWorld();
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
	
	public float FallShift()
	{
		return mUsualShiftkingRailgun;
	}
	
	void ShiftBackWorld()
	{
		float shift = -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL;
		
		mUsualShiftkingRailgun -= shift;

		mBgGen.ShiftBack(shift);
		mWorldGen.ShiftBack(shift);
		mPlayer.ShiftBack(shift);
		mAstroidSpawn.ShiftBack(shift);
		
		InGameCamera.Instance.GetComponent<FollowPlayer>().UpdatePosition ();
	}
	
	void ShowComponents(bool show)
	{
		InGameCamera.Instance.gameObject.SetActive (show);
		GUICanvas.Instance.ShowInGameButtons(show);
		GUICanvas.Instance.ShowBackToMenuButton(show);
		GUICanvas.Instance.ShowOptionButtons (false);
		
		mAstroidSpawn.gameObject.SetActive (false);
		mDirectionalLight.SetActive (show);
		gameObject.SetActive (show);
		mPlayer.gameObject.SetActive (show);
	}
	
	public void Disable() 
	{
		mWorldGen.StopSpawnSegments ();
		mBgGen.StopSpawnSegments ();

		ShowComponents(false);
		
		mWorldGen.UnloadSegments ();
		mBgGen.UnloadSegments ();

		AudioManager.Instance.StopMusic(fmodMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		
		//mPlayer.DepositData();
		
		mStartTime = -1;
	}
	
	public void Enable(int levelIndex)
	{
		mCurrentLevel = "Level" + levelIndex;

		ShowComponents(true);
		
		mBgGen.LoadSegments("Parralax", 500, 100);
		mWorldGen.LoadSegments("Level" + InGame.Instance.CurrentLevel(), 50, -1);

		mBgGen.StartSpawnSegments(0);

		AudioManager.Instance.PlayMusic(fmodMusic);
		
		mPlayer.GetComponent<Rigidbody>().useGravity = true;
		mPlayer.transform.position = Vector3.zero;
		
		mUsualShiftkingRailgun = 0;
		mIntroPhase = true;
		mIntroPhaseT = 0;
	}
}