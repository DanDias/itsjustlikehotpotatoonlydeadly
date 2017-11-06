using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Properties
    public string Name { get; set; }

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
		if(myGrenade == null)
			myGrenade = new Grenade(3);
		else {
			if (myTarget == null)
				System.Console.WriteLine ("Select a target");
			else {
				myTarget.ReceiveGrenade (myGrenade);
				myGrenade = null;
			}
		}
	}

	public void ReceiveGrenade(Grenade thrownGrenade)
	{
		myGrenade = thrownGrenade;
		myGrenade.ChangeTick(-1);
	}

	public void SetTarget(Character target)
	{
		myTarget = target;
	}
}
