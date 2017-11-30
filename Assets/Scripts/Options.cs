using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour 
{
	public RectTransform OptionsPanel;
	//public Text RoundText;
	//public Text RoundNumText;

	protected GoTweenChain movementTween;

	// Use this for initialization
	void Start () {
		// Movement fun
		movementTween = new GoTweenChain();
		movementTween.append(new GoTween(OptionsPanel,0.5f,new GoTweenConfig().anchoredPosition(new Vector2(0,-50))));
		movementTween.appendDelay(1);
		movementTween.append(new GoTween(OptionsPanel, 0.5f, new GoTweenConfig().anchoredPosition(new Vector2(0, 150))));
	}

	public void Close()
	{
		//RoundNumText.text = round.ToString();
		OptionsPanel.anchoredPosition = OptionsPanel.anchoredPosition + new Vector2(300, 0);
		//movementTween.restart();
	}

	public void Open()
	{
		// TODO: This is dumb reuse
		//RoundText.text = "Team " + team.ToString();
		//RoundNumText.text = "Wins!";

		OptionsPanel.anchoredPosition = OptionsPanel.anchoredPosition + new Vector2(-300, 0);
		//Continue.SetActive(true);
	}
}
