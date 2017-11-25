using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    private static World _instance;

    private static object _lock = new object();
    public static World Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new World();
                }
                return _instance;
            }
        }
    }
    protected World() { } // Guarantee it'll be a singleton only

    public CharacterEvent OnCharacterAdded = new CharacterEvent();
    public CharacterEvent OnCharacterRemoved = new CharacterEvent();
    public GrenadeEvent OnGrenadeAdded = new GrenadeEvent();
    public GrenadeEvent OnGrenadeRemoved = new GrenadeEvent();

    public List<Grenade> Grenades { get { return grenades; } }
    protected List<Grenade> grenades = new List<Grenade>();
    public List<Character> Characters { get { return characters; } }
    protected List<Character> characters = new List<Character>();
    
    public Dictionary<int, List<Character>> Teams {
        get
        {
            return characters.GroupBy(x => x.Team).ToDictionary(x => x.Key, x => x.ToList());
        }
    }

    public void Update(float deltaTime)
    {
        grenades.ForEach(g => g.Update(deltaTime));
        characters.ForEach(c => c.Update(deltaTime));
    }

    public void AddCharacter(Character c)
    {
        characters.Add(c);
        OnCharacterAdded.Invoke(c);
    }

    public void RemoveCharacter(Character c)
    {
        characters.Remove(c);
        OnCharacterRemoved.Invoke(c);
    }

    public void AddGrenade(Grenade g)
    {
        grenades.Add(g);
        OnGrenadeAdded.Invoke(g);
    }

    public void RemoveGrenade(Grenade g)
    {
        grenades.Remove(g);
        OnGrenadeRemoved.Invoke(g);
    }

    /// <summary>
    /// Tick the simulated world. This means grenades right now... but maybe status effects later?
    /// </summary>
    public void Tick()
    {
        foreach(Grenade g in grenades)
        {
            g.ChangeTick(-1);
        }
    }
}
