using UnityEngine;
using System.Collections;

public class GUICanvasBase : MonoBehaviour 
{
	public ButtonPress FindButton(string path)
	{
		if (transform.Find(path) == null)
		{
			print("ERRRO: " + path);
			return null;
		}

		return transform.Find(path).GetComponent<ButtonPress>();
	}
}
