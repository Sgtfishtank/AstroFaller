using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeathmenuButtons : MonoBehaviour
{
	int boxes;
	RectTransform[] t;
	// Use this for initialization
	void Start ()
	{
	
	}
	void OnEnable()
	{
		if(InGame.Instance.Player() == null)
		{
			return;
		}
		boxes = InGame.Instance.Player().CollectedPerfectDistances();
		if(t == null)
			t = gameObject.GetComponentsInChildren<RectTransform>();
		if(boxes < 5)
		{
			for (int i = 1+boxes; i < t.Length; i++)
			{
				t[i].gameObject.SetActive(false);
			}
		}
		else
		{
			for (int i = 2; i < t.Length-1; i++)
			{
				t[i].gameObject.SetActive(false);
			}
		}
		switch (boxes)
		{
		case 1:
			t[1].anchoredPosition = new Vector2(0,-97);
			break;
		case 2:
			t[1].anchoredPosition = new Vector2(-66.7f,-97);
			t[2].anchoredPosition = new Vector2(63.7f,-97);
			break;
		case 3:
			t[1].anchoredPosition = new Vector2(-95.7f,-97);
			t[2].anchoredPosition = new Vector2(1.1f,-97);
			t[3].anchoredPosition = new Vector2(97.1f,-97);
			break;
		case 4:
			t[1].anchoredPosition = new Vector2(-114.1f,-97);
			t[2].anchoredPosition = new Vector2(-36.1f,-97);
			t[3].anchoredPosition = new Vector2(41,-97);
			t[4].anchoredPosition = new Vector2(116.2f,-97);
			break;
		default:
			break;
		}
		if(boxes>4)
		{
			t[1].anchoredPosition = new Vector2(-31.4f,-97);
			t[5].gameObject.SetActive(true);
			t[5].gameObject.GetComponent<Text>().text = "x" + boxes;
		}
	}
	void OnDisable()
	{
		for (int i = 1; i < t.Length; i++)
		{
			 t[i].gameObject.SetActive(true);
		}
		t = null;
	}
	public void disableSpecific(int value)
	{
		t[value].gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
