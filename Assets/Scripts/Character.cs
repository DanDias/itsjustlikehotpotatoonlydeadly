using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Properties
    public string Name { get; protected set; }

    public Character(string name)
    {
        Name = name;
    }
}
