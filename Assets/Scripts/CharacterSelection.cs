using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterSelection : MonoBehaviour, IPointerClickHandler
{
    public Character Me;
    public bool selectable = false;

    protected GoTweenConfig flashing;
    protected GoTween gt;

    public void Start()
    {
        SelectionController.Instance.OnSelectModeChange.AddListener(ModeChange);
        flashing = new GoTweenConfig();
        flashing.addTweenProperty(new ColorTweenProperty("color", Color.red));
        flashing.loopType = GoLoopType.PingPong;
        flashing.iterations = -1;
    }

    public void ModeChange(SelectMode mode)
    {
        if (mode == SelectMode.Enemy && TurnManager.Instance.CurrentCharacter.Team != Me.Team)
        {
            selectable = true;
            gt = Go.to(GetComponent<SpriteRenderer>(), 0.5f, flashing);
        }
        else
        {
            Go.removeTween(gt);
            GetComponent<SpriteRenderer>().color = Color.white;
            selectable = false;
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable)
        {
            // Select character
            TurnManager.Instance.CurrentCharacter.SetTarget(Me);
            SelectionController.Instance.ChangeMode(SelectMode.None);
        }
    }
}
