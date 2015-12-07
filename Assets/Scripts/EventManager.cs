using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
				instance = Singleton<EventManager>.CreateInstance("Prefab/Essential/EventSystem");
				GameObject.DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    void Awake()
	{
    }

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
