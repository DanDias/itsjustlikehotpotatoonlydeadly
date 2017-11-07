using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TurnManager : Singleton<TurnManager>
{
    protected TurnManager() { } // Guarantee it'll be a singleton only

    // Events
    public UnityEvent OnChangeTurn = new UnityEvent();
    public UnityEvent<Character> OnRegisterCharacter = new CharacterEvent();

    // Properties
    public int TurnCounter { get; protected set; }
    public Character CurrentCharacter { get; protected set; }

    protected Dictionary<int, List<Character>> teams;
    protected Queue<Character> TurnOrder;

    // Use this for initialization
    void Start()
    {
        // TEST
		RegisterCharacter(new Character("Good Guy 1"), 1);
		RegisterCharacter(new Character("Good Guy 2"), 1);
		RegisterCharacter(new Character("Good Guy 3"), 1);
		RegisterCharacter(new Character("Bad Guy 1"), 2);
		RegisterCharacter(new Character("Bad Guy 2"), 2);
		RegisterCharacter(new Character("Bad Guy 3"), 2);
        StartBattle();
        /*for (int i = 0; i < 12; i++)
        {
            NextTurn();
            Debug.LogFormat("Turn {0} - {1}",TurnCounter,CurrentCharacter.Name);
			if (teams[1].Contains(CurrentCharacter))
				Debug.LogFormat("Good guy team turn");
			else
				Debug.LogFormat("bad guys turn");
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Initializes turn order and starts turn counter
    /// </summary>
    public void StartBattle()
    {
        TurnCounter = 0;
        if (TurnOrder != null)
            TurnOrder.Clear();
        else
            TurnOrder = new Queue<Character>();
        // TODO: Some way to order these better
        List<Character> all = new List<Character>();
        // Determine turn order
        foreach(KeyValuePair<int, List<Character>> kvp in teams)
        {
            all.AddRange(kvp.Value);
        }
        // TODO: could compare x and y's speed or something
        all.Sort((x, y) => Random.Range(-1, 1));
        foreach(Character c in all)
        {
            TurnOrder.Enqueue(c);
        }
		NextTurn();
    }

    public void RegisterCharacter(Character ch, int team)
    {
        if (teams == null)
            teams = new Dictionary<int, List<Character>>();
        if (!teams.ContainsKey(team))
            teams[team] = new List<Character>();
        teams[team].Add(ch);
        // TODO: Maybe just set in Character with a constructor
        ch.Team = team;
        if (OnRegisterCharacter != null)
            OnRegisterCharacter.Invoke(ch);
    }

    public void NextTurn()
    {
        TurnCounter++;
        // Peel off the next character
        CurrentCharacter = TurnOrder.Dequeue();
        // Queue them back into the turn order
        TurnOrder.Enqueue(CurrentCharacter);
        // Tell everyone it's the next turn
        if (OnChangeTurn != null)
            OnChangeTurn.Invoke();
    }

	public void Attack()
	{
		// bad test code to set a random target
		int t = 1;
		if (teams[t].Contains(CurrentCharacter)) 
			t = 2;
		CurrentCharacter.SetTarget(teams[t][Random.Range(0,3)]);
		Debug.LogFormat("{0} attacked {1}", CurrentCharacter.Name, CurrentCharacter.myTarget.Name);
		CurrentCharacter.ThrowGrenade();
		NextTurn();
	}
}
