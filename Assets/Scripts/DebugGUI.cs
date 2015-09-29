using UnityEngine;
using System.Collections;

public class DebugGUI : MonoBehaviour 
{
	public int mFpsUpdateFrames;

	private bool mShowDebugGUI;
	private int mDebugGUISizeY;

	private int mF;
	private float mFT;
	private float mFps;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mShowDebugGUI)
		{
			Player pl = InGame.Instance.Player();
			
			if (Input.GetKeyDown(KeyCode.I))
			{
				pl.mInvulnerable = !pl.mInvulnerable;
			}
			
			if (Input.GetKeyDown(KeyCode.D))
			{
				pl.Dash();
			}
			
			if (Input.GetKeyDown(KeyCode.Plus))
			{
				pl.PlayerHeal(1);
			}
			
			if (Input.GetKeyDown(KeyCode.Minus))
			{
				pl.PlayerDamage(1);
			}
			
			if (Input.GetKeyDown(KeyCode.O))
			{
				pl.mUseAirReg = !pl.mUseAirReg;
			}
			
			if (Input.GetKeyDown(KeyCode.P))
			{
				pl.mUseAirDrain = !pl.mUseAirDrain;
			}
			
			if (Input.GetKeyDown(KeyCode.B))
			{
				GUICanvas.Instance.ToggleShowButtons();
			}
			
			if (Input.GetKeyDown(KeyCode.K))
			{
				PlayerData.Instance.depositBolts(999);
			}
			
			if (Input.GetKeyDown(KeyCode.L))
			{
				PlayerData.Instance.withdrawBolts(PlayerData.Instance.bolts());
			}
		}

		Input.multiTouchEnabled = true;
		if (((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.LeftShift))) || (Input.touchCount >= 3))
		{
			mShowDebugGUI = !mShowDebugGUI;
			
			if (mShowDebugGUI == false)
			{
				GUICanvas.Instance.ShowButtons(false);
				InGame.Instance.Player().mInvulnerable = false;
			}
		}

		mF++;
		if (mF >= mFpsUpdateFrames)
		{
			mFps = mF / (Time.time -  mFT);
			mFT = Time.time;
			mF -= mFpsUpdateFrames;
		}
	}
	
	void OnGUI()
	{
		if (!mShowDebugGUI)
		{
			return;
		}
		
		int startX = 10;
		int startY = 10;
		int size = 20;
		
		GUI.Box(new Rect(10, 10, 200, mDebugGUISizeY - startY), "Debug Window");
		startX += 10;
		startY += size;
		
		if (InGame.Instance.gameObject.activeSelf) 
		{
			GUI.Label (new Rect (startX, startY, 180, size), "Level: " + InGame.Instance.CurrentLevel ());
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Time on level: " + WorldGen.Instance.LevelRunTime ());
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Bolts on level: " + WorldGen.Instance.Player ().colectedBolts ());
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Cyrstals on level: " + WorldGen.Instance.Player ().colectedCrystals ());
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Distance on level: " + WorldGen.Instance.Player ().distance ());
			startY += size;
		}
		
		GUI.Label (new Rect (startX, startY, 180, size), "Bolts: " + PlayerData.Instance.bolts());
		startY += size;
		
		GUI.Label (new Rect(startX, startY, 180, size), "Cyrstals: " + PlayerData.Instance.crystals());
		startY += size;
		
		GUI.Label (new Rect(startX, startY, 180, size), "Bolts Total: " + PlayerData.Instance.totalBolts());
		startY += size;
		
		GUI.Label (new Rect(startX, startY, 180, size), "Cyrstals Total: " + PlayerData.Instance.totalCrystals());
		startY += size;
		
		GUI.Label (new Rect(startX, startY, 180, size), "Distance total: " + PlayerData.Instance.totalDistance());
		startY += size;
		
		if (InGame.Instance.Player().gameObject.activeInHierarchy) 
		{
			GUI.Label (new Rect (startX, startY, 180, size), "Player Air: " + InGame.Instance.Player ().airAmount ());
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player HP: " + InGame.Instance.Player ().mLife);
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player Invurable: " + InGame.Instance.Player ().mInvulnerable);
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player Reg Air: " + InGame.Instance.Player ().mUseAirReg);
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player Drain Air: " + InGame.Instance.Player ().mUseAirDrain);
			startY += size;
		}

		GUI.Label (new Rect(startX, startY, 180, size), "FPS: " + (int)mFps);
		startY += size;
		
		mDebugGUISizeY = startY;
	}
}
