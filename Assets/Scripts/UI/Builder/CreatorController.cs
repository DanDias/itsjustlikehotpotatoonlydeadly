using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public class CreatorController : MonoBehaviour
{
    public InputField nameField;
    public GameObject CharacterSkills;
    public GameObject AllSkills;
    public GameObject InfoContainer;
    public GameObject SaveLoadContainer;
    public GameObject SaveLoadContent;
    public Button OKButton;
    public InputField SlotField;

    public GameObject SkillListPrefab;
    public GameObject ListItemPrefab;

    protected Skill SelectedSkill;

    Character CurrentCharacter;

	// Use this for initialization
	void Start ()
    {
        InitSkills();
        CurrentCharacter = new Character();
        TextAsset list = Resources.Load<TextAsset>("names");
        NameGenerator.Instance.Initialize(list.text);
        PlayerPrefs.DeleteAll();
    }

    public void AddSkill()
    {
        if (SelectedSkill != null) // TODO: Should you be able to add multiple skills you already have? If not, add a check here
        {
            CurrentCharacter.Skills.Add(SelectedSkill);
            RefreshSkillList(CharacterSkills, CurrentCharacter.Skills);
        }
    }

    public void RemoveSkill()
    {
        if (SelectedSkill != null && CurrentCharacter.Skills.Contains(SelectedSkill))
        {
            CurrentCharacter.Skills.Remove(SelectedSkill);
            RefreshSkillList(CharacterSkills, CurrentCharacter.Skills);
        }
    }

    void InitSkills()
    {
        // TODO: I should set this up so it reads from a list or something
        List<Skill> skills = ReflectiveEnumerator.GetEnumerableOfType<Skill>().ToList();

        RefreshSkillList(AllSkills, skills, s =>
         {
             InfoContainer.transform.Find("Name").GetComponent<Text>().text = "Name: " + s.Name;
             InfoContainer.transform.Find("Target").GetComponent<Text>().text = "Targets: " + s.Mode;
             InfoContainer.transform.Find("Cooldown").GetComponent<Text>().text = "Cooldown: " + s.MaxCooldown;
             InfoContainer.transform.Find("Description").GetComponent<Text>().text = "Description: " + s.Description;
         });
    }

    void RefreshSkillInformation()
    {

    }

    void RefreshSkillList(GameObject container, List<Skill> skills)
    {
        RefreshSkillList(container, skills, null);
    }

    void RefreshSkillList(GameObject container, List<Skill> skills, System.Action<Skill> clickFunction)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            GameObject obj = null;
            Skill skill = skills[i];
            if (i + 1 > container.transform.childCount)
            {
                obj = Instantiate(SkillListPrefab);
                obj.transform.SetParent(container.transform);
            }
            else
            {
                obj = container.transform.GetChild(i).gameObject;
            }
            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            obj.GetComponent<Button>().enabled = true;
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectedSkill = skill;
                if (clickFunction != null)
                    clickFunction(skill);
            });
            obj.GetComponentInChildren<Text>().text = skill.Name;
            obj.SetActive(true);
        }
        if (skills.Count < container.transform.childCount)
        {
            for (int i = container.transform.childCount; i > skills.Count; i--)
            {
                container.transform.GetChild(i-1).gameObject.SetActive(false);
            }
        }
    }

    public void RandomCharacterName()
    {
        nameField.text = NameGenerator.Instance.GetName();
    }
    
    void Save()
    {
        string slot = SlotField.text;
        CurrentCharacter.Name = nameField.text;
        XmlSerializer serializer = new XmlSerializer(typeof(Character));
        TextWriter writer = new StringWriter();
        serializer.Serialize(writer, CurrentCharacter);
        writer.Close();
        if (PlayerPrefs.GetString("Characters").Length > 0)
            PlayerPrefs.SetString("Characters", PlayerPrefs.GetString("Characters") + ";" + slot);
        else
            PlayerPrefs.SetString("Characters", slot);
        PlayerPrefs.SetString("ch_"+slot, writer.ToString());
        PlayerPrefs.Save();
        SaveLoadContainer.SetActive(false);
    }

    void Load()
    {
        string slot = SlotField.text;

        XmlSerializer serializer = new XmlSerializer(typeof(Character));
        string savedData = PlayerPrefs.GetString("ch_" + slot);
        if (savedData != null)
        {
            TextReader reader = new StringReader(savedData);
            CurrentCharacter = (Character)serializer.Deserialize(reader);
            reader.Close();
            nameField.text = CurrentCharacter.Name;
            RefreshSkillList(CharacterSkills, CurrentCharacter.Skills);
            SaveLoadContainer.SetActive(false);
        }
        else
        {
            Debug.LogError("There is no characters saved under that slot.");
        }
    }

    public void ShowSaveScreen()
    {
        SaveLoadContainer.SetActive(true);
        SaveLoadContainer.transform.Find("Window").Find("Title").GetComponentInChildren<Text>().text = "Save";
        OKButton.GetComponentInChildren<Text>().text = "Save";
        OKButton.onClick.RemoveAllListeners();
        OKButton.onClick.AddListener(() =>
        {
            if(SlotField.text.Length > 0)
                Save();
        });
        PopulateCharacterList();
        SlotField.text = nameField.text;
    }

    public void ShowLoadScreen()
    {
        SaveLoadContainer.SetActive(true);
        SaveLoadContainer.transform.Find("Window").Find("Title").GetComponentInChildren<Text>().text = "Load";
        OKButton.onClick.RemoveAllListeners();
        OKButton.onClick.AddListener(() =>
        {
            Load();
        });
        OKButton.GetComponentInChildren<Text>().text = "Load";
        PopulateCharacterList();
    }

    void PopulateCharacterList()
    {
        string cs = PlayerPrefs.GetString("Characters");
        if (cs.Length > 0)
        {
            // Remove list
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < SaveLoadContent.transform.childCount; i++)
            {
                children.Add(SaveLoadContent.transform.GetChild(i));
            }
            SaveLoadContent.transform.DetachChildren();
            children.ForEach(c => Destroy(c.gameObject));

            // Add list
            string[] chars = cs.Split(';');
            foreach (string c in chars)
            {
                GameObject obj = Instantiate(ListItemPrefab);
                obj.transform.SetParent(SaveLoadContent.transform);
                obj.GetComponent<Text>().text = c;
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SlotField.text = c;
                });
            }
        }
    }

    public void HideSaveLoadScreen()
    {
        SaveLoadContainer.SetActive(false);
    }
}
