using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundController : MonoBehaviour
{
    public RectTransform RoundPanel;
    public Text RoundNumText;

    protected GoTweenChain movementTween;

	// Use this for initialization
	void Start ()
    {
        TurnManager.Instance.OnRoundStart.AddListener(NewRound);
        RoundPanel.anchoredPosition = new Vector2(0, 150);

        // Movement fun
        movementTween = new GoTweenChain();
        movementTween.append(new GoTween(RoundPanel,0.5f,new GoTweenConfig().anchoredPosition(new Vector2(0,-50))));
        movementTween.appendDelay(1);
        movementTween.append(new GoTween(RoundPanel, 0.5f, new GoTweenConfig().anchoredPosition(new Vector2(0, 150))));
    }

    void NewRound(int round)
    {
        RoundNumText.text = round.ToString();
        movementTween.restart();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
