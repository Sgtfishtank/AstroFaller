using UnityEngine;
using System.Collections;

public class PerksMenu : MonoBehaviour 
{
	private Perk[] mPerks;

	// Use this for initialization
	void Start () 
	{
	}

	public void Init() 
	{
		mPerks = GetComponentsInChildren<Perk> ();
		
		for (int i = 0; i < mPerks.Length; i++) 
		{
			mPerks[i].Init();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.Keypad7))
		{
			mPerks[0].UnlockPart(Perk.PerkPart.Left);
		}
		else if (Input.GetKey(KeyCode.Keypad8))
		{
			mPerks[0].UnlockPart(Perk.PerkPart.Main);
		}
		else if (Input.GetKey(KeyCode.Keypad9))
		{
			mPerks[0].UnlockPart(Perk.PerkPart.Right);
		}

		if (Input.GetKey(KeyCode.Keypad4))
		{
			mPerks[1].UnlockPart(Perk.PerkPart.Left);
		}
		else if (Input.GetKey(KeyCode.Keypad5))
		{
			mPerks[1].UnlockPart(Perk.PerkPart.Main);
		}
		else if (Input.GetKey(KeyCode.Keypad6))
		{
			mPerks[1].UnlockPart(Perk.PerkPart.Right);
		}

		if (Input.GetKey(KeyCode.Keypad1))
		{
			mPerks[2].UnlockPart(Perk.PerkPart.Left);
		}
		else if (Input.GetKey(KeyCode.Keypad2))
		{
			mPerks[2].UnlockPart(Perk.PerkPart.Main);
		}
		else if (Input.GetKey(KeyCode.Keypad3))
		{
			mPerks[2].UnlockPart(Perk.PerkPart.Right);
		}
	}
}
