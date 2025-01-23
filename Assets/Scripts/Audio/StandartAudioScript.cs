using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StandartAudioScript : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = ButtonHandler.settings.masterVolume;
        audioSource.clip = audioClip;
        audioSource.Play();
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
        audioSource.volume = ButtonHandler.settings.masterVolume;
    }

    
}
