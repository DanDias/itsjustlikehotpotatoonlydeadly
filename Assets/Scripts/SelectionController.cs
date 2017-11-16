using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public GameObject pointerObject;
	public GameObject enemySelectObject;

    public void Awake()
    {
        TurnManager.Instance.OnChangeTurn.AddListener(SelectNewCharacter);
        TurnManager.Instance.OnEnemySelect.AddListener(SelectNewEnemy);
    }

    public void SelectNewCharacter(Character ch)
    {
        // Set the pointer to the CurrentCharacter
        pointerObject.transform.position = ch.Position + new Vector3(0, 1, 0);
		// Set enemy selector to current target
    }

	public void SelectNewEnemy(Character ch)
	{
        if (ch == null)
            enemySelectObject.SetActive(false);
        else
        {
            enemySelectObject.SetActive(true);
            // Set enemy selector to target
            enemySelectObject.transform.position = ch.Position + new Vector3(0, -0.9f, 0);
        }
	}
}
