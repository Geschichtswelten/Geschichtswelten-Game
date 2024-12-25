using Unity.VisualScripting;
using UnityEngine;

public class WorldMusicScript : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        ButtonHandler.OnSettingsChanged += HandleSettingsChanged;
        DontDestroyOnLoad(this.gameObject);
    }
    private void HandleSettingsChanged()
    {
        source.volume = ButtonHandler.settings.musicVolume;
    }

    public void StartingGame()
    {
        source.clip = clips[1];
    }
}
