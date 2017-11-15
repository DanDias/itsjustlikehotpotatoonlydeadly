using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CharacterInfoWindowController : MonoBehaviour
{
	public Text charName;
	public GameObject charImage;

	public void Start()
	{
		TurnManager.Instance.OnChangeTurn.AddListener(ChangeCharactInfoWindow);
	}

	public void ChangeCharactInfoWindow(Character ch)
	{
		charName.text = ch.Name;
		charImage.GetComponent<Image>().sprite = GetSprite("Assets/Sprites/" + ch.staticSprite + "/" + ch.staticSprite + ".png", ch.staticSprite + "_78");
		//charImage.GetComponent<Image>().sprite = GetSprite("Assets/Sprites/guy2/guy2.png", "guy2_78");
	}

	public Sprite GetSprite(string path, string name)
	{
		Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

		if(sprites != null)
		{
			for(int i = 0; i < sprites.Length; i++)
			{
				if(sprites[i].name == name)
					return (Sprite)sprites[i];
			}
		}
		return null;
	}
}
