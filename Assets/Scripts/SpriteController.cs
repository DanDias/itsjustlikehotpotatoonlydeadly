using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteController : MonoBehaviour
{
    public GameObject characterPrefab;

    public Vector3[] positions;

    protected List<int> takenPositions = new List<int>();

    public Dictionary<Character, Vector3> CharacterToPositionMap = new Dictionary<Character, Vector3>();

    // Use this for initialization
    void Awake()
    {
        // Hook up to see when a new characters is registered
        TurnManager.Instance.OnRegisterCharacter.AddListener(CreateCharacter);
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
        CharacterToPositionMap[ch] = position;
        // Create Character in scene
        GameObject obj = Instantiate(characterPrefab, position, Quaternion.identity);
        // Set name
        obj.name = ch.Name;
    }
}
