using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : IMenuItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Character Source { get; set; }
    public SelectMode Mode { get; set; }
    public int Cooldown { get; set; }
    public int MaxCooldown { get; protected set; }

    public List<Character> CharacterTargets = new List<Character>();
    public List<Grenade> GrenadeTargets = new List<Grenade>();

    public Skill(string name)
    {
        Name = name;
    }

    public Skill()
    {
        Name = "Throw";
    }

    public virtual void ChangeCooldown(int amount)
    {
        Cooldown += amount;
        if (Cooldown < 0)
            Cooldown = 0;
    }
    
    /// <summary>
    /// Execute Event, Override in inheriting classes, but always call base
    /// </summary>
    public virtual void Execute()
    {
        Cooldown = MaxCooldown;
    }

    /// <summary>
    /// Meets requirements. Basic requirement, skill is not on cooldown
    /// </summary>
    /// <returns></returns>
    public virtual bool MeetRequirements()
    {
        return Cooldown == 0;
    }
}
