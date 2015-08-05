using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour {

	public int xSize;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if(!(transform.position.x < xSize || transform.position.x > -xSize))
		{
			AstroidSpawn.RemoveAstroid(gameObject);
		}
	}
}
