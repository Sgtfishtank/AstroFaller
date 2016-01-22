using UnityEngine;
using System.Collections;

public class WorldMapGUI : GUICanvasBase
{
    private ButtonPress mPlayLevelButton;

    void Awake()
    {
        mPlayLevelButton = transform.Find("PlayLevelButton").GetComponent<ButtonPress>();
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void ShowPlayLevelButton(bool show)
    {
        mPlayLevelButton.gameObject.SetActive(show);
    }

    /*public ButtonPress PlayButton()
    {
        return mPlayLevelButton.GetComponent<ButtonPress>();
    }*/

    // PlayLevel
    public void PlayLevel()
    {
        MainGameMenu.Instance.WorldMapMenu().PlayLevel();
    }
}
