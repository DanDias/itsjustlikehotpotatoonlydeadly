using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : IMenuItem
{
    public string Name { get; set; }

    /// <summary>
    /// Execute Event, Override in inheriting classes
    /// </summary>
    public virtual void Execute()
    {
        // TODO: Something smarter than just calling on TurnManager
        TurnManager.Instance.Attack();
    }

    public Skill(string name)
    {
        Name = name;
    }
}
