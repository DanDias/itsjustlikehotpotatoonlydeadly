using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public GameObject pointerObject;

    public void SelectNewCharacter()
    {
        SpriteController sc = GetComponent<SpriteController>();
        // Set the pointer to the CurrentCharacter
        pointerObject.transform.position = sc.CharacterToPositionMap[
            TurnManager.Instance.CurrentCharacter] + new Vector3(0, 1, 0);
    }
}
