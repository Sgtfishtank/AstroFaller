using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class LaserLine : MonoBehaviour {
    public Material[] emisionmats;
    public GameObject electricity;
    public GameObject[] electricityBallEffect;
    public GameObject colliderHolder;
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
        colliderHolder = temp1.Where(x => x.name == "Colliders").Select(x => x.gameObject).ToArray()[0];
        colliders = new BoxCollider[electricityBallEffect.Length];
        List<GameObject> temp = electricity.GetComponent<DigitalRuby.ThunderAndLightning.LightningBoltPathScript>().LightningPath.List;
        for (int i = 0; i < temp.Count-1; i++)
        {
            colliders[i] = colliderHolder.AddComponent<BoxCollider>();
            

        }
        for (int i = 0; i < colliders.Length-1; i++)
        {
            calculateCollider(temp[i], temp[i + 1], colliders[i]);
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
                lastBurstTime = delayTime + Random.Range(0, randomDelay) + Time.time;
                localTime = 0;
                count = 0;
            }
            if (count == 3)
            {
                idelActive = burstTime + Random.Range(0, randomBurstTime)+Time.time;
                electricity.SetActive(true);
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
        box.size = new Vector3(1, len, 1);
        print(xdiff / ydiff);
        print(Mathf.Tan(xdiff / ydiff));
        colliderHolder.transform.rotation = Quaternion.Euler(0, 0, xdiff / ydiff);
    }
}
