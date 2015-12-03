using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuStarter : MonoBehaviour 
{
    public MainGameMenu.State mStartState;

    public GameObject[] mEscenncialPrefabs;
    public GameObject[] mInGamePrefabs;

	void Awake()
    {
        GameObject currInstance;

        currInstance = AudioManager.Instance.gameObject;
        currInstance = GlobalVariables.Instance.gameObject;
        currInstance = PlayerData.Instance.gameObject;
        currInstance = EventManager.Instance.gameObject;

        // triger static instance init
        currInstance = GUICanvasMenu.Instance.gameObject;
        currInstance = MenuCamera.Instance.gameObject;
        currInstance = MainGameMenu.Instance.gameObject;
	}

	// Use this for initialization
	void Start ()
    {
        if (PlayerData.Instance.StateToLoad == MainGameMenu.State.ERROR)
        {
            if (mStartState == MainGameMenu.State.ERROR)
            {
                MainGameMenu.Instance.Enable(MainGameMenu.State.DEFAULT);
            }
            else
            {
                MainGameMenu.Instance.Enable(mStartState);
            }
        }
        else
        {
            MainGameMenu.Instance.Enable(PlayerData.Instance.StateToLoad);
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
