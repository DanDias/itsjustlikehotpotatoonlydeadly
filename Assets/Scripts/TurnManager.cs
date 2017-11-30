using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TurnManager
{
    private static TurnManager _instance;

    private static object _lock = new object();
    public static TurnManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new TurnManager();
                }
                return _instance;
            }
        }
    }
    protected TurnManager() { } // Guarantee it'll be a singleton only

    // Events
    public IntEvent OnGameEnd = new IntEvent(); // int is the winning team number

    public CharacterEvent OnTurnStart = new CharacterEvent();
    public CharacterEvent OnTurnEnd = new CharacterEvent();
    public IntEvent OnRoundStart = new IntEvent();
    public IntEvent OnRoundEnd = new IntEvent();
    public SelectModeEvent OnSelectModeChange = new SelectModeEvent();

    // Properties
    public int RoundCounter { get; protected set; }
    public Character CurrentCharacter { get; protected set; }

    protected FlexibleQueue<Character> TurnOrder;

    protected Character lastCharacter;

    protected bool gameOver = false;

    /// <summary>
    /// Initializes turn order and starts turn counter
    /// </summary>
    public void StartBattle()
    {
        RoundCounter = 1;
        if (TurnOrder != null)
            TurnOrder.Clear();
        else
            TurnOrder = new FlexibleQueue<Character>();
        // Determine turn order
        TurnOrder = new FlexibleQueue<Character>(World.Instance.Characters.OrderBy(x=>Random.Range(-1,1)));
        lastCharacter = TurnOrder.Last();
        World.Instance.OnCharacterRemoved.AddListener(updateQueue);
        OnRoundStart.Invoke(RoundCounter);
        NextTurn();
    }

    public void NextTurn()
    {
        if (CurrentCharacter != null)
        {
            OnTurnEnd.Invoke(CurrentCharacter);
			if(CurrentCharacter == lastCharacter)
			   NextRound();
        }
        if (gameOver)
            return;
        // Peel off the next character
        CurrentCharacter = TurnOrder.Dequeue();
        // Remove dead characters from turn queue
        while (CurrentCharacter.isDead)
        {
			if(CurrentCharacter == lastCharacter)
			   NextRound();
            // Skip turn if they are dead
            TurnOrder.Enqueue(CurrentCharacter);
            CurrentCharacter = TurnOrder.Dequeue();
        }
        // Queue them back into the turn order
        TurnOrder.Enqueue(CurrentCharacter);
        // Tick down skill's cooldowns
        foreach (Skill sk in CurrentCharacter.Skills)
            sk.ChangeCooldown(-1);
        // If someone is knocked down skip their turn
        // TODO: This seems misplaced... Not sure where it should go yet.
        if (CurrentCharacter.isKnockedDown == true)
        {
            CurrentCharacter.SetKnockdown(false);
			CurrentCharacter.SetGettingUp(true);
            NextTurn();
			// if you don't return out it does some funny stuff like skip people's turns
			return;
        }
		// Tell everyone it's the next turn
		OnTurnStart.Invoke(CurrentCharacter);
        CurrentCharacter.OnActionEnd.AddListener(characterFinished);
    }

    public void NextRound()
    {
        OnRoundEnd.Invoke(RoundCounter);
        RoundCounter++;
        World.Instance.Tick();
        checkGameEnd();
        if (!gameOver)
            OnRoundStart.Invoke(RoundCounter);
    }

    protected void checkGameEnd()
    {
        if (!World.Instance.Teams.ContainsKey(1)) // Everyone gone from team 1
        {
            // you lose
            OnGameEnd.Invoke(2);
            gameOver = true;
        }
        else if (!World.Instance.Teams.ContainsKey(2)) // Everyone gone from team 2
        {
            // you win
            OnGameEnd.Invoke(1);
            gameOver = true;
        }
    }

    public void CleanUp()
    {
        gameOver = false;

        if (TurnOrder != null)
            TurnOrder.Clear();

        OnGameEnd.RemoveAllListeners();
        OnRoundEnd.RemoveAllListeners();
        OnRoundStart.RemoveAllListeners();
        OnSelectModeChange.RemoveAllListeners();
        OnTurnEnd.RemoveAllListeners();
        OnTurnStart.RemoveAllListeners();
    }

    protected void updateQueue(Character c)
    {
        
    }

    protected void characterFinished(Character c)
    {
        c.OnActionEnd.RemoveListener(characterFinished);
        if (CurrentCharacter.Target != null && CurrentCharacter.Target.isDead)
        {
            Debug.LogFormat("{0} has died.", CurrentCharacter.Target.Name);
            // Tell everyone the character is dead
            //OnCharacterDeath.Invoke(CurrentCharacter.myTarget);
            // TODO: Remove
            World.Instance.Teams[CurrentCharacter.Target.Team].Remove(CurrentCharacter.Target);
            checkGameEnd();
            CurrentCharacter.SetTarget(null);
        }

        if (!gameOver)
            NextTurn();
    }
}

public enum SelectMode { None, Skill, Enemy, Team }