using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteController : MonoBehaviour
{
    public List<GameObject> characterPrefabs;
    public GameObject grenadePrefab;
    public Sprite explosionSprite;

	int gExplodeTime = 75;
	int cDeathTime = 75;

	Animator cAnim;
	Animator gAnim;
	Grenade gToRemove;
    Vector3 grenadeArmOffset = new Vector3(0.35f, -0.45f, 0);

    public Vector3[] positions;

    protected List<int> takenPositions = new List<int>();

    public Dictionary<Character, GameObject> CharacterToGameObj = new Dictionary<Character, GameObject>();
    public Dictionary<Grenade, GameObject> GrenadeToGameObj = new Dictionary<Grenade, GameObject>();
	Dictionary<Grenade, int> GrenadesToRemove = new Dictionary<Grenade, int>();
	Dictionary<Character, int> CharactersToDie = new Dictionary<Character, int>();

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
        List<Grenade> gKeys = GrenadesToRemove.Keys.ToList();
		for(int i = 0; i < gKeys.Count; i++) 
		{
			GrenadesToRemove[gKeys[i]]++;
			if (GrenadesToRemove[gKeys[i]] == gExplodeTime) {
				RemoveGrenade(gKeys[i]);
			}
		}

		List<Character> cKeys = CharactersToDie.Keys.ToList();
		for(int i = 0; i < cKeys.Count(); i++)
		{
			CharactersToDie[cKeys[i]]++;
			if (CharactersToDie[cKeys[i]] == cDeathTime) {
				PlayDeathAnimation(cKeys[i]);
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
		int prefabSpot = Random.Range(0,characterPrefabs.Count);
		GameObject obj = Instantiate(characterPrefabs[prefabSpot], position, Quaternion.identity);
		ch.staticSprite = obj.name.Replace("(Clone)", "");
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
			gAnim = obj.GetComponent<Animator>();
			gAnim.SetTrigger("isMoving");
            GoTween gt = Go.to(obj.transform, 1f, new GoTweenConfig().positionPath(path, false));
            gt.setOnCompleteHandler(t =>
            {
				gAnim = obj.GetComponent<Animator>();
				if(g.CurrentTick == 1)
					gAnim.SetTrigger("isShaking");
				else
					gAnim.SetTrigger("isStopped");
				
                if (g.exploded)
                {
					//obj.GetComponent<Animator>().enabled = false;
					gAnim.enabled = false;
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
		CharactersToDie[ch] = 0;
	}

	void PlayDeathAnimation(Character ch)
	{
		CharactersToDie.Remove(ch);
		cAnim = CharacterToGameObj[ch].GetComponent<Animator>();
		cAnim.SetTrigger("Die");
	}
}
