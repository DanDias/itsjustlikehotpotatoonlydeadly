using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Properties
    public string Name { get; set; }
    public int Team { get; set; }
    public Vector3 Position { get; protected set; }
    public List<Skill> Skills { get; protected set; }
    public Skill ActiveSkill { get; protected set; }
    public List<Grenade> Grenades { get { return myGrenades; } }
    public Character Target { get { return myTarget; } }

    public static Vector3 armOffset = new Vector3(0.35f, -0.45f, 0);

    // Events
    public CharacterEvent OnChange = new CharacterEvent();
    public CharacterEvent OnTargetSelected = new CharacterEvent();
    public ThrowEvent OnThrowStart = new ThrowEvent();
    public CharacterEvent OnActionEnd = new CharacterEvent();

    protected List<Grenade> myGrenades;
	protected Character myTarget;
    // TODO: Maybe eventually replace these with a character state
	public bool isDead { get; protected set; }
    public bool isKnockedDown { get; protected set; }

	public string staticSprite;
    
	int leftOrRight = -1;

	public Character(string name, int t)
    {
        Name = name;
		myGrenades = new List<Grenade>();
		myTarget = null;
		isDead = false;
        isKnockedDown = false;
		Team = t;
		if(Team == 2)
			leftOrRight = 1;
        Skills = new List<Skill>();
        // TODO: Load skills from text file or something
        Skills.Add(new Skills.Throw());
        Skills.Add(new Skills.Knockdown());
        Skills.Add(new Skills.Cook());
    }

    public void Update(float deltaTime)
    {

    }

    public void SetPosition(Vector3 pos)
    {
        Position = pos;
        if (OnChange != null)
            OnChange.Invoke(this);
    }

	public void ThrowGrenade()
	{
		//Debug.LogFormat ("Throwing grenade");
		if (myTarget == null)
			System.Console.WriteLine ("Select a target");
		else 
		{
            if (myGrenades.Count == 0)
            {
                //Grenade g = new Grenade(3);
                Grenade g = new Grenade(1); // For debugging grenade explodes
                g.SetPosition(Position);
                World.Instance.AddGrenade(g);
                myGrenades.Add(g);
                g.OnExploded.AddListener(grenadeExploded);
            }
            ThrowData data = new ThrowData(this, myTarget, ActiveSkill, myGrenades[0]);
            myGrenades[0].Throw(data);
            OnThrowStart.Invoke(data);
            FinishedThrowingGrenade();
        }
	}

    public void FinishedThrowingGrenade()
    {
        myGrenades[0].OnExploded.RemoveListener(grenadeExploded);
        myGrenades.Remove(myGrenades[0]);
        for(int i=0;i<myGrenades.Count;i++)
        {
            myGrenades[i].SetPosition(Position + armOffset + new Vector3(leftOrRight*i, 0, 0));
        }
    }

    public void ActionCompleted()
    {
        OnActionEnd.Invoke(this);
    }

	public void ReceiveGrenade(Grenade thrownGrenade)
	{
		thrownGrenade.SetPosition(Position + armOffset + new Vector3(leftOrRight * myGrenades.Count, 0, 0));
		myGrenades.Add(thrownGrenade);
        myGrenades[0].SetPosition(Position + armOffset);
        thrownGrenade.OnExploded.AddListener(grenadeExploded);
	}

    public void SetActiveSkill(Skill skill)
    {
        ActiveSkill = skill;
        ActiveSkill.Source = this;
    }

    // Sets the current target
    public void SetTarget(Character target)
	{
		myTarget = target;
        if (ActiveSkill != null)
        { 
            // TODO: This is terrible. Should probably make this syntax better
            ActiveSkill.CharacterTargets.Clear();
            ActiveSkill.CharacterTargets.Add(target);
        }
        // TODO: This event is a little weird, passing in character since it has a Target.
        //       Might expect this to be the target character... 
        //       thought about args with Character, Character, but eh...
        OnTargetSelected.Invoke(this); 
	}

    public void SetKnockdown(bool status)
    {
        isKnockedDown = status;
        OnChange.Invoke(this);
    }

    public void SetDead(bool status)
    {
        isDead = true;
        OnChange.Invoke(this);
    }

    public void ExecuteSkill()
    {
        if (ActiveSkill != null)
        {
            ActiveSkill.Execute();
            ActiveSkill = null;
        }
    }

    protected void grenadeExploded(Grenade g)
    {
        foreach(Grenade og in myGrenades)
        {
            if (!og.exploded)
            {
                og.Explode();
            }
        }
        SetDead(true);
    }
}
