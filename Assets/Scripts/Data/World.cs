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
        grenades.ForEach(g => g.ChangeTick(-1));

        grenades.RemoveAll(g =>
        {
            if (g.exploded)
            {
                OnGrenadeRemoved.Invoke(g);
                return true;
            }
            return false;
        });

        characters.RemoveAll(c =>
        {
            Debug.Log("Checking " + c.Name);
            if (c.isDead)
            {
                Debug.Log("Removing " + c.Name);
                OnCharacterRemoved.Invoke(c);
                return true;
            }
            return false;
        });
    }
    
    public void CleanUp()
    {
        characters.Clear();
        grenades.Clear();

        OnCharacterAdded.RemoveAllListeners();
        OnCharacterRemoved.RemoveAllListeners();
        OnGrenadeAdded.RemoveAllListeners();
        OnGrenadeRemoved.RemoveAllListeners();
    }

    public Character GetAITarget()
	{
		return Teams[1][Random.Range(0, Teams[1].Count)];
	}
}
