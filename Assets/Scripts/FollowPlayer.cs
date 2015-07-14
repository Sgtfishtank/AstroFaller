using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	// Use this for initialization
	GameObject mplayer;
	void Start ()
	{
		mplayer = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = mplayer.transform.position;
		pos.z += -20;
		transform.position = pos;
	}
}
