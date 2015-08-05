using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour 
{
	public GameObject[] mMenuPositions;
	public GameObject mStartMenuPosition;
	public MenuCamera mMenuCamera;

	// Use this for initialization
	void Start () 
	{
		mMenuCamera = GameObject.Find ("Menu Camera").GetComponent<MenuCamera>();

		if (mStartMenuPosition == null)
		{
			mStartMenuPosition = mMenuPositions[0];
		}

		mMenuCamera.transform.position = mStartMenuPosition.transform.position;

		transform.Find ("World Map 0x y0 230z").GetComponent<WorldMapMenu> ().Init();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0) 
		{

		}
	}
}
