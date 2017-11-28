using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip[] Clips;
    protected Dictionary<Grenade, AudioSource> sources = new Dictionary<Grenade, AudioSource>();

	// Use this for initialization
	void Start ()
    {
        World.Instance.OnGrenadeAdded.AddListener(RegisterGrenade);
        World.Instance.OnGrenadeRemoved.AddListener(DeregisterGrenade);
    }

    void RegisterGrenade(Grenade g)
    {
        sources[g] = Camera.main.gameObject.AddComponent<AudioSource>();
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
}
