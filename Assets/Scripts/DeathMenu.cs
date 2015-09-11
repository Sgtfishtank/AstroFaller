using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
	TextMesh[] mTexts;
	Player mPlayer;
	// Use this for initialization
	void Start()
	{

	}
	void OnEnable ()
	{
		mTexts = gameObject.GetComponentsInChildren<TextMesh> ();
		mPlayer = InGame.Instance.mPlayer;

		int multi = calculateMultiplier ();

		print (mPlayer.distance ());
		print (multi.ToString());
		print (mPlayer.colectedBolts ());
		print (multi * mPlayer.colectedBolts ());

		for (int i = 0; i < mTexts.Length; i++)
		{
			if (mTexts[i].gameObject.name == "distance total")
				mTexts[i].text =  mPlayer.distance ().ToString();
			else if (mTexts[i].gameObject.name == "bolts gathered")
				mTexts[i].text = mPlayer.colectedBolts ().ToString();
			else if (mTexts[i].gameObject.name == "multiplier number")
				mTexts[i].text = multi.ToString();
			else if (mTexts[i].gameObject.name == "bolts total")
				mTexts[i].text =(multi * mPlayer.colectedBolts ()).ToString();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	int calculateMultiplier()
	{
		return 1;
	}
}
