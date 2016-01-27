using UnityEngine;
using System.Collections;

public class ElektricJellyFish : MonoBehaviour 
{
    public float mSpeed;
    public float mFreqency;
    public float mRotSpeed;

    private float mStartTime;

	// Use this for initialization
	void Start () 
    {
	}

    void OnDisable()
    {
    }

    void OnEnable()
    {
        mStartTime = Time.time + (Random.Range(-1.0f, 1.0f) / (mFreqency * Mathf.PI * 2));
    }

	// Update is called once per frame
	void Update () 
    {
        transform.position += new Vector3(0, mSpeed * ((Mathf.Sin((Time.time - mStartTime + Random.Range(-0.03f, 0.03f)) * Mathf.PI * 2 * mFreqency) + 0.6f) / 2f), 0) * Time.deltaTime;
        float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * mRotSpeed * angle);
    }
}
