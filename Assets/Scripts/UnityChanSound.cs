using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityChan2DController), typeof(AudioSource))]
public class UnityChanSound : MonoBehaviour {

	[SerializeField]
	private AudioClip damageVoice;

	[SerializeField]
	private AudioClip[] jumpVoices;

	private AudioSource audioSource;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void OnDamage()
	{
		PlayVoice(damageVoice);
	}

	void Jump()
	{
		int i = Random.Range(0, jumpVoices.Length);
		PlayVoice(jumpVoices[i]);
	}

	void PlayVoice(AudioClip voice)
	{
		audioSource.Stop();
		audioSource.PlayOneShot(voice);
	}

}
