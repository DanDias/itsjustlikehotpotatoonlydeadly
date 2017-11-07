using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public GameObject pointerObject;

    public void SelectNewCharacter()
    {
        // Set the pointer to the CurrentCharacter
        pointerObject.transform.position = TurnManager.Instance.CurrentCharacter.Position + new Vector3(0, 1, 0);
    }
}
