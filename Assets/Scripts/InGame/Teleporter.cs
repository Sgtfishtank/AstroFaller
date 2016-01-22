using UnityEngine;
using System.Collections;
using System.Linq;

public class Teleporter : MonoBehaviour {

    public Transform tpin;
    public Transform tpout;
    // Use this for initialization
    void Start () {
        tpin = transform.GetComponentsInChildren<Transform>().Where(x => x.name == "Teleport_in").Select(x => x.transform).ToArray()[0];
        tpout = transform.GetComponentsInChildren<Transform>().Where(x => x.name == "Teleport__out").Select(x => x.transform).ToArray()[0];
        Transform[] temp =  tpin.GetComponentsInChildren<Transform>().Where(x => x.name == "Teleport_Lowp").Select(x => x.transform).ToArray();
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(Mathf.Abs(temp[0].position.x - temp[1].position.x), 0.75f, 0.75f);
        box.center = new Vector3(tpin.position.x, tpin.position.y, 0);
        box.isTrigger = true;
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.transform.position = tpout.transform.position + new Vector3(0, -1.5f, 0);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
