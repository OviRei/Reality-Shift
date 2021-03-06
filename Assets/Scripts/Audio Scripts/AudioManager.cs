using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Audio")]
	[SerializeField] private static AudioManager instance;
	[SerializeField] private AudioMixerGroup mixerGroup;
	[SerializeField] private Sound[] sounds;

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach(Sound s in sounds)
		{
			if(s.source == null) s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.spatialBlend = s.spatialBlend;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if(s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.clip = s.clip;
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	//public IEnumerator StartFade(string sound, float duration)
    //{
	//	Sound s = Array.Find(sounds, item => item.name == sound);
	//	if(s == null) yield break;
//
    //    float currentTime = 0;
    //    float start = s.source.volume;
//
    //    while(currentTime < duration)
    //    {
    //        currentTime += Time.deltaTime;
    //        s.source.volume = Mathf.Lerp(start, 0, currentTime / duration);
    //        yield return null;
    //    }
	//	yield break;
    //}
}