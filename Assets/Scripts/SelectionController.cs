using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public GameObject pointerObject;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SelectNewCharacter()
    {
        // Set the pointer to the CurrentCharacter
        pointerObject.transform.position = GetComponent<SpriteController>().CharacterToPositionMap[
            TurnManager.Instance.CurrentCharacter] + new Vector3(0, 1, 0);
    }
}
