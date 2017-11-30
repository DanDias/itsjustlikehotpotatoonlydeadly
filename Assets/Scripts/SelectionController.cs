using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : Singleton<SelectionController>
{
    public GameObject pointerObject;
	public GameObject enemySelectObject;

    public SelectMode CurrentMode { get; protected set; }

    public void Awake()
    {
        TurnManager.Instance.OnTurnStart.AddListener(SelectNewCharacter);
        TurnManager.Instance.OnGameEnd.AddListener(gameOver);
    }

    public void SelectNewCharacter(Character ch)
    {
        // Ready mode
		// Set the pointer to the CurrentCharacter
		pointerObject.transform.position = ch.Position + new Vector3(0, 1, 0);
		if(ch.Team != 2)
		{
			CurrentMode = SelectMode.Skill;
			ch.OnTargetSelected.AddListener(SelectNewEnemy);
		} else
		{
			StartCoroutine(AITurn(ch));
		}
    }

    // TODO: Decouple AI from controllers eventually
	public IEnumerator AITurn(Character ch)
	{
		ch.OnTargetSelected.AddListener(SelectNewEnemy);
		yield return new WaitForSeconds(1);
		ch.UseAI();
	}

    public void ChangeMode(SelectMode mode)
    {
        //Debug.Log("Changing to mode: " + mode);
        CurrentMode = mode;
        // Tell everyone the selection has changed
        //OnSelectModeChange.Invoke(mode);
    }

    public void SelectNewEnemy(Character ch)
	{
        if (ch.Target == null)
            enemySelectObject.SetActive(false);
        else
        {
            enemySelectObject.SetActive(true);
            // Set enemy selector to target
            enemySelectObject.transform.position = ch.Target.Position + new Vector3(0, -0.9f, 0);
            // Tween it to make it disappear after 3 seconds
            SpriteRenderer sr = enemySelectObject.GetComponent<SpriteRenderer>();
            sr.color = new Color(1, 1, 1, 1);
            GoTween gt = Go.to(sr, 3f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 0)));
            gt.easeType = GoEaseType.CubicOut;
            TurnManager.Instance.CurrentCharacter.ExecuteSkill();
            
        }
	}

    void gameOver(int team)
    {
        pointerObject.SetActive(false);
    }
}