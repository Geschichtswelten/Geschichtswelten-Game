using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class ItemSfxHandler : MonoBehaviour
{
    [SerializeField] private List<AudioClip> action1Sounds = new List<AudioClip>();
    
    [SerializeField] private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayAction1()
    {
        if(action1Sounds == null)
            return;
        
        var clip = action1Sounds[Random.Range(0, action1Sounds.Count)];
        source.PlayOneShot(clip);
    }
}