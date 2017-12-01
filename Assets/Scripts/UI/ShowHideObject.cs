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
	public void ShowHide(string msg)
	{
		obj.GetComponentInChildren<Text>().text = msg;
		obj.SetActive(!obj.activeSelf);
	}
}
