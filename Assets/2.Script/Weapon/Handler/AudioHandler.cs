using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("# Main Data")]
    List<AudioSource> _sources = new List<AudioSource>();

    [Header("# References Data")]
    Gun _gun;
    AudioClip _shotAudio;
    AudioClip _reloadAudio;

    void Awake()
    {
        _gun = GetComponent<Gun>();
        if (_gun != null)
        {
            GunData gunData = _gun.Data as GunData;
            _shotAudio = gunData.ShotAudio;
            _reloadAudio = gunData.ReloadAudio;
        }
    }

    public void PlayShotAudio()
    {
        if (_shotAudio == null) return;
        PlayAudioClip(_shotAudio);
    }

    public void PlayReloadAudio()
    {
        if (_reloadAudio == null) return;
        PlayAudioClip(_reloadAudio);
    }

    void PlayAudioClip(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource availableSource = GetAudioSource();

        availableSource.PlayOneShot(clip);
    }

    AudioSource GetAudioSource()
    {
        AudioSource result = null;

        foreach (var source in _sources)
        {
            if (!source.isPlaying)
            {
                result = source;
            }
        }

        if (result == null)
        {
            result = gameObject.AddComponent<AudioSource>();
            result.volume = 1f;
            result.playOnAwake = false;
            _sources.Add(result);
        }

        return result;
    }
}
