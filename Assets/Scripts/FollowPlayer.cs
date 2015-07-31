using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	// Use this for initialization
	GameObject mplayer;
	public float dist;
	public int ydist;
	void Start ()
	{
		mplayer = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = transform.position;
		pos.z = -dist;
		pos.y = mplayer.transform.position.y-ydist;
		transform.position = pos;
	}
}
