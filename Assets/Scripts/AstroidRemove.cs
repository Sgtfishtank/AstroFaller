using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour {

	public int xSize;
	private GameObject mpl;
	// Use this for initialization
	void Start ()
	{
		mpl = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if(!(transform.position.x < xSize && transform.position.x > -xSize &&
		   transform.position.y >5 && transform.position.y < 25))
		{
			AstroidSpawn.RemoveAstroid(gameObject);

		}
	}
}
