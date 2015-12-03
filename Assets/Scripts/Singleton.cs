using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Singleton<T>
{
    public static T CreateInstance(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject thisObject = GameObject.Find(prefab.name);
        if (thisObject == null)
        {
            thisObject = GameObject.Instantiate<GameObject>(prefab);
            thisObject.name = prefab.name;
        }
        return thisObject.GetComponent<T>();
    }
}
