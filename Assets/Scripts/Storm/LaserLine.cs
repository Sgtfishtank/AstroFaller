using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class LaserLine : MonoBehaviour {
    public Material[] emisionmats;
    public GameObject electricity;
    public GameObject[] electricityBallEffect;
    public BoxCollider[] colliders;
    public float delayTime;
    public float randomDelay;
    public float burstTime;
    public float randomBurstTime;
    float lastBurstTime;
    float localTime;
    public float scale;
    public Color baseColor;
    
    float idelActive;

    public int count;
    bool increase = true;



    // Use this for initialization
    void Start () {
        baseColor = new Color(0.3529412f, 0.8125762f, 1);
        Transform[] temp1 = transform.GetComponentsInChildren<Transform>();
        electricityBallEffect = temp1.Where(x => x.name == "cloud_effect").Select(x => x.gameObject).ToArray();
        electricity = temp1.Where(x => x.name == "Electricity").Select(x => x.gameObject).ToArray()[0];
        GameObject[] a = temp1.Where(x => x.name == "cloud_lp:cloud_lp").Select(x => x.gameObject).ToArray();
        emisionmats = new Material[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            emisionmats[i]= a[i].GetComponent<Renderer>().material;
            emisionmats[i].SetColor("_EmissionColor", baseColor*0);
        }
        electricity.SetActive(false);
        for (int i = 0; i < electricityBallEffect.Length; i++)
        {
            electricityBallEffect[i].SetActive(false);
        }
        colliders = new BoxCollider[electricityBallEffect.Length-1];
        List<GameObject> temp = electricity.GetComponent<DigitalRuby.ThunderAndLightning.LightningBoltPathScript>().LightningPath.List;
        for (int i = 0; i < temp.Count-1; i++)
        {
            colliders[i] = new GameObject("Collider").AddComponent<BoxCollider>();
            colliders[i].transform.parent = transform;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            calculateCollider(temp[i], temp[i + 1], colliders[i]);
            colliders[i].transform.position = (temp[i].transform.position + temp[i + 1].transform.position) / 2;
            colliders[i].gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(lastBurstTime < Time.time)
        {
            localTime += Time.deltaTime;
            if(idelActive > Time.time)
            {
                return;
            }
            if (idelActive < Time.time && count == 4)
            {
                for (int i = 0; i < emisionmats.Length; i++)
                {
                    emisionmats[i].SetColor("_EmissionColor", baseColor*0);
                }
                electricity.SetActive(false);
                for (int i = 0; i < electricityBallEffect.Length; i++)
                {
                    electricityBallEffect[i].SetActive(false);
                }
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].gameObject.SetActive(false);
                }
                lastBurstTime = delayTime + Random.Range(0, randomDelay) + Time.time;
                localTime = 0;
                count = 0;
            }
            if (count == 3)
            {
                idelActive = burstTime + Random.Range(0, randomBurstTime)+Time.time;
                electricity.SetActive(true);
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].gameObject.SetActive(true);
                }
                for (int i = 0; i < electricityBallEffect.Length; i++)
                {
                    electricityBallEffect[i].SetActive(true);
                }
                count = 4;
            }
            
            float emission = Mathf.PingPong(localTime * scale, 1.0f);

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            for (int i = 0; i < emisionmats.Length; i++)
            {
                emisionmats[i].SetColor("_EmissionColor", finalColor);
            }
            if (emission >= 0.902f && increase)
            {
                count++;
                increase = false;
            }
            if(emission < 0.5f)
            {
                increase = true;
            }

        }
	}
    void calculateCollider(GameObject ball1, GameObject ball2, BoxCollider box)
    {
        float xdiff = Mathf.Abs(ball1.transform.position.x - ball2.transform.position.x);
        float ydiff = Mathf.Abs(ball1.transform.position.y - ball2.transform.position.y);
        float len = Mathf.Sqrt(Mathf.Pow(xdiff, 2) + Mathf.Pow(ydiff, 2));
        box.size = new Vector3(len, 0.75f, 1);
        if(ball1.transform.position.y > ball2.transform.position.y)
            box.gameObject.transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, ball1.transform.position - ball2.transform.position));
        else
            box.gameObject.transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.right, ball1.transform.position - ball2.transform.position));
    }
}
