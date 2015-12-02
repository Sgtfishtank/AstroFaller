using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameStarter : MonoBehaviour 
{
	public enum StartState
	{
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
		currInstance = InGame.Instance.gameObject;
		//currInstance = MainGameMenu.Instance.gameObject;

		currInstance = InGameCamera.Instance.gameObject;
		//currInstance = MenuCamera.Instance.gameObject;

		currInstance = GlobalVariables.Instance.gameObject;
		currInstance = PlayerData.Instance.gameObject;
		currInstance = GUICanvas.Instance.gameObject;
		currInstance = AudioManager.Instance.gameObject;

		currInstance = this.gameObject;
	}

	// Use this for initialization
	void Start () 
	{
		switch (mStartState) 
		{
		case StartState.AstroidLevel:
			WorldGen.Instance.Enable(1);
			break;
		case StartState.AlienLevel:
			WorldGen.Instance.Enable(2);
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
