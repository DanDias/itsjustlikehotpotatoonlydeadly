using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // TEST
        World.Instance.AddCharacter(new Character("Good Guy 1", 1));
        World.Instance.AddCharacter(new Character("Good Guy 2", 1));
        World.Instance.AddCharacter(new Character("Good Guy 3", 1));
        World.Instance.AddCharacter(new Character("Bad Guy 1", 2));
        World.Instance.AddCharacter(new Character("Bad Guy 2", 2));
        World.Instance.AddCharacter(new Character("Bad Guy 3", 2));
        TurnManager.Instance.StartBattle();
    }
	
	// Update is called once per frame
	void Update ()
    {
        World.Instance.Update(Time.deltaTime);
	}
}
