using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour {

	public int xSize;
	private GameObject mpl;
	public AstroidSpawn a;
	// Use this for initialization
	void Start ()
	{
		mpl = WorldGen.Instance.mPlayer;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if(!(transform.position.x < xSize && transform.position.x > -xSize &&
		     transform.position.y < mpl.transform.position.y + 5 && transform.position.y > mpl.transform.position.y - 25))
		{
			a.RemoveAstroid(gameObject);

		}
	}
}
