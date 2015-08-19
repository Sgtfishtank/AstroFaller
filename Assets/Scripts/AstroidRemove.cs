using UnityEngine;
using System.Collections;

public class AstroidRemove : MonoBehaviour {

	public int xSize;
	private Player mpl;
	private AstroidSpawn mAstroidSpawn;

	// Use this for initialization
	void Start ()
	{
		mpl = WorldGen.Instance.Player();
		mAstroidSpawn = WorldGen.Instance.AstroidSpawn ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (OutOfBound())
		{
			mAstroidSpawn.RemoveAstroid(gameObject);
		}
	}

	public bool OutOfBound()
	{
		return (!(transform.position.x < GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE && 
		            transform.position.x > -GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE &&
		            transform.position.y < (mpl.transform.position.y + 5) && 
		            transform.position.y > (mpl.transform.position.y - 25)));
	}
}
