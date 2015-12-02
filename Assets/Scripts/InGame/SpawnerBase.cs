using UnityEngine;
using System.Collections;

public abstract class SpawnerBase : MonoBehaviour
{
	protected bool SpawnObjs;

	public abstract void SpawnCollisionEffects (Vector3 position);

	public bool OutOfBound(GameObject spawninst)
	{
		Vector3 pos = spawninst.transform.position;
		float xMax = GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * 1.5f;
		float xMin = -GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * 1.5f;
		float yMax = InGame.Instance.Player().transform.position.y + 25;
		float yMin = InGame.Instance.Player().transform.position.y - 50;
		
		return ((pos.x > xMax) || (pos.x < xMin) || (pos.y > yMax) || (pos.y < yMin));
	}
	public void StopSpawning ()
	{
		SpawnObjs = false;
	}
	
	public void StartSpawning ()
	{
		SpawnObjs = true;
	}

	public abstract void SpawnObject ();
	public abstract void LoadObjects();
	public abstract void UnloadObjects();
	public abstract void ShiftBack (float shift);
	public abstract void Reset ();
}
