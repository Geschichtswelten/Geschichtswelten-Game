using System;

[Serializable]
public class SettingsClass
{
    //Audio (might add more)
    public float masterVolume;
    public float dialogueVolume;
    public float musicVolume;

   

    //UI
    //resolution: 0 = 16:9; 1 = 4:5
    public float mouseSensitivity;

    //Keybindings
    public SettingsClass(float masterVolume, float dialogueVolume,
        float musicVolume, float mouseSensitivity)
    {
        this.masterVolume = masterVolume;
        this.dialogueVolume = dialogueVolume;
        this.musicVolume = musicVolume;
        this.mouseSensitivity = mouseSensitivity;
    }
    public SettingsClass()
    {
        this.masterVolume = 0;
        this.dialogueVolume = 0;
        this.musicVolume = 0;
        this.mouseSensitivity = 100;
        
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }
    public void SetDialogueVolume(float volume)
    {
        dialogueVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void SetMouseSensitivity(float mouseSensitivity)
    {
        this.mouseSensitivity = mouseSensitivity;
    }

}
