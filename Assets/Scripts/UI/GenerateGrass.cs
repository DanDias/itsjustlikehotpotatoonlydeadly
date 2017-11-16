using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateGrass : MonoBehaviour
{
	public GameObject grass;
	public GameObject dirt;

	public void Awake()
	{
		int sizeX = 12;
		int sizeY = 5;
		for(int i = -sizeX; i <= sizeX; i++)
		{
			for(int j = -sizeY; j <= sizeY; j++)
			{
				if (Random.Range(0,100) < 95)
					Instantiate(grass, new Vector3(i, j, 0), Quaternion.identity);
				else
					Instantiate(dirt, new Vector3(i, j, 0), Quaternion.identity);
			}
		}
	}
}


