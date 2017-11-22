using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : UnityEvent<Character> { }

public class GrenadeEvent : UnityEvent<Grenade> { }

public class SelectModeEvent : UnityEvent<SelectMode> { }

public class ThrowEvent : UnityEvent<ThrowData> { }

public class IntEvent : UnityEvent<int> { }


public struct ThrowData
{
    private readonly Character source;
    public Character Source { get { return source; } }
    private readonly Character target;
    public Character Target { get { return target; } }
    private readonly Skill skill;
    public Skill Skill { get { return skill; } }
    private readonly Grenade grenade;
    public Grenade Grenade { get { return grenade; } }

    public ThrowData(Character s, Character t, Skill sk, Grenade g)
    {
        this.source = s;
        this.target = t;
        this.skill = sk;
        this.grenade = g;
    }
}