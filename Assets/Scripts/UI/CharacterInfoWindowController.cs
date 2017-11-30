using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CharacterInfoWindowController : MonoBehaviour
{
	public Text charName;
	public GameObject charImage;

	public void Awake()
	{
		TurnManager.Instance.OnTurnStart.AddListener(ChangeCharactInfoWindow);
	}

	public void ChangeCharactInfoWindow(Character ch)
	{
		charName.text = ch.Name;
        // Still frame
		charImage.GetComponent<Image>().sprite = GetSprite("Assets/Sprites/" + ch.Sprite + "/" + ch.Sprite + ".png", ch.Sprite + "_78");
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
