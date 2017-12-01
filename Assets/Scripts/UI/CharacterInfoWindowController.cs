using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindowController : MonoBehaviour
{
	public Text charName;
	public GameObject charImage;
    public Sprite[] sprites;

	public void Awake()
	{
		TurnManager.Instance.OnTurnStart.AddListener(ChangeCharactInfoWindow);
	}

	public void ChangeCharactInfoWindow(Character ch)
	{
		charName.text = ch.Name;
        // Still frame
		charImage.GetComponent<Image>().sprite = GetSprite(ch.Sprite + "_78");
	}

	public Sprite GetSprite(string name)
	{
		if(sprites != null)
		{
			for(int i = 0; i < sprites.Length; i++)
			{
				if(sprites[i].name == name)
					return sprites[i];
			}
		}
		return null;
	}
}
