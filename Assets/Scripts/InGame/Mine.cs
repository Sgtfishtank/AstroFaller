using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour 
{
	private SphereCollider mCol;
	public Animator mAnim;
	
	private float mBlowTime;

	void Awake()
	{
		mCol = GetComponent<SphereCollider> ();
		mAnim = transform.Find ("mine_anim").GetComponent<Animator> ();;
		enabled = false;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float blowRadius = 2;
		if (mBlowTime < Time.time) 
		{

			Vector3 pos = InGame.Instance.Player().CenterPosition();

			if (Vector3.Distance(transform.position, pos) < blowRadius) 
			{
				InGame.Instance.Player().PlayerDamage(1);
			}
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		float blowDelay = 2;
		if (col.tag == "Player") 
		{
			mBlowTime = Time.time + blowDelay;
			mAnim.SetTrigger("Expand");
			enabled = true;
		}
	}
	
	/*void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			enabled = false;
			mAnim.Play("Close");
		}
	}*/
}
