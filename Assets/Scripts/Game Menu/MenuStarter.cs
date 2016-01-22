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
		// triger static instance init
		AudioManager.Instance.enabled = true;
		GlobalVariables.Instance.enabled = true;
		PlayerData.Instance.enabled = true;
		EventManager.Instance.enabled = true;
		
		MenuGUICanvas.Instance.enabled = true;
		MenuCamera.Instance.enabled = true;
		MainGameMenu.Instance.enabled = true;
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
