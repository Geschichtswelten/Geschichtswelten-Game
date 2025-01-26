using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FinalChoiceScript : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private AudioSource _source;
    public GameObject canv;
    private MainGameLoop loop;
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.loop = false;
        _source.playOnAwake = false;
        _source.volume = ButtonHandler.settings.masterVolume;
        loop = FindAnyObjectByType<MainGameLoop>();
    }

    public void KillArminius()
    {
        _source.clip = clip;
        _source.PlayDelayed(2);
        loop.killedArminius = true;
        loop.KilledArminius();
        Destroy(canv, 2);
    }

    public void SpareArminius()
    {
        loop.killedArminius = false;
        Destroy(canv);
    }
    
}
