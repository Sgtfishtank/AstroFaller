using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour 
{
    public float mAttrractRadius;
    public float mAttrractForce;

    private Player mPlayer;
    private float mScale;

    void Awake()
    {
        mScale = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

	// Use this for initialization
	void Start () 
    {
        mPlayer = InGame.Instance.Player();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 dir = (transform.position - mPlayer.CenterPosition());
        float len = dir.magnitude;
        if (len < (mAttrractRadius * mScale))
        {
            float force = mAttrractForce * mPlayer.Rigidbody().mass / (len * len);
            mPlayer.Rigidbody().AddForce(dir.normalized * force);
        }
	}
}
