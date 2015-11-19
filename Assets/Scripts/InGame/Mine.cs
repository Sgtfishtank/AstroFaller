using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour 
{
	private SphereCollider mCol;
	public Animator mAnim;
	public GameObject mAniobj;
	public ParticleSystem mDetect;
	public ParticleSystem mExplode;
	
	public float mBlowTime;
	public float blowDelay = 1;
	public SphereCollider hardColl;
	public float blowRadius = 3;

	void Awake()
	{
		mCol = GetComponent<SphereCollider> ();
		mAnim = transform.Find ("mine_anim").GetComponent<Animator> ();
		enabled = false;
		mDetect.gameObject.SetActive(true);
		mExplode.gameObject.SetActive(false);
		mBlowTime = -1;
	}

	// Use this for initialization
	void Start () 
	{

	}
	void OnEnable()
	{
		mExplode.gameObject.SetActive(false);
		mDetect.startColor = new Color (1, 1, 1, 0.1f);
		hardColl.enabled = true;
	}
	// Update is called once per frame
	void Update () 
	{

		if (mBlowTime < Time.time && mBlowTime != -1) 
		{

			Vector3 pos = InGame.Instance.Player().CenterPosition();

			if (Vector3.Distance(transform.position, pos) < blowRadius) 
			{

				InGame.Instance.Player().PlayerDamage(1);
			}

			InGame.Instance.Player().Rigidbody().AddExplosionForce(10f, transform.position, blowRadius,0f, ForceMode.Impulse);
			mDetect.gameObject.SetActive(false);
			mExplode.gameObject.SetActive(true);
			mBlowTime = -1;
			mAniobj.SetActive(false);

		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			mBlowTime = Time.time + blowDelay;
			mAnim.SetTrigger("Expand");
			enabled = true;
			mDetect.startColor = new Color(1f,0f,0.2f,0.2f);
			print(mDetect.startColor);
		}
	}
	void OnCollisionEnter(Collision coll)
	{
		if (coll.transform.tag == "Player")
		{
			Vector3 pos = InGame.Instance.Player().CenterPosition();

			if (Vector3.Distance(transform.position, pos) < blowRadius) 
			{

				InGame.Instance.Player().PlayerDamage(1);
			}
			//coll.gameObject.GetComponent<Rigidbody>().AddExplosionForce(10f,transform.position,3f,0f,ForceMode.Impulse);
			InGame.Instance.Player().Rigidbody().AddExplosionForce(10f, transform.position, blowRadius, 0f, ForceMode.Impulse);
			mDetect.gameObject.SetActive (false);
			mExplode.gameObject.SetActive (true);
			mBlowTime = -1;
			mAniobj.SetActive (false);
			hardColl.enabled = false;
		}
	}
}
