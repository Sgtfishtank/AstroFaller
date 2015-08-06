using UnityEngine;
using System.Collections;

public class MainGameMenu : MonoBehaviour 
{
	public GameMenu mStartMenu;

	private GameMenu[] mGameMenus;
	private MenuCamera mMenuCamera;
	private PopupBuyMenu mPopupBuyMenu;

	// Use this for initialization
	void Start () 
	{
		mPopupBuyMenu = GameObject.Find("Pop-up buy menu").GetComponent<PopupBuyMenu>();
		mPopupBuyMenu.Init ();

		mMenuCamera = GameObject.Find ("Menu Camera").GetComponent<MenuCamera>();

		mGameMenus = GetComponentsInChildren<GameMenu> ();
		if (mStartMenu == null)
		{
			mStartMenu = mGameMenus[0];
		}

		mMenuCamera.transform.position = mStartMenu.transform.position + mMenuCamera.mCameraOffset;

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Init();
		}

		mStartMenu.Focus ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0) 
		{

		}
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			ChangeGameMenu(0);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			ChangeGameMenu(1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			ChangeGameMenu(2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			ChangeGameMenu(3);
		}
	}

	void ChangeGameMenu (int index)
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Unfocus();
		}

		mMenuCamera.StartMove (mGameMenus [index].gameObject);
		mGameMenus[index].Focus();
	}
}
