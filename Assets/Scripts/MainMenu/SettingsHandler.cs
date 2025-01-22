using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [Header("Children")] 
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject scrollView;
    [Space]

    [Header("Settings")]
    [Header("Audio")]
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider dialogueVolume;
    [SerializeField] private Slider musicVolume;

    [Space] [Header("Graphic Settings")] 
    [SerializeField] private TMP_Dropdown resolution;

    [Space] [Header("Keybindings")] 
    public int PLACEHOLDER;


    void Awake()
    {
        backButton.SetActive(false);
        scrollView.SetActive(false);
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
        if (masterVolume != null && dialogueVolume != null && musicVolume != null && resolution != null)
        {
            masterVolume.value = ButtonHandler.settings.masterVolume;
            dialogueVolume.value = ButtonHandler.settings.dialogueVolume;
            musicVolume.value = ButtonHandler.settings.musicVolume;

            resolution.value = ButtonHandler.settings.resolution;
        }

    }

    public void leaveSettings()
    {
        JsonHandler.WriteSettings(ButtonHandler.settings);
    }

    public void ChangedMasterVolume()
    {
        if (masterVolume != null)
        {
            ButtonHandler.settings.SetMasterVolume(masterVolume.value);
            ButtonHandler.InvokeOnSettingsChanged();
        }
    }
    public void ChangedDialogueVolume()
    {
        if (dialogueVolume != null)
        {
            ButtonHandler.settings.SetDialogueVolume(dialogueVolume.value);
            ButtonHandler.InvokeOnSettingsChanged();
        }
    }
    public void ChangedMusicVolume()
    {
        if (musicVolume != null)
        {
            ButtonHandler.settings.SetMusicVolume(musicVolume.value);
            ButtonHandler.InvokeOnSettingsChanged();
        }
    }
    public void ChangedResolution()
    {
        if (resolution != null)
        {
            ButtonHandler.settings.SetResolution(resolution.value);
            ButtonHandler.InvokeOnSettingsChanged();
        }
    }
    


}
