using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : UnityEvent<Character> { }

public class GrenadeEvent : UnityEvent<Grenade> { }

public class SelectModeEvent : UnityEvent<SelectMode> { }
