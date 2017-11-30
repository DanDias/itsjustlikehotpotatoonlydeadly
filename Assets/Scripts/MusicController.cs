using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour 
{
	public AudioClip[] Music;
	public AudioSource musicSource; 

	// Use this for initialization
	void Start () 
	{
		if(PlayerPrefs.GetFloat("MusicVolume") != 0)
			musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
		PlayMusic();
	}
	
	// Update is called once per frame
	public void PlayMusic()
	{
		musicSource.Stop();
		musicSource.clip = Music[Random.Range(0, Music.Length)];
		musicSource.Play();
		Invoke("PlayMusic", musicSource.clip.length);
	}

	public void ChangeVolume(float newVolume)
	{
		PlayerPrefs.SetFloat("MusicVolume", newVolume);
		musicSource.volume = newVolume;
	}
}
