using UnityEngine;
using System.Collections;

public abstract class SpawnerBase : MonoBehaviour
{
	protected bool SpawnObjs;

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
