using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour 
{
	public GameObject OptionsPanel;

	// Use this for initialization
	void Start () {
		
	}

	public void Close()
	{
		OptionsPanel.SetActive(false);
	}

	public void Open()
	{
		OptionsPanel.SetActive(true);
	}
}
