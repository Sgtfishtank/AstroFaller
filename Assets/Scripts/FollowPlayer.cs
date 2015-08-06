using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	// Use this for initialization
	public Transform mplayer;
	public float dist;
	public int ydist;
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = transform.position;
		pos.z = -dist;
		pos.y = mplayer.position.y-ydist;
		transform.position = pos;
	}
}
