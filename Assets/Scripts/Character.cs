﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Properties
    public string Name { get; set; }
    public int Team { get; set; }
    public Vector3 Position { get; protected set; }
    // TODO: Probably should be a Skill class that does a bit more than just display a name.
    public List<Skill> Skills { get; protected set; }

    // Events
    public CharacterEvent OnPositionChange = new CharacterEvent();

    public List<Grenade> myGrenades;
	public Character myTarget;
	public bool isDead;


	int leftOrRight = -1;

	public Character(string name, int t)
    {
        Name = name;
		myGrenades = new List<Grenade>();
		myTarget = null;
		isDead = false;
		Team = t;
		if(Team == 2)
			leftOrRight = 1;
        Skills = new List<Skill>();
        // TODO: Load skills from text file or something
        Skills.Add(new Skill("Hurl Grenade"));
        Skills.Add(new Skill("Lob Grenade"));
        Skills.Add(new Skill("Throw Grenade"));
        Skills.Add(new Skill("Do something cool"));
    }

    public void SetPosition(Vector3 pos)
    {
        Position = pos;
        if (OnPositionChange != null)
            OnPositionChange.Invoke(this);
    }

	public void ThrowGrenade()
	{
		Debug.LogFormat ("Throwing grenade");
		if (myTarget == null)
			System.Console.WriteLine ("Select a target");
		else 
		{
			if (myGrenades.Count == 0)
				myGrenades.Add(new Grenade(3));

			myTarget.ReceiveGrenade (myGrenades[0]);
			//myGrenades[0].SetPosition (myTarget.Position);
			myGrenades.Remove(myGrenades[0]);
			foreach(Grenade g in myGrenades)
			{
				g.SetPosition(g.Position + new Vector3(-leftOrRight, 0, 0));
			}
		}
	}

	public void ReceiveGrenade(Grenade thrownGrenade)
	{
		thrownGrenade.ChangeTick(-1);
		thrownGrenade.SetPosition(Position + new Vector3(leftOrRight * myGrenades.Count, 0, 0));
		myGrenades.Add(thrownGrenade);
		if (thrownGrenade.exploded) 
		{
			foreach(Grenade g in myGrenades)
			{
				g.Explode();
				g.SetPosition(g.Position);
			}
			isDead = true;
		}
	}

	public void SetTarget(Character target)
	{
		myTarget = target;
	}
}
