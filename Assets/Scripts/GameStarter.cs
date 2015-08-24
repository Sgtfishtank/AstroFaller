using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour 
{
	public enum StartState
	{
		MainMenu,
		InGame
	}

	public StartState mStartState;
	public int mStartInGameLevel;

	private GameObject[] mEscenncials;

	void Awake()
	{
		mEscenncials = Resources.LoadAll<GameObject>("Prefab/Essential") as GameObject[];
		for (int i = 0; i < mEscenncials.Length; i++) 
		{
			if (GameObject.Find(mEscenncials[i].name) == null)
			{
				print("Error essceciall " + mEscenncials[i].name + " is missing in the scene.");
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		switch (mStartState) 
		{
		case StartState.MainMenu:
			MainGameMenu.Instance.Enable();
			WorldGen.Instance.Disable();
			break;
		case StartState.InGame:
			MainGameMenu.Instance.Disable();
			WorldGen.Instance.Enable("Level" + mStartInGameLevel);
			break;
		default:
			print("ERROR: StartState " + mStartState);
			MainGameMenu.Instance.Disable();
			WorldGen.Instance.Disable();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
	
	}
}
