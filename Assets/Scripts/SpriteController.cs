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
	Grenade gToRemove;
    Vector3 grenadeArmOffset = new Vector3(0.35f, -0.45f, 0);

    public Vector3[] positions;

    protected List<int> takenPositions = new List<int>();

    public Dictionary<Character, GameObject> CharacterToGameObj = new Dictionary<Character, GameObject>();
    public Dictionary<Grenade, GameObject> GrenadeToGameObj = new Dictionary<Grenade, GameObject>();
	Dictionary<Grenade, int> GrenadesToRemove = new Dictionary<Grenade, int>();

    // Use this for initialization
    void Awake()
    {
        // Hook up to see when a new characters is registered
        TurnManager.Instance.OnRegisterCharacter.AddListener(CreateCharacter);
		TurnManager.Instance.OnCharacterDeath.AddListener(KillCharacter);
        TurnManager.Instance.OnRegisterGrenade.AddListener(CreateGrenade);
	}
	
	// Update is called once per frame
	void Update()
    {
        List <Grenade> keys = GrenadesToRemove.Keys.ToList();
		for(int i = 0; i < keys.Count; i++) 
		{
			GrenadesToRemove[keys[i]]++;
			if (GrenadesToRemove[keys[i]] == 75) {
				RemoveGrenade(keys[i]);
			}
		}
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
        GameObject obj = Instantiate(grenadePrefab, TurnManager.Instance.CurrentCharacter.Position + grenadeArmOffset, Quaternion.identity);
        g.OnChange.AddListener(ChangeGrenade);
        //g.OnRemove.AddListener(RemoveGrenade);
        GrenadeToGameObj[g] = obj;
    }

    public void ChangeGrenade(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];
        // If the grenade has switched positions, move it to their hand
        if (Vector3.Distance(obj.transform.position, g.Position) > 0)
        {
            // Do curve
            Vector3 start = obj.transform.position;
            Vector3 end = g.Position + grenadeArmOffset;
            var points = new Vector3[] {
                start,
                new Vector3(start.x+(end.x-start.x)/2,(end.y-start.y)+3,start.z),
                end
            };
            var path = new GoSpline(points);
            GoTween gt = Go.to(obj.transform, 1f, new GoTweenConfig().positionPath(path, false));
            gt.setOnCompleteHandler(t =>
            {
                if (g.exploded)
                {
                    obj.GetComponent<SpriteRenderer>().sprite = explosionSprite;
                    obj.transform.localScale = new Vector3(.1f, .1f, 0f);
                    GrenadesToRemove[g] = 0;
                }
            });

        }
    }

    public void RemoveGrenade(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];
        //g.OnRemove.RemoveAllListeners();
        g.OnChange.RemoveAllListeners();
        Destroy(obj);
        GrenadeToGameObj.Remove(g);
		GrenadesToRemove.Remove(g);
    }

	public void KillCharacter(Character ch)
	{
		Debug.LogFormat ("kill character {0}", ch.Name);
		anim = CharacterToGameObj[ch].GetComponent<Animator> ();
		anim.SetTrigger("Die");
	}
}
