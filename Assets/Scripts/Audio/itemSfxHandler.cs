using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class ItemSfxHandler : MonoBehaviour
{
    [SerializeField] private List<AudioClip> action1Sounds = new List<AudioClip>();
    
    [SerializeField] private AudioSource source;

    private void Awake()
    {
        source.volume = ButtonHandler.settings.masterVolume;
    }
    
    private void OnEnable()
    {
        ButtonHandler.OnSettingsChanged += HandleVolumeChange;
    }

    private void OnDisable()
    {
        ButtonHandler.OnSettingsChanged -= HandleVolumeChange;
    }

    private void HandleVolumeChange()
    {
        source.volume = ButtonHandler.settings.masterVolume;
    }

    public void PlayAction1()
    {
        if(action1Sounds == null)
            return;
        
        var clip = action1Sounds[Random.Range(0, action1Sounds.Count)];
        source.PlayOneShot(clip);
    }
}