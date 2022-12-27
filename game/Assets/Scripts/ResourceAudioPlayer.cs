using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceAudioPlayer : MonoBehaviour
{
	public string[] resourcePaths;
	public AudioSource audioSource;
	public bool loop;
	public bool playOnAwake;

	void Awake()
	{
		if (playOnAwake)
			Fire();
	}

	public void Fire()
	{
		DoAudio(true);
	}

	public void DoAudio(bool fire)
	{
		var clips = new List<AudioClip>();
		foreach (var resourcePath in resourcePaths)
		{
			var audioClip = Resources.Load<AudioClip>(resourcePath);
			if (audioClip)
				clips.Add(audioClip);
			else
				Debug.LogWarning($"Not playing {resourcePath} since it doesn't exist");
		}

		if (clips.Count == 0)
			return;

		var clip = clips[Random.Range(0, clips.Count)];
		audioSource.loop = loop;
		audioSource.clip = clip;
		if (fire)
			audioSource.PlayOneShot(clip);
		else
			audioSource.Play();
	}

	public void Play()
	{
		DoAudio(false);
	}
}
