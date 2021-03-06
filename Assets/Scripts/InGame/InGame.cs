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
                if (PlayerData.Instance.CurrentScene() != PlayerData.Scene.IN_GAME)
                {
                    throw new NotImplementedException();
                }

                instance = Singleton<InGame>.CreateInstance("Prefab/InGame/Game");
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
    private GameObject mDeathMenu;
    private Level mCurrentLevel;

    public float mUsualShiftkingRailgun = 0;

	private AudioInstanceData fmodMusic;
	private AudioInstanceData fmodPerfect;

    private ParticleManager mAstCollParticle1Manager;
    private ParticleManager mAstCollParticle2Manager;
    private ParticleManager mBulletCollParticleManager;
    private ParticleManager mMissileCollParticleManager;
	private ParticleManager mLightningCollParticleManager;

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
		// creat collision efets
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

        GameObject deathMenuObj = GameObject.Find("pop_up_menu_results");
        if (deathMenuObj == null)
        {
            deathMenuObj = GameObject.Instantiate(mDeathMenuPrefab);
            deathMenuObj.name = mDeathMenuPrefab.name;
        }
        mDeathMenu = deathMenuObj;
        mDeathMenu.SetActive(false);

        fmodMusic = AudioManager.Instance.GetMusicEvent("AsteroidLevelMusic/AsteroidMusicPrototype", false);
        fmodPerfect = AudioManager.Instance.GetSoundsEvent("PerfectDistance/PerfectDistance", true, 3);

        mPerfectDistanceMid = GameObject.Instantiate(mPerfectDistanceMidPrefab);

        // creat collision efets
        mAstCollParticle1Manager = GetComponents<ParticleManager>()[0];
        mAstCollParticle2Manager = GetComponents<ParticleManager>()[1];
        mBulletCollParticleManager = GetComponents<ParticleManager>()[2];
        mMissileCollParticleManager = GetComponents<ParticleManager>()[3];
        mLightningCollParticleManager = GetComponents<ParticleManager>()[4];
    }

    // Use this for initialization
    void Start()
    {
        System.GC.Collect();
    }

    void ActivateCorrectSpawner(Level level)
    {
        if (mSpawnerBase != null)
        {
            Debug.LogError(" Spawner not removed.");
        }

        GameObject astroidSpawnObj = GameObject.Instantiate(mSpawnerPrefabs[(int)level - 1]);
        astroidSpawnObj.name = mSpawnerPrefabs[(int)level - 1].name;
        astroidSpawnObj.transform.parent = transform;
        mSpawnerBase = astroidSpawnObj.GetComponent<SpawnerBase>();
    }

    public Player Player()
    {
        return mPlayer;
    }

    public SpawnerBase BaseSpawner()
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

    // Update is called once per frame
    void Update()
    {
        if (mIntroPhase)
        {
            if (startdelay == -1)
            {
                startdelay = Time.time + GlobalVariables.Instance.LOAD_LEVEL_DELAY;
            }

            if (startdelay > Time.time)
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
                AudioManager.Instance.StopMusic(fmodMusic);
            }

            if (mPlayer.transform.position.y < -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL)
            {
                ShiftBackWorld();
            }
        }
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayMusic(fmodMusic);
        mSpawnerBase.Reset();
        mWorldGen.DespawnSegments();
        mPlayer.StartGame();

        mStartTime = Time.time;
        mUsualShiftkingRailgun = 0;

        mPerfectDistanceMid.gameObject.SetActive(true);

        mWorldGen.StartSpawnSegments(mPlayer.transform.position.y - 25);
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

    public void UpdatePerfectDistance(float posY, bool triggerParticles)
    {
        if (triggerParticles)
        {
            GameObject a = GameObject.Instantiate(mPerfectDistanceParticlesPrefab, mPerfectDistanceParticlesPrefab.transform.position + mPerfectDistanceMid.transform.position, mPerfectDistanceParticlesPrefab.transform.rotation) as GameObject;
            a.transform.parent = transform.Find("ParticlesGoesHere");
            AudioManager.Instance.PlaySoundOnce(fmodPerfect);
        }

        float yValue = posY;
        mPerfectDistanceMid.transform.position = new Vector3(0, yValue, 0);
    }

    public void PlayerDied()
    {
        mWorldGen.StopSpawnSegments();
        mBgGen.StopSpawnSegments();
        mWorldGen.DespawnSegments();
        mBgGen.DespawnSegments();
    }

    void ShiftBackWorld()
    {
        float shift = -GlobalVariables.Instance.WORLD_SHIFT_BACK_INTERVAL;

        mUsualShiftkingRailgun -= shift;

        mBgGen.ShiftBack(shift);
        mWorldGen.ShiftBack(shift);
        mPlayer.ShiftBack(shift);
        mSpawnerBase.ShiftBack(shift);

        mPerfectDistanceMid.transform.position -= new Vector3(0, shift, 0);
        if (!mPlayer.isDead())
        {
            InGameCamera.Instance.GetComponent<FollowPlayer>().UpdatePosition();
        }

        switch (mCurrentLevel)
        {
            case Level.ASTROID_BELT:
                mAstCollParticle1Manager.ShiftBack(shift);
                mAstCollParticle2Manager.ShiftBack(shift);
                break;
            case Level.ALIEN_BATTLEFIELD:
                mBulletCollParticleManager.ShiftBack(shift);
                mMissileCollParticleManager.ShiftBack(shift);
                break;
            case Level.COSMIC_STORM:
                mLightningCollParticleManager.ShiftBack(shift);
                break;
        }
    }

    public void SpawnAstCollisionEffects(Vector3 position)
    {
        mAstCollParticle1Manager.Spawn(position);
        mAstCollParticle2Manager.Spawn(position);
    }

    public void SpawnBulletCollisionEffects(Vector3 position)
    {
        mBulletCollParticleManager.Spawn(position);
    }

    public void SpawnMissileCollisionEffects(Vector3 position)
    {
        mMissileCollParticleManager.Spawn(position);
    }

    public void SpawnLightningCollisionEffects(Vector3 position)
    {
        mLightningCollParticleManager.Spawn(position);
    }

    public bool OutOfSegmentBounds(GameObject spawninst)
    {
        Vector3 pos = spawninst.transform.position;
        float xMax = GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * 1.5f;
        float xMin = -GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * 1.5f;
        float yMax = mPlayer.transform.position.y + 25;
        float yMin = mPlayer.transform.position.y - 50;

        return ((pos.x > xMax) || (pos.x < xMin) || (pos.y > yMax) || (pos.y < yMin));
    }

    void ShowComponents(bool show)
    {
        InGameCamera.Instance.gameObject.SetActive(show);
        InGameGUICanvas.Instance.ShowInGameButtons(show);

        mPerfectDistanceMid.gameObject.SetActive(false);

        mDirectionalLight.SetActive(show);
        gameObject.SetActive(show);
        mPlayer.gameObject.SetActive(show);
    }

    public void Disable()
    {
        mWorldGen.StopSpawnSegments();
        mBgGen.StopSpawnSegments();

        ShowComponents(false);

        mWorldGen.UnloadSegments();
        mBgGen.UnloadSegments();

        if (mSpawnerBase != null)
        {
            mSpawnerBase.UnloadObjects();
            Destroy(mSpawnerBase);
            mSpawnerBase = null;
        }

        AudioManager.Instance.StopMusic(fmodMusic);

        mStartTime = -1;
    }

    public void Enable(Level level)
    {
        int maxParticles = GlobalVariables.Instance.SPAWN_COLLISON_MAX_PARTICLES;
        Transform parent = transform.Find("ParticlesGoesHere").transform;

        mCurrentLevel = level;

        ShowComponents(true);

        switch (mCurrentLevel)
        {
            case Level.ERROR:
                throw new NotImplementedException("Error level loaded!");
            case Level.ASTROID_BELT:
                mAstCollParticle1Manager.Load(maxParticles, parent);
                mAstCollParticle2Manager.Load(maxParticles, parent);
                mWorldGen.LoadSegments("Level1", 50, -1);
                mBgGen.LoadSegments("Parralax", 120, 5);
                break;
            case Level.ALIEN_BATTLEFIELD:
                mBulletCollParticleManager.Load(maxParticles, parent);
                mMissileCollParticleManager.Load(maxParticles, parent);
                mWorldGen.LoadSegments("Level2", 50, -1);
                mBgGen.LoadSegments("Parralax", 120, 5);
                break;
            case Level.COSMIC_STORM:
                mLightningCollParticleManager.Load(maxParticles, parent);
                mWorldGen.LoadSegments("LevelCosmicStorm", 50, -1);
                mBgGen.LoadSegments("Parralax", 120, 5);
                break;
            default:
                throw new NotImplementedException("Error no loaded!");
        }
        ActivateCorrectSpawner(mCurrentLevel);
        mSpawnerBase.LoadObjects();

        //mBgGen.StartSpawnSegments(0);

        mIntroPhase = true;
        mIntroPhaseT = 0;
        InGameGUICanvas.Instance.SetFadeColor(Color.black);
    }
}
