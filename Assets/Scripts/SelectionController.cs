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
            // Tween it to make it disappear after 3 seconds
            SpriteRenderer sr = enemySelectObject.GetComponent<SpriteRenderer>();
            sr.color = new Color(1, 1, 1, 1);
            GoTween gt = Go.to(sr, 3f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 0)));
            gt.easeType = GoEaseType.CubicOut;
            
        }
	}
}