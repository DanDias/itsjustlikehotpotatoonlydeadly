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
    public ThrowEvent OnThrown = new ThrowEvent();
    public ThrowEvent OnCaught = new ThrowEvent();
    public GrenadeEvent OnExploded = new GrenadeEvent();

    protected ThrowData currentThrow;
    protected float throwTime = 0; 
    protected static float speed = 1f;

    public Grenade (int mxT)
	{
		CurrentTick = mxT;
		shakeCount = Random.Range(1, 3);
        //Debug.LogFormat("shakeCount {0}", shakeCount);
        World.Instance.AddGrenade(this);
    }

    public void Update(float deltaTime)
    {
        if (currentThrow != null)
        {
            throwTime += deltaTime;
            Vector3 arc = new Vector3(0, Mathf.Sin(throwTime*Mathf.PI), 0);
            Vector3 newPos = Vector3.Lerp(currentThrow.Source.Position, currentThrow.Target.Position, throwTime * speed);
            SetPosition(newPos+arc);
            if (Vector3.Distance(newPos,currentThrow.Target.Position) == 0)
            {
                currentThrow.Target.ReceiveGrenade(this); // TODO: This is probably bad... but whatever
                OnCaught.Invoke(currentThrow);
                currentThrow.Source.ActionCompleted();
                currentThrow = null;
                throwTime = 0;
            }
        }
    }

    public void SetPosition(Vector3 pos)
    {
        Position = pos;
        OnMove.Invoke(this);
    }

    public void Throw(ThrowData data)
    {
        currentThrow = data;
        OnThrown.Invoke(data);
    }

    public void Catch(ThrowData data)
    {
        OnCaught.Invoke(data);
        currentThrow = null;
    }

	public void ChangeTick(int tick)
	{
		CurrentTick += tick;
		//Debug.LogFormat("ticking... {0}", CurrentTick);
		if(CurrentTick <= shakeCount)
		{
			//Debug.LogFormat("bout to explode... {0}", CurrentTick);
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

