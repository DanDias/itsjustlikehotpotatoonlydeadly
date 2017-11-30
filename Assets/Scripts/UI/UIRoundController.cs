using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundController : MonoBehaviour
{
    public RectTransform RoundPanel;
    public Text RoundText;
    public Text RoundNumText;
	public GameObject Continue;

    protected GoTweenChain movementTween;
    
	// Use this for initialization
	void Start ()
    {
        TurnManager.Instance.OnRoundStart.AddListener(newRound);
        TurnManager.Instance.OnGameEnd.AddListener(gameOver);
        RoundPanel.anchoredPosition = new Vector2(0, 150);

        // Movement fun
        movementTween = new GoTweenChain();
        movementTween.append(new GoTween(RoundPanel,0.5f,new GoTweenConfig().anchoredPosition(new Vector2(0,-50))));
        movementTween.appendDelay(1);
        movementTween.append(new GoTween(RoundPanel, 0.5f, new GoTweenConfig().anchoredPosition(new Vector2(0, 150))));
    }

    void newRound(int round)
    {
        RoundNumText.text = round.ToString();
        movementTween.restart();
    }

    void gameOver(int team)
    {
        // TODO: This is dumb reuse
        RoundText.text = "Team " + team.ToString();
        RoundNumText.text = "Wins!";

        RoundPanel.anchoredPosition = new Vector2(0, -50);
		Debug.LogFormat("Game over man... gmae over");
		Continue.SetActive(true);
    }
}
