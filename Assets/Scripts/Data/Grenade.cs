using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade
{
	int shakeCount;
	public int CurrentTick { get; protected set;}
	public bool exploded = false;
	public bool boutToExplode = false;

    public Vector3 Position { get; protected set; }

    public GrenadeEvent OnMove = new GrenadeEvent();
    public GrenadeEvent OnChange = new GrenadeEvent();
    public GrenadeEvent OnThrown = new GrenadeEvent();
    public GrenadeEvent OnCaught = new GrenadeEvent();
    public GrenadeEvent OnExploded = new GrenadeEvent();

    public Grenade (int mxT)
	{
		CurrentTick = mxT;
		shakeCount = Random.Range(1, 3);
		Debug.LogFormat("shakeCount {0}", shakeCount);
	}

    public void SetPosition(Vector3 pos)
    {
        Position = pos;
        OnMove.Invoke(this);
    }

	public void ChangeTick(int tick)
	{
		CurrentTick += tick;
		Debug.LogFormat("ticking... {0}", CurrentTick);
		if(CurrentTick <= shakeCount)
		{
			Debug.LogFormat("bout to explode... {0}", CurrentTick);
			boutToExplode = true;
		}
		if(CurrentTick <= 0)
			Explode();
        OnChange.Invoke(this);
	}

	public void Explode()
	{
		exploded = true;
        OnExploded.Invoke(this);
	}
}

