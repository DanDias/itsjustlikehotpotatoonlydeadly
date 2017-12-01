using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideObject : MonoBehaviour {

	public GameObject obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void ShowHide(string msg, Vector3 pos)
	{
		obj.GetComponentInChildren<Text>().text = msg;
		obj.transform.position = pos + new Vector3(234, 0, 0);
		obj.SetActive(!obj.activeSelf);
	}

	public void ShowHide(string msg)
	{
		ShowHide(msg, obj.transform.position);
	}
}
