using UnityEngine;
using System;
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
				if (Application.loadedLevelName != "InGameLevel")
				{
					throw new NotImplementedException();
				}

				instance = Singleton<InGame>.CreateInstance("Prefab/Essential/InGame/Game");
			}
			return instance;
		}
	}

	public enum Level 
    {
        ERROR = 0, 
        ASTROID_BELT = 1, 
        ALIEN_BATTLEFIELD, 
        COSMIC_STORM,

        DEFAULT = ASTROID_BELT,
    };

	public GameObject mPlayerPrefab;
	public GameObject[] mSpawnerPrefabs;
	public GameObject mDirectionalLightPrefab;
	public GameObject mPerfectDistanceMidPrefab;
	public GameObject mPerfectDistanceParticlesPrefab;
	public GameObject mDeathMenuPrefab;
	
	private Player mPlayer;
	public SpawnerBase mSpawnerBase;
	public GameObject mDeathMenu;
	private Level mCurrentLevel;
	
	public float mUsualShiftkingRailgun = 0;
	
	private FMOD.Studio.EventInstance fmodMusic;
	private FMOD.Studio.EventInstance fmodPerfect;
	
	public bool mIntroPhase;
	public float mIntroPhaseT;
	private GameObject mDirectionalLight;
	public float mStartTime = -1;

	private WorldGen mWorldGen;
	private WorldGen mBgGen;

    private GameObject mPerfectDistanceMid;
    private float startdelay = -1;

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
		
		GameObject dirLightObj = GameObject.Find("InGame DirectionalLight");
		if (dirLightObj == null)
		{
			dirLightObj = GameObject.Instantiate(mDirectionalLightPrefab);
			dirLightObj.name = mDirectionalLightPrefab.name;
		}
		mDirectionalLight = dirLightObj;

		GameObject deathMenuObj = GameObject.Find ("pop_up_menu_results");
		if(deathMenuObj == null)
		{
			deathMenuObj = GameObject.Instantiate(mDeathMenuPrefab);
			deathMenuObj.name = mDeathMenuPrefab.name;
		}
		mDeathMenu = deathMenuObj;
		mDeathMenu.SetActive (false);
		
		fmodMusic = AudioManager.Instance.GetEvent("Music/AsteroidMusicPrototype/AsteroidMusicPrototyp");
		fmodPerfect = AudioManager.Instance.GetEvent("Sounds/PerfectDistance/PerfectDistance");

		mPerfectDistanceMid = GameObject.Instantiate (mPerfectDistanceMidPrefab);
	}

	void ActivateCorrectSpawner(Level level)
	{
		if (mSpawnerBase != null)
		{
			Debug.LogError(" Spawner not removed.");
		}

		GameObject astroidSpawnObj = GameObject.Instantiate(mSpawnerPrefabs[(int)level-1]);
		astroidSpawnObj.name = mSpawnerPrefabs[(int)level-1].name;
		astroidSpawnObj.transform.parent = transform;
		mSpawnerBase = astroidSpawnObj.GetComponent<SpawnerBase>();
	}
	
	public Player Player ()
	{
		return mPlayer;
	}
	
	public SpawnerBase BaseSpawner ()
	{
		return mSpawnerBase;
	}
	
	public Level CurrentLevel()
	{
		return mCurrentLevel;
	}

	public DeathMenu DeathMenu()
	{
		return mDeathMenu.GetComponent<DeathMenu>();
	}

	// Use this for initialization
	void Start () 
	{
        System.GC.Collect();
	}

	// Update is called once per frame
	void Update () 
	{
		if (mIntroPhase)
		{
			if(startdelay == -1)
			{
				startdelay = Time.time+GlobalVariables.Instance.LOAD_LEVEL_DELAY;
			}

			if(startdelay > Time.time)
			{
				return;
			}

			mIntroPhaseT = Mathf.Clamp01(mIntroPhaseT + (Time.deltaTime / GlobalVariables.Instance.WORLD_GEN_INTRO_TIME));
			
			Color fadeColor = Color.black;
			fadeColor.a = 1f - mIntroPhaseT;

			InGameGUICanvas.Instance.SetFadeColor(fadeColor);
			
			if (mIntroPhaseT >= 1f)
			{
				InGameGUICanvas.Instance.SetFadeColor(new Color(0, 0, 0, 0));
				mIntroPhase = false;
				StartGame();
			}
		}
		else
		{
			if (mPlayer.isDead())
			{
				AudioManager.Instance.StopMusic(fmodMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}

			if (mPlayer.transform.position.y < -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL)
			{
				ShiftBackWorld();
			}
		}
	}

	public void StartGame ()
	{
		AudioManager.Instance.PlayMusic(fmodMusic);
		mSpawnerBase.Reset();
		mWorldGen.DespawnSegments();
		mPlayer.StartGame();
		
		mStartTime = Time.time;
		mUsualShiftkingRailgun = 0;
		
		mPerfectDistanceMid.gameObject.SetActive (true);

		mWorldGen.StartSpawnSegments (mPlayer.transform.position.y - 25);
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

	public void UpdatePerfectDistance (float posY, bool triggerParticles)
	{
		if (triggerParticles)
		{
			GameObject a = GameObject.Instantiate (mPerfectDistanceParticlesPrefab, mPerfectDistanceParticlesPrefab.transform.position + mPerfectDistanceMid.transform.position, mPerfectDistanceParticlesPrefab.transform.rotation) as GameObject;
			a.transform.parent = transform.Find("ParticlesGoesHere");
			AudioManager.Instance.PlaySoundOnce(fmodPerfect);
		}

		float yValue = posY;
		mPerfectDistanceMid.transform.position = new Vector3 (0, yValue, 0);

	}

	public void PlayedDeath ()
	{
		mWorldGen.StopSpawnSegments ();
		mBgGen.StopSpawnSegments ();
		mWorldGen.DespawnSegments ();
		mBgGen.DespawnSegments ();
	}

	void ShiftBackWorld()
	{
		float shift = -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL;
		
		mUsualShiftkingRailgun -= shift;

		mBgGen.ShiftBack(shift);
		mWorldGen.ShiftBack(shift);
		mPlayer.ShiftBack(shift);
		mSpawnerBase.ShiftBack(shift);

		mPerfectDistanceMid.transform.position -= new Vector3 (0, shift, 0);
		if (!mPlayer.isDead ())
			InGameCamera.Instance.GetComponent<FollowPlayer>().UpdatePosition ();
	}
	
	void ShowComponents(bool show)
	{
		InGameCamera.Instance.gameObject.SetActive (show);
		InGameGUICanvas.Instance.ShowInGameButtons(show);
		
		mPerfectDistanceMid.gameObject.SetActive (false);

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
		
		if (mSpawnerBase != null) 
		{
			mSpawnerBase.UnloadObjects ();
			Destroy (mSpawnerBase);
			mSpawnerBase = null;
		}

		AudioManager.Instance.StopMusic(fmodMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

		mStartTime = -1;
	}
	
	public void Enable(InGame.Level level)
	{
        mCurrentLevel = level;

		ShowComponents(true);
		
		mBgGen.LoadSegments("Parralax"/*+(int)mCurrentLevel*/, 120, 5);
		mWorldGen.LoadSegments("Level" + (int)mCurrentLevel, 50, -1);
		ActivateCorrectSpawner(mCurrentLevel);
		mSpawnerBase.LoadObjects();

		mBgGen.StartSpawnSegments(0);

		mPlayer.IntroLoad();

		mIntroPhase = true;
		mIntroPhaseT = 0;
		InGameGUICanvas.Instance.SetFadeColor(Color.black);
	}
}
