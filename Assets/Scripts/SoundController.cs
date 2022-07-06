using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController main;

    [SerializeField] private AudioSource _audioSourcesPrefab;

    private List<AudioSource> _audioSources = new List<AudioSource>();

    public void PlayClip(AudioClip clip)
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying == false)
            {
                _audioSources[i].PlayOneShot(clip);
                return;
            }
        }


        _audioSources.Add(Instantiate(_audioSourcesPrefab, transform));
        _audioSources[_audioSources.Count - 1].PlayOneShot(clip);
    }

    private void Awake()
    {
        main = this;
    }
}
