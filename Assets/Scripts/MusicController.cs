using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour 
{
	public AudioClip[] Music;
	public AudioSource musicSource;
	public Slider volumeControl;
	float musicVolume = 0.5f;

	// Use this for initialization
	void Start () 
	{
		if(PlayerPrefs.GetFloat("MusicVolume") == -1)
			musicVolume = 0;
		else
			musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		
		musicSource.volume = musicVolume;
		volumeControl.value = musicSource.volume;
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
		musicSource.volume = newVolume;
		if(newVolume == 0)
			newVolume = -1;
		
		PlayerPrefs.SetFloat("MusicVolume", newVolume);
	}
}
