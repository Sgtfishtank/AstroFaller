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
	public float PLAYER_VERTICAL_SPEED_FALLOF			 = 0.1f ;
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

	// TUTORIAL

	// astreoid level
	public int ASTEROID_BONUS_1_CRITERA_DISTANCE = 1000;
	public int ASTEROID_BONUS_1_REWARD_BOLTS = 1000;

	// cosmic storm level
	public int COSMIC_LEVEL_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_1_CRITERA_DISTANCE = 1000;
	public int COSMIC_BONUS_1_REWARD_BOLTS = 1000;


	// PERKS

	// Air Perk
	public int AIR_PERK_MAIN_COST_CRYSTALS = 1;
	public int AIR_PERK_MAIN_COST_BOLTS = 2;
	public int AIR_PERK_LEFT_COST_CRYSTALS = 4;
	public int AIR_PERK_LEFT_COST_BOLTS = 8;
	public int AIR_PERK_RIGHT_COST_CRYSTALS = 16;
	public int AIR_PERK_RIGHT_COST_BOLTS = 32;
	
	// Life Perk
	public int LIFE_PERK_MAIN_COST_CRYSTALS = 1;
	public int LIFE_PERK_MAIN_COST_BOLTS = 2;
	public int LIFE_PERK_LEFT_COST_CRYSTALS = 4;
	public int LIFE_PERK_LEFT_COST_BOLTS = 8;
	public int LIFE_PERK_RIGHT_COST_CRYSTALS = 16;
	public int LIFE_PERK_RIGHT_COST_BOLTS = 32;
	
	// Burst Perk
	public int BURST_PERK_MAIN_COST_CRYSTALS = 1;
	public int BURST_PERK_MAIN_COST_BOLTS = 2;
	public int BURST_PERK_LEFT_COST_CRYSTALS = 4;
	public int BURST_PERK_LEFT_COST_BOLTS = 8;
	public int BURST_PERK_RIGHT_COST_CRYSTALS = 16;
	public int BURST_PERK_RIGHT_COST_BOLTS = 32;


	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
		_thisObject = GameObject.Find("GlobalVaribels");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private static GlobalVariables instance = null;
	public static GlobalVariables Instance
	{
		get
		{
			if (instance == null)
			{

				if (_thisObject ==  null)
				{
					return new GlobalVariables();
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
