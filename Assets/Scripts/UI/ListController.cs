using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour
{
    public GameObject ContentPanel;

    public GameObject ListItemPrefab;

	// Use this for initialization
	void Awake()
    {
        TurnManager.Instance.OnTurnStart.AddListener(PopulateSkills);
        TurnManager.Instance.OnGameEnd.AddListener(gameOver);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void DisableSkills(ThrowData data)
    {
        data.Source.OnThrowStart.RemoveListener(DisableSkills);
        for(int i=0;i<ContentPanel.transform.childCount;i++)
        {
            Transform obj = ContentPanel.transform.GetChild(i);
            obj.gameObject.SetActive(false);
        }
    }

    void PopulateSkills(Character ch)
    {
        // TODO: Decouple AI from controllers eventually
        // If AI, don't show skills
        if (ch.Team == 2)
            return;
        ch.OnThrowStart.AddListener(DisableSkills);
        List<Skill> skills = ch.Skills;

        for(int i=0;i< skills.Count;i++)
        {
            GameObject obj = null;
            Skill skill = skills[i];
            if (i + 1 > ContentPanel.transform.childCount)
            {
                obj = Instantiate(ListItemPrefab);
                obj.transform.SetParent(ContentPanel.transform);
            }
            else
            {
                obj = ContentPanel.transform.GetChild(i).gameObject;
            }
            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            // TODO: Maybe this needs to be set on skill init?
            skill.Source = ch;
            if (skill.MeetRequirements())
            {
                obj.GetComponent<Button>().enabled = true;
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (SelectionController.Instance.CurrentMode == SelectMode.Skill)
                    {
                        TurnManager.Instance.CurrentCharacter.SetActiveSkill(skill);
                        //TODO: This seems hacky. Target for refactoring.
                        if (skill.Mode == SelectMode.None)
                            TurnManager.Instance.CurrentCharacter.ExecuteSkill();
                        else
                            SelectionController.Instance.ChangeMode(skill.Mode);
                    }
                });
            }
            else
            {
                //Debug.Log(skill.Name + " does not meet requirements, disabling.");
                obj.GetComponent<Button>().enabled = false;
            }
			obj.GetComponentInChildren<Text>().text = skill.Name + "   " + skill.Cooldown;
            obj.SetActive(true);
        }
        if (skills.Count < ContentPanel.transform.childCount)
        {
            for(int i= skills.Count-1;i<ContentPanel.transform.childCount;i++)
            {
                ContentPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void gameOver(int team)
    {
        // TODO: Something less dumb...
        ContentPanel.transform.parent.parent.gameObject.SetActive(false);
    }
}
