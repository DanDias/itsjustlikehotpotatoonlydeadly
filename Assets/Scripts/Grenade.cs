using System;
using System.Collections.Generic;
using UnityEngine;

public class Grenade
{
	int maxTick;
	int currentTick;

	public Grenade (int mxT)
	{
		maxTick = mxT;
		currentTick = maxTick;
	}

	public void ChangeTick(int tick)
	{
		currentTick += tick;
		if(currentTick <= 0)
			Explode();
	}

	public void Explode()
	{
		System.Console.WriteLine("BOOM!");
	}
}

