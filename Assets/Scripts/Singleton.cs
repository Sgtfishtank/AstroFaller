using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Singleton<T>
{
	private static bool creating = false;

    public static T CreateInstance(string path)
	{
		if (creating) 
		{
			throw new NullReferenceException("ERROR: Circular Singleton Creation Call!");
		}
		
		creating = true;

        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject thisObject = GameObject.Find(prefab.name);
        if (thisObject == null)
		{
            thisObject = GameObject.Instantiate<GameObject>(prefab);
			thisObject.name = prefab.name;
		}

		creating = false;

        return thisObject.GetComponent<T>();
    }
}
