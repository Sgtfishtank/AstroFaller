using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameStarter : MonoBehaviour 
{
    public InGame.Level mStartLevel;

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

		currInstance = InGameCamera.Instance.gameObject;

		currInstance = GlobalVariables.Instance.gameObject;
		currInstance = PlayerData.Instance.gameObject;
		currInstance = GUICanvasInGame.Instance.gameObject;
		currInstance = AudioManager.Instance.gameObject;

		currInstance = this.gameObject;
	}

	// Use this for initialization
	void Start ()
    {
        PlayerData.Instance.LevelToLoad = mStartLevel;
        WorldGen.Instance.Enable();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
