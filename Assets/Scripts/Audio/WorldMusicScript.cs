using Unity.VisualScripting;
using UnityEngine;

public class WorldMusicScript : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    private void Awake()
    {
    
        ButtonHandler[] ls = Resources.FindObjectsOfTypeAll<ButtonHandler>();
        if (ls.Length > 1)
        {
            if (ls[0] == this)
            {
                Destroy(ls[1].gameObject);
            }
        }
        source = GetComponent<AudioSource>();
        ButtonHandler.OnSettingsChanged += HandleSettingsChanged;
        DontDestroyOnLoad(this.gameObject);
    }
    private void HandleSettingsChanged()
    {
        
        source.volume = ButtonHandler.settings.musicVolume * ButtonHandler.settings.masterVolume;
    }

    public void StartingGame()
    {
        source.clip = clips[1];
    }
}
