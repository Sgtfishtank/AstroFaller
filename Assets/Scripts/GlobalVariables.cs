using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour 
{
	// GLOBAL VARIABLES START HERE


	/*----------------------------------------AstroidSpawn----------------------------------*/
	public float ASTROID_SPAWN_SPAWNRATE 				= 5f   ;
	public float ASTROID_SPAWN_XOFFSET					= 10f  ; 
	public float ASTROID_SPAWN_ROTATION_SPEED 			= 10f ;
	public int ASTROID_SPAWN_MAX_ASTROIDS				= 5;
	public int ASTROID_SPAWN_MAX_PARTICLES				= 3;
	public int ASTROID_SPAWN_IMPACT_FACTOR				= 25;

	/*----------------------------------------Player----------------------------------------*/

	public float PLAYER_DASH_CD							 = 10f  ;
	public int   BOLT_VALUE								 = 1	;
	public int   BOLT_CLUSTER_VALUE						 = 1	;
	public float BOLT_TEXT_SHOW_TIME					= 5;
	public float PLAYER_HORIZONTAL_MOVESPEED			= 1000f;
	public float PLAYER_HORIZONTAL_MOVESPEED_KEYBORD	= 1000f;
	public float PLAYER_VERTICAL_SPEED_FALLOF			= 2f ;
	public float PLAYER_DASH_SPEED_DELAY				= 2f   ;
	public float PLAYER_DASH_SPEED						= 20f  ;
	public float PLAYER_MAX_HORIZONTAL_MOVESPEED	= 10f  ;
	public float PLAYER_HOVER_FORCE						= 15f	;
	public float PLAYER_MIN_HOVER_SPEED 				= 1.5f;
	public float PLAYER_HOVER_FAILED_FORCE				= 10;
	public float PLAYER_HORIZONTAL_MOVESPEED_HOVER_FACTOR = 0.5f;
	public float PLAYER_HOVER_FAILED_TIME				= 0.45f;
	public float PLAYER_MINMAX_X						= 5;
	public int   PLAYER_MAX_LIFE						= 3		;
	public float PLAYER_MAX_AIR							= 10;
	public float PLAYER_AIR_DRAIN						= 10;
	public float PLAYER_AIR_REG							= 10;

	// other
	public int MAX_TEXT_PARTICLES						= 15;
	public int MAX_BOLT_PARTICLES						= 12;
	public int PERFECT_DISTANCE_SIZE					= 10;
	public float ASTEROID_WARNING_MAX_SHOW_TIME			= 2;
	public float LOAD_LEVEL_DELAY						= 3;

	// WorldGen
	public float WORLD_SHIFT_BACK_INTERVAL				= 1000f;
	public float WORLD_GEN_INTRO_TIME					= 3f;

	// MAIN MENU CAMERA
	public float MAIN_CAMERA_MOVE_ZOOM_OUT_FACTOR		= 1f;
	public float MAIN_CAMERA_OFFSET_MOVE_SPEED			= 0.387f;
	public Vector3 MAIN_CAMERA_OFFSET					= new Vector3(0, 0, 230);
	public Vector3 MAIN_CAMERA_START_LEVEL_ZOOM			= new Vector3(0, 0, -460);

	// MAIN_MENU_GUI_CANVAS
	public float BUTTON_PRESS_MOVE_TIME				= 0.8f;

	// WORLD_MAP_MENU
	public float WORLD_MAP_SCROLL_OFFSET = 60;
	public float WORLD_MAP_LEVELS_SIZE = 60;
	public float WORLD_MAP_LEVELS_SNAP_SPEED = 4;
	public float WORLD_MAP_LEVELS_SCROLL_SPEED = 180;

	// LEVELS
	public float LEVELS_FOCUS_ZOOM = 100;

	// tutorial level

	// astreoid level
	public int ASTEROID_BONUS_1_CRITERA_DISTANCE = 1000;
	public int ASTEROID_BONUS_1_REWARD_BOLTS = 1000;
	public int ASTEROID_BONUS_1_REWARD_CRYSTALS = 1000;
	public int ASTEROID_BONUS_2_CRITERA_DISTANCE = 1000;
	public int ASTEROID_BONUS_2_REWARD_BOLTS = 1000;
	public int ASTEROID_BONUS_2_REWARD_CRYSTALS = 1000;

	// cosmic storm level
	public int COSMIC_LEVEL_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_1_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_1_REWARD_BOLTS = 1000;
	public int COSMIC_BONUS_1_REWARD_CRYSTALS = 1000;
	public int COSMIC_BONUS_2_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_2_REWARD_BOLTS = 1000;
	public int COSMIC_BONUS_2_REWARD_CRYSTALS = 1000;
	
	// satelite graveyard level
	public int SATELITE_LEVEL_CRITERA_DISTANCE = 1000;
	public int SATELITE_BONUS_1_CRITERA_DISTANCE = 1000;
	public int SATELITE_BONUS_1_REWARD_BOLTS = 1000;
	public int SATELITE_BONUS_1_REWARD_CRYSTALS = 1000;
	public int SATELITE_BONUS_2_CRITERA_DISTANCE = 1000;
	public int SATELITE_BONUS_2_REWARD_BOLTS = 1000;
	public int SATELITE_BONUS_2_REWARD_CRYSTALS = 1000;

	// black hole level
	public int BLACK_HOLE_LEVEL_CRITERA_DISTANCE = 1000;

	// PERKS
	public int PERKS_MAX_LEVEL = 3;
	private const int PERKS_ARRAY_SIZE = 3;

	// Air Perk
	public int[] AIR_PERK_COST_CRYSTALS = new int[PERKS_ARRAY_SIZE];
	public int[] AIR_PERK_COST_BOLTS = new int[PERKS_ARRAY_SIZE];
	//public int[] AIR_PERK_LEVELS = new int[PERKS_ARRAY_SIZE];
	public string[] AIR_PERK_DESCRIPTION = new string[PERKS_ARRAY_SIZE];
	//public string[] AIR_PERK_LEVELS_UNIT = new string[PERKS_ARRAY_SIZE];

	// Life Perk
	public int[] LIFE_PERK_COST_CRYSTALS = new int[PERKS_ARRAY_SIZE];
	public int[] LIFE_PERK_COST_BOLTS = new int[PERKS_ARRAY_SIZE];
	public string[] LIFE_PERK_DESCRIPTION = new string[PERKS_ARRAY_SIZE];

	// Burst Perk
	public int[] BURST_PERK_COST_CRYSTALS = new int[PERKS_ARRAY_SIZE];
	public int[] BURST_PERK_COST_BOLTS = new int[PERKS_ARRAY_SIZE];
	public string[] BURST_PERK_DESCRIPTION = new string[PERKS_ARRAY_SIZE];

	// ITEMS
	public int ITEMS_START_LEVEL = 0;
	public int ITEMS_MAX_LEVEL = 6;
	private const int ITEMS_ARRAY_SIZE = 6;

	// Ulimited air
	public int[] UNLIMITED_AIR_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE];
	public int[] UNLIMITED_AIR_COST_BOLTS = new int[ITEMS_ARRAY_SIZE];
	public int[] UNLIMITED_AIR_LEVELS = new int[ITEMS_ARRAY_SIZE + 1];
	public string UNLIMITED_AIR_DESCRIPTION = "---";
	public string UNLIMITED_AIR_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] BOLT_MAGNET_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE];
	public int[] BOLT_MAGNET_COST_BOLTS = new int[ITEMS_ARRAY_SIZE];
	public int[] BOLT_MAGNET_LEVELS = new int[ITEMS_ARRAY_SIZE + 1];
	public string BOLT_MAGNET_DESCRIPTION = "---";
	public string BOLT_MAGNET_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] BOLT_MULTIPLIER_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE];
	public int[] BOLT_MULTIPLIER_COST_BOLTS = new int[ITEMS_ARRAY_SIZE];
	public int[] BOLT_MULTIPLIER_LEVELS = new int[ITEMS_ARRAY_SIZE + 1];
	public string BOLT_MULTIPLIER_DESCRIPTION = "---";
	public string BOLT_MULTIPLIER_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] FORCE_FIELD_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE];
	public int[] FORCE_FIELD_COST_BOLTS = new int[ITEMS_ARRAY_SIZE];
	public int[] FORCE_FIELD_LEVELS = new int[ITEMS_ARRAY_SIZE + 1];
	public string FORCE_FIELD_DESCRIPTION = "---";
	public string FORCE_FIELD_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] ROCKET_THRUST_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE];
	public int[] ROCKET_THRUST_COST_BOLTS = new int[ITEMS_ARRAY_SIZE];
	public int[] ROCKET_THRUST_LEVELS = new int[ITEMS_ARRAY_SIZE + 1];
	public string ROCKET_THRUST_DESCRIPTION = "---";
	public string ROCKET_THRUST_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] SHOCKWAVE_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE];
	public int[] SHOCKWAVE_COST_BOLTS = new int[ITEMS_ARRAY_SIZE];
	public int[] SHOCKWAVE_LEVELS = new int[ITEMS_ARRAY_SIZE + 1];
	public string SHOCKWAVE_DESCRIPTION = "---";
	public string SHOCKWAVE_LEVELS_UNIT = "---";

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	// sigleton
	private static GlobalVariables instance = null;
	public static GlobalVariables Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject _thisObject = GameObject.Find("GlobalVaribelsPrefab");
				instance = _thisObject.GetComponent<GlobalVariables>();
			}
			return instance;
		}
	}

	public int DistanceCritera (string levelName)
	{
		switch (levelName) 
		{
		case "bonus 1": 
			return ASTEROID_BONUS_1_CRITERA_DISTANCE;
		case "bonus 2": 
			return ASTEROID_BONUS_2_CRITERA_DISTANCE;
		case "Cosmic storm": 
			return COSMIC_LEVEL_CRITERA_DISTANCE;
		case "bonus 3": 
			return COSMIC_BONUS_1_CRITERA_DISTANCE;
		case "bonus 4":
			return COSMIC_BONUS_2_CRITERA_DISTANCE;
		case "satellite graveyard":
			return SATELITE_LEVEL_CRITERA_DISTANCE;
		case "bonus 5":
			return SATELITE_BONUS_1_CRITERA_DISTANCE;
		case "bonus 6":
			return SATELITE_BONUS_2_CRITERA_DISTANCE;
		case "black hole":
			return BLACK_HOLE_LEVEL_CRITERA_DISTANCE;
		case "Alien Territory":
			return ASTEROID_BONUS_1_CRITERA_DISTANCE;
		default:
			print("Error in DistanceCritera " + levelName);
			break;
		}
		
		return 0;
	}

	public int BonusRewardBolts(string levelName)
	{
		switch (levelName) 
		{
		case "bonus 1": 
			return ASTEROID_BONUS_1_REWARD_BOLTS;
		case "bonus 2": 
			return ASTEROID_BONUS_2_REWARD_BOLTS;
		case "bonus 3": 
			return COSMIC_BONUS_1_REWARD_BOLTS;
		case "bonus 4": 
			return COSMIC_BONUS_2_REWARD_BOLTS;
		case "bonus 5": 
			return SATELITE_BONUS_1_REWARD_BOLTS;
		case "bonus 6":
			return SATELITE_BONUS_2_REWARD_BOLTS;
		default:
			print("Error in BonusRewardBolts " + levelName);
			break;
		}

		return 0;
	}

	public int BonusRewardCrystals (string levelName)
	{
		switch (levelName) 
		{
		case "bonus 1": 
			return ASTEROID_BONUS_1_REWARD_CRYSTALS;
		case "bonus 2": 
			return ASTEROID_BONUS_2_REWARD_CRYSTALS;
		case "bonus 3": 
			return COSMIC_BONUS_1_REWARD_CRYSTALS;
		case "bonus 4": 
			return COSMIC_BONUS_2_REWARD_CRYSTALS;
		case "bonus 5": 
			return SATELITE_BONUS_1_REWARD_CRYSTALS;
		case "bonus 6":
			return SATELITE_BONUS_2_REWARD_CRYSTALS;
		default:
			print("Error in BonusRewardCrystals " + levelName);
			break;
		}
		
		return 0;
	}

	public GameObject Instanciate(GameObject prefab, Transform parent, float scale)
	{
		if (prefab == null) 
		{
			print("ERORR null in Instanciate ");
			return null;
		}

		GameObject sammax = GameObject.Instantiate (prefab);
		sammax.transform.parent = parent;
		sammax.transform.localPosition = Vector3.zero;
		sammax.transform.localRotation = Quaternion.identity;
		sammax.transform.localScale = Vector3.one * scale;
		sammax.transform.name = prefab.name;
		return sammax;
	}
}
