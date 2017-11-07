using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Properties
    public string Name { get; set; }
    public int Team { get; set; }

    public Grenade myGrenade;
	public Character myTarget;

    public Character(string name)
    {
        Name = name;
		myGrenade = null;
		myTarget = null;
    }

	public void ThrowGrenade()
	{
		Debug.LogFormat ("Throwing grenade");
		if (myTarget == null)
			System.Console.WriteLine ("Select a target");
		
		if(myGrenade == null)
			myGrenade = new Grenade(3);

		myTarget.ReceiveGrenade (myGrenade);
		myGrenade = null;
	}

	public void ReceiveGrenade(Grenade thrownGrenade)
	{
		myGrenade = thrownGrenade;
		myGrenade.ChangeTick(-1);
		if(myGrenade.exploded)
			myGrenade = null;
	}

	public void SetTarget(Character target)
	{
		myTarget = target;
	}
}
