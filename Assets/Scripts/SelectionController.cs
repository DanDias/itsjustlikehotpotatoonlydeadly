using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public GameObject pointerObject;
	public GameObject enemySelectObject;

    public void SelectNewCharacter()
    {
        // Set the pointer to the CurrentCharacter
        pointerObject.transform.position = TurnManager.Instance.CurrentCharacter.Position + new Vector3(0, 1, 0);
		// Set enemy selector to current target
    }

	public void SelectNewEnemy()
	{
		// Set enemy selector to current target
		enemySelectObject.transform.position = TurnManager.Instance.CurrentCharacter.myTarget.Position + new Vector3(0, -0.9f, 0);
	}
}
