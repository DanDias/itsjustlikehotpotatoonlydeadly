using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public GameObject pointerObject;
	public GameObject enemySelectObject;

	Text charName;
	GameObject charImage;

    public void Start()
    {
        TurnManager.Instance.OnChangeTurn.AddListener(SelectNewCharacter);
        TurnManager.Instance.OnEnemySelect.AddListener(SelectNewEnemy);
		charName = GameObject.FindWithTag("charName").GetComponent<Text>();
		charImage = GameObject.FindWithTag("charImage");
    }

    public void SelectNewCharacter(Character ch)
    {
        // Set the pointer to the CurrentCharacter
        pointerObject.transform.position = ch.Position + new Vector3(0, 1, 0);
		// Set enemy selector to current target
		charName.text = ch.Name;
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
