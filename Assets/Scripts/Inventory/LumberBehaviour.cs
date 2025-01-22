using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LumberBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    private BoxCollider boxCollider;
    [SerializeField] private AudioClip[] clip;
    [SerializeField] private int timberTime;
    [SerializeField] private GameObject woodItem;

    bool isTimber;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource.volume = ButtonHandler.settings.masterVolume;
        if (clip.Length > 0)
        {
            audioSource.clip = clip[Random.Range(0, clip.Length)];
        }
        isTimber = false;
        StartCoroutine(TransformToTimber());
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
    
    private void OnTriggerEnter(Collider other)
    {
        isTimber = true;
    }

    private IEnumerator TransformToTimber()
    {
        while (timberTime > 0 && !isTimber)
        {
            yield return new WaitForSeconds(1);
            timberTime--;
        }
        if (clip.Length > 0)
        {
            audioSource.Play();
        }
        Instantiate(woodItem, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }



}
