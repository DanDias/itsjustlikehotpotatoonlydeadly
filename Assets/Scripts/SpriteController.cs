using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteController : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject grenadePrefab;
    public Sprite explosionSprite;

	Animator anim;

    public Vector3[] positions;

    protected List<int> takenPositions = new List<int>();

    public Dictionary<Character, GameObject> CharacterToGameObj = new Dictionary<Character, GameObject>();
    public Dictionary<Grenade, GameObject> GrenadeToGameObj = new Dictionary<Grenade, GameObject>();

    // Use this for initialization
    void Awake()
    {
        // Hook up to see when a new characters is registered
        TurnManager.Instance.OnRegisterCharacter.AddListener(CreateCharacter);
		TurnManager.Instance.OnCharacterDeath.AddListener(KillCharacter);
        TurnManager.Instance.OnRegisterGrenade.AddListener(CreateGrenade);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void CreateCharacter(Character ch)
    {
        Vector3 position = Vector3.zero;
        if (ch.Team == 1)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (!takenPositions.Contains(i))
                {
                    takenPositions.Add(i);
                    position = positions[i-1];
                    break;
                }
            }
        }
        else if (ch.Team == 2)
        {
            for (int i = 4; i <= 6; i++)
            {
                if (!takenPositions.Contains(i))
                {
                    takenPositions.Add(i);
                    position = positions[i-1];
                    break;
                }
            }
        }
        // Create Character in scene
        ch.SetPosition(position);
        GameObject obj = Instantiate(characterPrefab, position, Quaternion.identity);
        // Set name
        obj.name = ch.Name;
        CharacterToGameObj[ch] = obj;
    }

    public void CreateGrenade(Grenade g)
    {
        GameObject obj = Instantiate(grenadePrefab, Vector3.zero, Quaternion.identity);
        g.OnChange.AddListener(ChangeGrenade);
        g.OnRemove.AddListener(RemoveGrenade);
        GrenadeToGameObj[g] = obj;
    }

    public void ChangeGrenade(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];
        obj.transform.position = g.Position;
        if (g.exploded)
        {
            obj.GetComponent<SpriteRenderer>().sprite = explosionSprite;
        }
    }

    public void RemoveGrenade(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];
        g.OnRemove.RemoveAllListeners();
        g.OnChange.RemoveAllListeners();
        Destroy(obj);
        GrenadeToGameObj.Remove(g);
    }

	public void KillCharacter(Character ch)
	{
		Debug.LogFormat ("kill character {0}", ch.Name);
		anim = CharacterToGameObj[ch].GetComponent<Animator> ();
		anim.SetTrigger("Die");
	}
}
