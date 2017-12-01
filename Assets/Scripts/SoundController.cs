using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioClip[] Clips;
	public AudioClip Explosion;
	public Slider volumeControl;
    protected Dictionary<Grenade, AudioSource> sources = new Dictionary<Grenade, AudioSource>();
	float soundVolume = 0.5f;

	// Use this for initialization
	void Start ()
    {
		if(PlayerPrefs.GetFloat("SoundVolume") == -1)
			soundVolume = 0;
		else
			soundVolume = PlayerPrefs.GetFloat("SoundVolume");

		volumeControl.value = soundVolume;
        World.Instance.OnGrenadeAdded.AddListener(RegisterGrenade);
        World.Instance.OnGrenadeRemoved.AddListener(DeregisterGrenade);
    }

    void RegisterGrenade(Grenade g)
    {
		
        sources[g] = Camera.main.gameObject.AddComponent<AudioSource>();
		sources[g].volume = soundVolume;
        g.OnThrown.AddListener(GrenadeThrow);
        g.OnExploded.AddListener(GrenadeExploded);
    }

    void DeregisterGrenade(Grenade g)
    {
        //g.OnThrown.RemoveListener(GrenadeThrow);
        //g.OnExploded.RemoveListener(GrenadeExploded);
        //Destroy(sources[g]);
    }

    void GrenadeThrow(ThrowData data)
    {
        //sources[data.Grenade].clip = Clips.FirstOrDefault(c => c.name.Contains("ThrowWhistle"));
		sources[data.Grenade].clip = Clips[Random.Range(0, Clips.Length)];
        sources[data.Grenade].Play();
    }

    void GrenadeExploded(Grenade g)
    {
        //sources[g].clip = Clips.FirstOrDefault(c => c.name.Contains("ExplosionFall"));
		sources[g].clip = Explosion;
        sources[g].Play();
    }

	public void ChangeVolume(float newVolume)
	{
		soundVolume = newVolume;
		var keys = sources.Keys;
		foreach(Grenade g in keys)
		{
			sources[g].volume = newVolume;
		}
		if(newVolume == 0)
			newVolume = -1;
		
		PlayerPrefs.SetFloat("SoundVolume", newVolume);
	}
}
