using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour 
{
	// sigleton
	private static GameObject _thisObject;
	//public GameObject THISOBJECT;

	// GLOBAL VARIABLES START HERE
	public int ASTEROID_BONUS_1_CRITERA_DISTANCE = 1000;
	public int ASTEROID_BONUS_1_CRITERA_BOLTS = 0;
	public int ASTEROID_BONUS_1_REWARD_BOLTS = 1000;

	// WORLD_MAP_MENU
	public float WORLD_MAP_SCROLL_OFFSET = 60;
	public float WORLD_MAP_LEVELS_SIZE = 60;
	public float WORLD_MAP_LEVELS_SNAP_SPEED = 4;
	public float WORLD_MAP_LEVELS_SCROLL_SPEED = 180;


	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
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
				_thisObject = GameObject.Find("GlobalVaribels");
				if (_thisObject ==  null)
				{
					return new GlobalVariables();
				}

				instance = _thisObject.GetComponent<GlobalVariables>();
			}
			return instance;
		}
	}

	public int BoltsCritera (string levelName)
	{
		switch (levelName) 
		{
		case "bonus 1": case "bonus 2": case "bonus 3": 
		case "bonus 4": case "bonus 5": case "bonus 6":
			return ASTEROID_BONUS_1_CRITERA_BOLTS;
		default:
			print("Error in BoltsCritera " + levelName);
			break;
		}

		return 0;
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
