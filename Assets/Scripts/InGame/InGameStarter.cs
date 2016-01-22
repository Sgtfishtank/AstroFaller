using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameStarter : MonoBehaviour 
{
    public InGame.Level mStartLevel;

	void Awake()
	{
		// triger static instance init
		AudioManager.Instance.enabled = true;
		GlobalVariables.Instance.enabled = true;
		PlayerData.Instance.enabled = true;
        EventManager.Instance.enabled = true;

		InGameGUICanvas.Instance.enabled = true;
		InGameCamera.Instance.enabled = true;
		InGame.Instance.enabled = true;
	}

	// Use this for initialization
	void Start ()
    {
        if (PlayerData.Instance.LevelToLoad == InGame.Level.ERROR)
        {
            if (mStartLevel == InGame.Level.ERROR)
            {
                WorldGen.Instance.Enable(InGame.Level.DEFAULT);
            }
            else
            {
                WorldGen.Instance.Enable(mStartLevel);
            }
        }
        else
        {
            WorldGen.Instance.Enable(PlayerData.Instance.LevelToLoad);
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
