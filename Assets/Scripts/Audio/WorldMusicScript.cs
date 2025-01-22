using Unity.VisualScripting;
using UnityEngine;

public class WorldMusicScript : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void OnEnable()
    {
        ButtonHandler.OnSettingsChanged += HandleSettingsChanged;
    }

    private void OnDisable()
    {
        ButtonHandler.OnSettingsChanged -= HandleSettingsChanged;
    }
    private void HandleSettingsChanged()
    {
        
        source.volume = ButtonHandler.settings.musicVolume * ButtonHandler.settings.masterVolume;
    }

    public void ToggleMute()
    {
        if (source.mute == true)
        {
            source.mute = false;
        }
        else
        {
            source.mute = true;
        }
    }

    public void StartingGame()
    {
        source.clip = clips[1];
    }
}
