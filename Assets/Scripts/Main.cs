using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        NameGenerator.Instance.Initialize(Resources.Load<TextAsset>("names").text);
        // TEST
        World.Instance.AddCharacter(new Character(NameGenerator.Instance.GetPersona(), 1));
        World.Instance.AddCharacter(new Character(NameGenerator.Instance.GetPersona(), 1));
        World.Instance.AddCharacter(new Character(NameGenerator.Instance.GetPersona(), 1));
        World.Instance.AddCharacter(new Character(NameGenerator.Instance.GetPersona(), 2));
        World.Instance.AddCharacter(new Character(NameGenerator.Instance.GetPersona(), 2));
        World.Instance.AddCharacter(new Character(NameGenerator.Instance.GetPersona(), 2));
        TurnManager.Instance.StartBattle();
    }
	
	// Update is called once per frame
	void Update ()
    {
        World.Instance.Update(Time.deltaTime);
	}
}
