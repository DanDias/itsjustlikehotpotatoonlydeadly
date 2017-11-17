using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterSelection : MonoBehaviour, IPointerClickHandler
{
    public Character Me;

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectMode mode = TurnManager.Instance.CurrentMode;
        if (mode == SelectMode.Enemy && TurnManager.Instance.CurrentCharacter.Team != Me.Team)
        {
            // Select character
            TurnManager.Instance.SetCurrentTarget(Me);
        }
    }
}
