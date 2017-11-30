using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip[] Clips;
    protected Dictionary<Grenade, AudioSource> sources = new Dictionary<Grenade, AudioSource>();
	float soundVolume;

	// Use this for initialization
	void Start ()
    {
		if(PlayerPrefs.GetFloat("SoundVolume") != 0)
			soundVolume = PlayerPrefs.GetFloat("SoundVolume");
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
        sources[data.Grenade].clip = Clips.FirstOrDefault(c => c.name.Contains("ThrowWhistle"));
        sources[data.Grenade].Play();
    }

    void GrenadeExploded(Grenade g)
    {
        sources[g].clip = Clips.FirstOrDefault(c => c.name.Contains("ExplosionFall"));
        sources[g].Play();
    }

	public void ChangeVolume(float newVolume)
	{
		soundVolume = newVolume;
		PlayerPrefs.SetFloat("SoundVolume", newVolume);
		var keys = sources.Keys;
		foreach(Grenade g in keys)
		{
			sources[g].volume = newVolume;
		}
	}
}
