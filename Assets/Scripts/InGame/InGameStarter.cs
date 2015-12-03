using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameStarter : MonoBehaviour 
{
    public InGame.Level mStartLevel;

	void Awake()
	{
        GameObject currInstance;
        currInstance = AudioManager.Instance.gameObject;
        currInstance = GlobalVariables.Instance.gameObject;
        currInstance = PlayerData.Instance.gameObject;
        currInstance = EventManager.Instance.gameObject;

        // triger static instance init
        currInstance = InGameGUICanvas.Instance.gameObject;
        currInstance = InGameCamera.Instance.gameObject;
		currInstance = InGame.Instance.gameObject;

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
