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
	int cDeathTime = 5;

	Animator cAnim;
	Animator gAnim;
	//Grenade gToRemove;
    //Vector3 grenadeArmOffset = new Vector3(0.35f, -0.45f, 0);

    public Vector3[] positions;

    protected List<int> takenPositions = new List<int>();

    public Dictionary<Character, GameObject> CharacterToGameObj = new Dictionary<Character, GameObject>();
    public Dictionary<Grenade, GameObject> GrenadeToGameObj = new Dictionary<Grenade, GameObject>();
	Dictionary<Grenade, int> GrenadesToRemove = new Dictionary<Grenade, int>();
	Dictionary<Character, int> CharactersToDie = new Dictionary<Character, int>();

    // Use this for initialization
    void Awake()
    {
        // Hook up to global events
        World.Instance.OnCharacterAdded.AddListener(CreateCharacter);
        World.Instance.OnGrenadeAdded.AddListener(CreateGrenade);
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
        obj.GetComponent<CharacterSelection>().Me = ch;
        // Set name
        obj.name = ch.Name;
        CharacterToGameObj[ch] = obj;
        // Hook events
        ch.OnChange.AddListener(CharacterChange);
        ch.OnThrowStart.AddListener(CharacterStartThrow);
        ch.OnActionEnd.AddListener(CharacterEndAction);
        ch.OnTargetSelected.AddListener(CharacterSelected);
    }

    protected void CharacterChange(Character c)
    {
        if (c.isDead)
            KillCharacter(c);
        if (c.isKnockedDown)
            KnockdownCharacter(c);
    }

    protected void CharacterStartThrow(ThrowData data)
    {

    }

    protected void CharacterEndAction(Character c)
    {

    }

    protected void CharacterSelected(Character c)
    {

    }

    public void KnockdownCharacter(Character c)
    {
        Animator anim = CharacterToGameObj[c].GetComponent<Animator>();
        anim.SetBool("KnockedDown",c.isKnockedDown);
    }

    public void CreateGrenade(Grenade g)
    {
        GameObject obj = Instantiate(grenadePrefab, g.Position, Quaternion.identity);
        g.OnChange.AddListener(ChangeGrenade);
        g.OnMove.AddListener(MoveGrenade);
        g.OnThrown.AddListener(ThrowGrenade);
        g.OnCaught.AddListener(CatchGrenade);
        g.OnExploded.AddListener(grenadeExploded);
        GrenadeToGameObj[g] = obj;
    }

    public void ChangeGrenade(Grenade g)
    {
        grenadeSettle(g);
    }

    public void CatchGrenade(ThrowData data)
    {
        grenadeSettle(data.Grenade);
    }

    public void MoveGrenade(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];
        obj.transform.position = g.Position;
        /*
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
                grenadeSettle(g);
            });
        }
        */
    }

    public void ThrowGrenade(ThrowData data)
    {
        GameObject obj = GrenadeToGameObj[data.Grenade];
        obj.GetComponent<Animator>().SetTrigger("isMoving");
    }

    protected void grenadeSettle(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];

        gAnim = obj.GetComponent<Animator>();
        if (g.boutToExplode)
            gAnim.SetTrigger("isShaking");
        else
            gAnim.SetTrigger("isStopped");
    }

    protected void grenadeExploded(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];

        gAnim = obj.GetComponent<Animator>();
        gAnim.enabled = false;
        obj.GetComponent<SpriteRenderer>().sprite = explosionSprite;
        obj.transform.localScale = new Vector3(.1f, .1f, 0f);
        GrenadesToRemove[g] = 0;
    }

    public void RemoveGrenade(Grenade g)
    {
        GameObject obj = GrenadeToGameObj[g];
        g.OnChange.RemoveAllListeners();
        g.OnMove.RemoveAllListeners();
        Destroy(obj);
        GrenadeToGameObj.Remove(g);
		GrenadesToRemove.Remove(g);
    }

	public void KillCharacter(Character c)
	{
		Debug.LogFormat ("kill character {0}", c.Name);
		CharactersToDie[c] = 0;
	}

	void PlayDeathAnimation(Character c)
	{
		CharactersToDie.Remove(c);
		cAnim = CharacterToGameObj[c].GetComponent<Animator>();
		cAnim.SetTrigger("Die");
	}
}
