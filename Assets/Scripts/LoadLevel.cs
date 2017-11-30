using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour{

	public void Load(string level)
	{
        // Clean Up
        World.Instance.CleanUp();
        TurnManager.Instance.CleanUp();

        SceneManager.LoadScene(level);
	}
}
