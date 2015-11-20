using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	public float mStartTime;
	private Player mPlayer;
	private AstroidSpawn mAstroidSpawn;
	private Rigidbody mRb;
	
	private Vector3 mStartPos;
	private float mHeight;
	private float mSpinSpeed;

	void Awake()
	{
		mRb = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () 
	{
		mPlayer = WorldGen.Instance.Player();
		mAstroidSpawn = WorldGen.Instance.AstroidSpawn ();
	}
	
	void OnDisable()
	{
	}
	
	void OnEnable()
	{
		mStartTime = Time.time;
		mHeight = 0.5f + (Random.value * 2.5f);
		mSpinSpeed = 40 + (Random.value * 160);
		mStartPos = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		float freq = mRb.velocity.magnitude;
		float time = Time.time - mStartTime;
		float hegiht = mHeight / freq;

		Vector3 pendVel = Vector3.Cross(new Vector3(0, 0, 1), mRb.velocity);
		
		Vector3 offset = (pendVel * Mathf.Sin (time * freq) * hegiht);
		Vector3 offset2 = (pendVel * Mathf.Cos(time * freq) * freq * hegiht);

		Vector3 pos = mStartPos + (mRb.velocity * time) + offset;
		Vector3 vel = (mRb.velocity) + offset2;

		transform.LookAt(transform.position + vel);


		Vector3 rotEu = transform.localRotation.eulerAngles;

		transform.localRotation = Quaternion.Euler(rotEu.x, rotEu.y, (Time.time - mStartTime) * mSpinSpeed);
		
		Debug.DrawLine (transform.position, transform.position + (vel * Time.deltaTime), Color.green, 100);
		transform.position = pos;
	}
	
	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject != gameObject)
		{
			mAstroidSpawn.SpawnAstCollisionEffects(coll.contacts[0].point);
			mAstroidSpawn.SpawnMissileCollisionEffects(transform.position);
			mAstroidSpawn.RemoveAstroid(gameObject);
		}
	}

}
