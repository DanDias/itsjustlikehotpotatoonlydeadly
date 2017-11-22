using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TurnManager : Singleton<TurnManager>
{
    protected TurnManager() { } // Guarantee it'll be a singleton only

    // Events
    public IntEvent OnGameEnd = new IntEvent(); // int is the winning team number

    public CharacterEvent OnTurnStart = new CharacterEvent();
    public CharacterEvent OnTurnEnd = new CharacterEvent();
    public UnityEvent OnRoundStart = new UnityEvent();
    public UnityEvent OnRoundEnd = new UnityEvent();
    public SelectModeEvent OnSelectModeChange = new SelectModeEvent();

    // Properties
    public int TurnCounter { get; protected set; }
    public Character CurrentCharacter { get; protected set; }
    public Skill CurrentSkill { get; protected set; } // TODO: Remove. Moved to Character.ActiveSkill

    protected FlexibleQueue<Character> TurnOrder;

    protected Character lastCharacter;

    protected bool gameOver = false;

    /// <summary>
    /// Initializes turn order and starts turn counter
    /// </summary>
    public void StartBattle()
    {
        TurnCounter = 0;
        if (TurnOrder != null)
            TurnOrder.Clear();
        else
            TurnOrder = new FlexibleQueue<Character>();
        // Determine turn order
        TurnOrder = new FlexibleQueue<Character>(World.Instance.Characters.OrderBy(x=>Random.Range(-1,1)));
        lastCharacter = TurnOrder.Last();
        World.Instance.OnCharacterRemoved.AddListener(updateQueue);
        OnRoundStart.Invoke();
        NextTurn();
    }

    public void NextTurn()
    {
        if (CurrentCharacter != null)
        {
            OnTurnEnd.Invoke(CurrentCharacter);
            if (CurrentCharacter == lastCharacter)
                NextRound();
        }
        TurnCounter++;
        // Peel off the next character
        CurrentCharacter = TurnOrder.Dequeue();
        // Queue them back into the turn order
        TurnOrder.Enqueue(CurrentCharacter);
        // Tell everyone it's the next turn
        OnTurnStart.Invoke(CurrentCharacter);
        // Tick down skill's cooldowns
        foreach (Skill sk in CurrentCharacter.Skills)
            sk.ChangeCooldown(-1);
        // If someone is knocked down skip their turn
        if (CurrentCharacter.isKnockedDown == true)
        {
            CurrentCharacter.isKnockedDown = false;
            NextTurn();
        }
        CurrentCharacter.OnActionEnd.AddListener(characterFinished);
    }

    public void NextRound()
    {
        OnRoundStart.Invoke();
        World.Instance.Tick();
        OnRoundEnd.Invoke();
    }
    
    // Informs everyone what the current target is. Should not be used by anything other than the event.
    // TODO: Is this redundant? Or maybe there should be a data object for the turn data instead of doing so much in TurnManager...
	protected void SelectTarget(Character target)
	{
        // Tell everyone the active target has changed
        //OnEnemySelect.Invoke(CurrentCharacter.Target);
        Debug.Log("Calling TurnManager.SelectTarget... shouldn't be");
    }

    protected void checkGameEnd()
    {
        if (World.Instance.Teams[1].Count == 0)
        {
            // you lose
            OnGameEnd.Invoke(2);
            gameOver = true;
        }
        else if (World.Instance.Teams[2].Count == 0)
        {
            // you win
            OnGameEnd.Invoke(1);
            gameOver = true;
        }
    }

    protected void updateQueue(Character c)
    {
        // Remove dead characters from turn queue
        while (CurrentCharacter.isDead)
        {
            Character previous = CurrentCharacter;
            CurrentCharacter = TurnOrder.Dequeue();
            if (previous == lastCharacter)
                lastCharacter = TurnOrder.Last();
        }
    }

    protected void characterFinished(Character c)
    {
        c.OnActionEnd.RemoveListener(characterFinished);
        if (CurrentCharacter.Target.isDead)
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