using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundSystemDef {
    public string soundName;
    public AudioClip clip;
}

public class SoundSystem : MonoBehaviour {
    public int numberOfSources = 20;
    public AudioSource audioSourcePrefab;

    public SoundSystemDef[] soundLibrary = null;

    private int current = 0;
    private AudioSource[] sources;

    private Dictionary<string, AudioClip> soundMap = new Dictionary<string, AudioClip>(); 

    void Awake() {
        for (int i = 0; i < soundLibrary.Length; i++) {
            SoundSystemDef def = soundLibrary[i];
            soundMap[def.soundName] = def.clip;
        }
    }

    void Start() {
        sources = new AudioSource[numberOfSources];
        for (int i = 0; i < numberOfSources; i++) {
            AudioSource source = Instantiate(audioSourcePrefab, transform.position, transform.rotation) as AudioSource;
            source.transform.parent = this.transform;
            sources[i] = source;
        }
    }

    private AudioSource GetNextSource() {
        AudioSource source = sources[current];
        current = (current+1)%numberOfSources;
        return source;
    }

    public int PlaySound(string soundName) {
        if (!this.enabled) {
            return 0;
        }

        if (!soundMap.ContainsKey(soundName)) {
            Util.Log("Tried to play nonexistent sound {0}", soundName);
            return -1;
        }

        AudioClip clip = soundMap[soundName];
        AudioSource source = GetNextSource();
        source.clip = clip;
        source.loop = false;
        source.Play();

        return current;
    }
}
