using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TurnManager : Singleton<TurnManager>
{
    protected TurnManager() { } // Guarantee it'll be a singleton only

    // Events
    public CharacterEvent OnRegisterCharacter = new CharacterEvent();
	public CharacterEvent OnCharacterDeath = new CharacterEvent();
    public GrenadeEvent OnRegisterGrenade = new GrenadeEvent();
    public CharacterEvent OnChangeTurn = new CharacterEvent();
    public CharacterEvent OnEnemySelect = new CharacterEvent();
    public SelectModeEvent OnSelectModeChange = new SelectModeEvent();

    // Properties
    public int TurnCounter { get; protected set; }
    public Character CurrentCharacter { get; protected set; }
    public Skill CurrentSkill { get; protected set; }

    protected Dictionary<int, List<Character>> teams;
    protected Queue<Character> TurnOrder;

    public SelectMode CurrentMode { get; protected set; }

    // Use this for initialization
    void Start()
    {
        // TEST
		RegisterCharacter(new Character("Good Guy 1", 1));
		RegisterCharacter(new Character("Good Guy 2", 1));
		RegisterCharacter(new Character("Good Guy 3", 1));
		RegisterCharacter(new Character("Bad Guy 1", 2));
		RegisterCharacter(new Character("Bad Guy 2", 2));
		RegisterCharacter(new Character("Bad Guy 3", 2));
        StartBattle();
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

    public void RegisterCharacter(Character ch)
    {
        if (teams == null)
            teams = new Dictionary<int, List<Character>>();
		if (!teams.ContainsKey(ch.Team))
			teams[ch.Team] = new List<Character>();
		teams[ch.Team].Add(ch);
        OnRegisterCharacter.Invoke(ch);
        ch.OnTargetSelected.AddListener(SelectTarget);
    }

    public void RegisterGrenade(Grenade g)
    {
        OnRegisterGrenade.Invoke(g);
    }

    public void NextTurn()
    {
        TurnCounter++;
        // Peel off the next character
        CurrentCharacter = TurnOrder.Dequeue();
        // Remove dead characters from turn queue
		while(CurrentCharacter.isDead)
		{
			CurrentCharacter = TurnOrder.Dequeue();
		}
        // Queue them back into the turn order
        TurnOrder.Enqueue(CurrentCharacter);
        // Ready mode
        CurrentMode = SelectMode.Skill;
        // Tell everyone it's the next turn
        OnChangeTurn.Invoke(CurrentCharacter);
        // Tick down skill's cooldowns
        foreach (Skill sk in CurrentCharacter.Skills)
            sk.ChangeCooldown(-1);
        // If someone is knocked down skip their turn
        if (CurrentCharacter.isKnockedDown == true)
        {
            CurrentCharacter.isKnockedDown = false;
            NextTurn();
        }
    }

	public void Attack()
	{
        // TODO: This is no longer an attack, it's mostly clean up.
		Debug.LogFormat("{0} attacked {1}", CurrentCharacter.Name, CurrentCharacter.myTarget.Name);
		//CurrentCharacter.ThrowGrenade();
		if (CurrentCharacter.myTarget.isDead) 
		{
			Debug.LogFormat ("{0} has died.", CurrentCharacter.myTarget.Name);
            // Tell everyone the character is dead
		    OnCharacterDeath.Invoke(CurrentCharacter.myTarget);
			teams[CurrentCharacter.myTarget.Team].Remove(CurrentCharacter.myTarget);
			if(teams[1].Count == 0)
			{
				// you lose
			}
			else if (teams[2].Count == 0)
			{
				// you win
			}
			CurrentCharacter.SetTarget(null);
		}
			
		NextTurn();
	}

    // Sets the current target
    public void SetCurrentTarget(Character target)
    {
        CurrentCharacter.SetTarget(target);
        if (CurrentSkill != null)
        {
            // TODO: This is terrible. Should probably make this syntax better
            CurrentSkill.CharacterTargets.Clear();
            CurrentSkill.CharacterTargets.Add(target);
            CurrentSkill.Execute();
            // TODO: Figure out a better way to deal with this change... just call Attack for now
            Attack();
            CurrentSkill = null;
        }
    }

    public void SetCurrentSkill(Skill sk)
    {
        CurrentSkill = sk;
        CurrentSkill.Source = CurrentCharacter;
        if (sk.Mode != SelectMode.None)
            ChangeMode(sk.Mode);
        else
        {
            sk.Execute();
            NextTurn();
        }

    }

    // Informs everyone what the current target is. Should not be used by anything other than the event.
    // TODO: Is this redundant? Or maybe there should be a data object for the turn data instead of doing so much in TurnManager...
	protected void SelectTarget(Character target)
	{
        // Tell everyone the active target has changed
        OnEnemySelect.Invoke(CurrentCharacter.myTarget);
    }

    public void ChangeMode(SelectMode mode)
    {
        Debug.Log("Changing to mode: " + mode);
        CurrentMode = mode;
        // Tell everyone the selection has changed
        OnSelectModeChange.Invoke(mode);
    }
}

public enum SelectMode { None, Skill, Enemy, Team }