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



	// WORLD_MAP_MENU
	public float WORLD_MAP_SCROLL_OFFSET = 60;
	public float WORLD_MAP_LEVELS_SIZE = 60;
	public float WORLD_MAP_LEVELS_SNAP_SPEED = 4;
	public float WORLD_MAP_LEVELS_SCROLL_SPEED = 180;

	// LEVELS

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
	public int[] ULIMITED_AIR_COST_CRYSTALS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] ULIMITED_AIR_COST_BOLTS = new int[ITEMS_ARRAY_SIZE]{1, 2, 3, 4};
	public int[] ULIMITED_AIR_LEVELS = new int[ITEMS_ARRAY_SIZE + 1]{0, 1, 2, 3, 4};
	public string ULIMITED_AIR_DESCRIPTION = "---";
	public string ULIMITED_AIR_LEVELS_UNIT = "---";

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
		_thisObject = GameObject.Find("GlobalVaribelsPrefab");
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
				_thisObject = GameObject.Find("GlobalVaribelsPrefab");
				if (_thisObject ==  null)
				{
					//_thisObject = GameObject.Find("GlobalVaribelsPrefab");
				}

				instance = _thisObject.GetComponent<GlobalVariables>();
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
