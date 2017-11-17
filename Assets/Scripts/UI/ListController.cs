using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

	// Use this for initialization
	void Start ()
    {
        TurnManager.Instance.OnChangeTurn.AddListener(PopulateSkills);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void PopulateSkills(Character ch)
    {
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
            obj.GetComponent<Button>().onClick.AddListener(()=>
            {
                if (TurnManager.Instance.CurrentMode == SelectMode.Skill)
                {
                    TurnManager.Instance.SetCurrentSkill(skill);
                }
            });
            obj.GetComponentInChildren<Text>().text = skill.Name;
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
}
