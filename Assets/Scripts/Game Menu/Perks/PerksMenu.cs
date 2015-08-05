using UnityEngine;
using System.Collections;

public class PerksMenu : MonoBehaviour 
{
	Perk[] mPerks;

	// Use this for initialization
	void Start () 
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
	
	}
}
