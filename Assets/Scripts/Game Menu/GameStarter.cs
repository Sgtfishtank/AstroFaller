using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour 
{
	public enum StartState
	{
		WorldMap,
		Items,
		Perks,
		CrystalShop,
		AstroidLevel,
		AlienLevel,
	}

	public StartState mStartState;

	private GameObject[] mEscenncials;
	public GameObject currInstance;

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

		// triger static instance init
		//currInstance = InGame.Instance.gameObject;
		currInstance = MainGameMenu.Instance.gameObject;

		//currInstance = InGameCamera.Instance.gameObject;
		currInstance = MenuCamera.Instance.gameObject;

		currInstance = GlobalVariables.Instance.gameObject;
		currInstance = PlayerData.Instance.gameObject;
		currInstance = GUICanvasMenu.Instance.gameObject;
		currInstance = AudioManager.Instance.gameObject;

		currInstance = this.gameObject;
	}

	// Use this for initialization
	void Start () 
	{
		switch (mStartState) 
		{
            case StartState.WorldMap:
			MainGameMenu.Instance.Enable(0);
			break;
		case StartState.Perks:
			
			MainGameMenu.Instance.Enable(1);
			break;
		case StartState.Items:
			
			MainGameMenu.Instance.Enable(2);
			break;
		case StartState.CrystalShop:
			
			MainGameMenu.Instance.Enable(3);
			break;
		default:
			print("ERROR: StartState " + mStartState);
			MainGameMenu.Instance.Enable(0);
			WorldGen.Instance.Disable();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
