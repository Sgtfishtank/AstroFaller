using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour 
{
	// sigleton
	private static GameObject _thisObject;

	// GLOBAL VARIABLES START HERE


	/*----------------------------------------AstroidSpawn----------------------------------*/
	public float ASTROID_SPAWN_SPAWNRATE 				 = 5f   ;
	public float ASTROID_SPAWN_XOFFSET					 = 10f  ; 
	public float ASTROID_SPAWN_ROTATION_SPEED 			 = 10f  ;
	public int   ASTROID_SPAWN_MAX_ASTROIDS				 = 5    ;

	/*----------------------------------------Player----------------------------------------*/
	public float PLAYER_HORIZONTAL_MOVESPEED			 = 1000f;
	public float PLAYER_HORIZONTAL_MOVESPEED_KEYBORD	 = 1000f;
	public float PLAYER_VERTICAL_SPEED_FALLOF			 = 2f ;
	public float PLAYER_DASH_SPEED_DELAY				 = 2f   ;
	public float PLAYER_DASH_SPEED						 = 20f  ;
	public float PLAYER_HORIZONTAL_MOVESPEED_MAX_SPEED	 = 10f  ;
	public float PLAYER_HOVER_FORCE						 = 15f	;
	public float PLAYER_HOVER							 = 1f   ;

	// MAIN MENU CAMERA
	public float MAIN_CAMERA_MOVE_ZOOM_OUT_FACTOR		= 1f;
	public float MAIN_CAMERA_OFFSET_MOVE_SPEED			= 0.387f;
	public Vector3 MAIN_CAMERA_OFFSET					= new Vector3(0, 0, 230);

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

	// cosmic storm level
	public int COSMIC_LEVEL_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_1_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_1_REWARD_BOLTS = 1000;
	
	// satelite

	// PERKS

	// Air Perk
	public int AIR_PERK_MAIN_COST_CRYSTALS = 1;
	public int AIR_PERK_MAIN_COST_BOLTS = 2;
	public int[] AIR_PERK_MAIN_LEVELS = new int[2]{0, 1};
	public string AIR_PERK_MAIN_DESCRIPTION = "---";
	public string AIR_PERK_MAIN_LEVELS_UNIT = "---";
	
	public int AIR_PERK_LEFT_COST_CRYSTALS = 4;
	public int AIR_PERK_LEFT_COST_BOLTS = 8;
	public int[] AIR_PERK_LEFT_LEVELS = new int[2]{0, 1};
	public string AIR_PERK_LEFT_DESCRIPTION = "---";
	public string AIR_PERK_LEFT_LEVELS_UNIT = "---";
	
	public int AIR_PERK_RIGHT_COST_CRYSTALS = 16;
	public int AIR_PERK_RIGHT_COST_BOLTS = 32;
	public int[] AIR_PERK_RIGHT_LEVELS = new int[2]{0, 1};
	public string AIR_PERK_RIGHT_DESCRIPTION = "---";
	public string AIR_PERK_RIGHT_LEVELS_UNIT = "---";

	// Life Perk
	public int LIFE_PERK_MAIN_COST_CRYSTALS = 1;
	public int LIFE_PERK_MAIN_COST_BOLTS = 2;
	public int[] LIFE_PERK_MAIN_LEVELS = new int[2]{0, 1};
	public string LIFE_PERK_MAIN_DESCRIPTION = "---";
	public string LIFE_PERK_MAIN_LEVELS_UNIT = "---";
	
	public int LIFE_PERK_LEFT_COST_CRYSTALS = 4;
	public int LIFE_PERK_LEFT_COST_BOLTS = 8;
	public int[] LIFE_PERK_LEFT_LEVELS = new int[2]{0, 1};
	public string LIFE_PERK_LEFT_DESCRIPTION = "---";
	public string LIFE_PERK_LEFT_LEVELS_UNIT = "---";
	
	public int LIFE_PERK_RIGHT_COST_CRYSTALS = 16;
	public int LIFE_PERK_RIGHT_COST_BOLTS = 32;
	public int[] LIFE_PERK_RIGHT_LEVELS = new int[2]{0, 1};
	public string LIFE_PERK_RIGHT_DESCRIPTION = "---";
	public string LIFE_PERK_RIGHT_LEVELS_UNIT = "---";

	// Burst Perk
	public int BURST_PERK_MAIN_COST_CRYSTALS = 1;
	public int BURST_PERK_MAIN_COST_BOLTS = 2;
	public int[] BURST_PERK_MAIN_LEVELS = new int[2]{0, 1};
	public string BURST_PERK_MAIN_DESCRIPTION = "---";
	public string BURST_PERK_MAIN_LEVELS_UNIT = "---";

	public int BURST_PERK_LEFT_COST_CRYSTALS = 4;
	public int BURST_PERK_LEFT_COST_BOLTS = 8;
	public int[] BURST_PERK_LEFT_LEVELS = new int[2]{0, 1};
	public string BURST_PERK_LEFT_DESCRIPTION = "---";
	public string BURST_PERK_LEFT_LEVELS_UNIT = "---";

	public int BURST_PERK_RIGHT_COST_CRYSTALS = 16;
	public int BURST_PERK_RIGHT_COST_BOLTS = 32;
	public int[] BURST_PERK_RIGHT_LEVELS = new int[2]{0, 1};
	public string BURST_PERK_RIGHT_DESCRIPTION = "---";
	public string BURST_PERK_RIGHT_LEVELS_UNIT = "---";

	// ITEMS
	public int ITEMS_START_LEVEL = 1;
	public int ITEMS_MAX_LEVEL = 3;
	private const int ITEMS_ARRAY_SIZE = 4;

	// Ulimited air
	public int[] UNLIMITED_AIR_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] UNLIMITED_AIR_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] UNLIMITED_AIR_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
	public string UNLIMITED_AIR_DESCRIPTION = "---";
	public string UNLIMITED_AIR_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] BOLT_MAGNET_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] BOLT_MAGNET_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] BOLT_MAGNET_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
	public string BOLT_MAGNET_DESCRIPTION = "---";
	public string BOLT_MAGNET_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] BOLT_MULTIPLIER_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] BOLT_MULTIPLIER_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] BOLT_MULTIPLIER_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
	public string BOLT_MULTIPLIER_DESCRIPTION = "---";
	public string BOLT_MULTIPLIER_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] FORCE_FIELD_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] FORCE_FIELD_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] FORCE_FIELD_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
	public string FORCE_FIELD_DESCRIPTION = "---";
	public string FORCE_FIELD_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] ROCKET_THRUST_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] ROCKET_THRUST_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] ROCKET_THRUST_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
	public string ROCKET_THRUST_DESCRIPTION = "---";
	public string ROCKET_THRUST_LEVELS_UNIT = "---";
	
	// Ulimited air
	public int[] SHOCKWAVE_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] SHOCKWAVE_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] SHOCKWAVE_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
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

	private static GlobalVariables instance = null;
	public static GlobalVariables Instance
	{
		get
		{
			if (instance == null)
			{
				_thisObject = GameObject.Find("GlobalVaribelsPrefab");
				if (_thisObject ==  null)
				{
				}

				instance = _thisObject.GetComponent<GlobalVariables>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	public int DistanceCritera (string levelName)
	{
		switch (levelName) 
		{
		case "bonus 1": case "bonus 2": case "bonus 3": 
		case "bonus 4": case "bonus 5": case "bonus 6":
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
		case "bonus 1": case "bonus 2": case "bonus 3": 
		case "bonus 4": case "bonus 5": case "bonus 6":
			return ASTEROID_BONUS_1_REWARD_BOLTS;
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
		case "bonus 1": case "bonus 2": case "bonus 3": 
		case "bonus 4": case "bonus 5": case "bonus 6":
			return ASTEROID_BONUS_1_REWARD_BOLTS;
		default:
			print("Error in BonusRewardCrystals " + levelName);
			break;
		}
		
		return 0;
	}
}
