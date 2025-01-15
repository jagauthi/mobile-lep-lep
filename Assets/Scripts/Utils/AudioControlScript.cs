using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControlScript : MonoBehaviour {

	AudioMixerSnapshot startSnap, bossSnap;
	AudioSource currentSource, snowZone, maxSong, townSong, launcherMusic;

	void playClip(AudioSource clip) {
		currentSource.Stop();
		currentSource = clip;
		currentSource.Play();
	}

	void Start () {
		AudioSource[] comps = GetComponents<AudioSource>();
		snowZone = comps[0];
		maxSong = comps[1];
		townSong = comps[2];
		launcherMusic = comps[3];
		currentSource = snowZone;
		currentSource.Play();
	}

	void loadMusicClips() {
		//Use this to programmatically load the audio clips.
		//I'm not using this currently because I'm just having the audio sources 
		//	be components on the audio controller gameobject.
		snowZone.clip = (AudioClip)Resources.Load("Audio/SnowZone");
		maxSong.clip = (AudioClip)Resources.Load("Audio/MaxSong");
		townSong.clip = (AudioClip)Resources.Load("Audio/Town");
		launcherMusic.clip = (AudioClip)Resources.Load("Audio/LauncherMusic");
	}
	
	void Update () {
		
	}

	public void playAmbientSong() {
		playClip(launcherMusic);
	}

	public void playBossSong() {
		playClip(maxSong);
	}
}
