using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade
{
	int maxTick;
	int currentTick;
	public bool exploded = false;

	public Grenade (int mxT)
	{
		maxTick = mxT;
		currentTick = maxTick;
	}

	public void ChangeTick(int tick)
	{
		currentTick += tick;
		Debug.LogFormat("ticking... {0}", currentTick);
		if(currentTick <= 0)
			Explode();
	}

	public void Explode()
	{
		Debug.LogFormat("BOOM!");
		exploded = true;
	}
}

