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



	// Use this for initialization
	void Start ()
	{
		_thisObject = gameObject;
		instance = _thisObject.GetComponent<GlobalVariables>();
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
				instance = _thisObject.GetComponent<GlobalVariables>();
			}
			return instance;
		}
	}

	public int BoltsCritera (string levelName)
	{
		switch (levelName) 
		{
		case "Bonus 1":
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
		case "Bonus 1":
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
		case "Bonus 1":
			return ASTEROID_BONUS_1_REWARD_BOLTS;
		default:
			print("Error in DistanceCritera " + levelName);
			break;
		}
		
		return 0;
	}
}
