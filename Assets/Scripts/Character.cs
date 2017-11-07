using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Properties
    public string Name { get; set; }
    public int Team { get; set; }
    public Vector3 Position { get; protected set; }

    // Events
    public CharacterEvent OnPositionChange = new CharacterEvent();

    public Grenade myGrenade;
	public Character myTarget;
	public bool isDead;

    public Character(string name)
    {
        Name = name;
		myGrenade = null;
		myTarget = null;
		isDead = false;
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
			if (myGrenade == null)
				myGrenade = new Grenade (3);

			myTarget.ReceiveGrenade (myGrenade);
			myGrenade.SetPosition (myTarget.Position);
			myGrenade = null;
		}
	}

	public void ReceiveGrenade(Grenade thrownGrenade)
	{
		myGrenade = thrownGrenade;
		myGrenade.ChangeTick(-1);
		if (myGrenade.exploded) 
		{
			myGrenade = null;
			isDead = true;
		}
	}

	public void SetTarget(Character target)
	{
		myTarget = target;
	}
}
