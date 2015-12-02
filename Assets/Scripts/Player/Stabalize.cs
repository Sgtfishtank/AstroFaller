using UnityEngine;
using System.Collections;

public class Stabalize : MonoBehaviour {
	public float temp1;
	public float temp2;
	public float temp3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.localPosition = Vector3.zero;
		transform.rotation= Quaternion.identity;
		Vector3 v = InGame.Instance.Player().CenterPosition();
		v.z += temp1;
		v.y += temp2;
		v.x += temp3;
		transform.position = v;
	}
}
