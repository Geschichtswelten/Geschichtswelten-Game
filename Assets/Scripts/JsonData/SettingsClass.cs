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
    public int resolution;

    //Keybindings
    public SettingsClass(float masterVolume, float dialogueVolume,
        float musicVolume, int resolution)
    {
        this.masterVolume = masterVolume;
        this.dialogueVolume = dialogueVolume;
        this.musicVolume = musicVolume;
        this.resolution = resolution;
    }
    public SettingsClass()
    {
        this.masterVolume = 0;
        this.dialogueVolume = 0;
        this.musicVolume = 0;
        this.resolution = 0;
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

    public void SetResolution(int res)
    {
        resolution = res;
    }

}
