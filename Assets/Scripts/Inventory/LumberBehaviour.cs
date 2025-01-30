using System.Collections;
using DefaultNamespace;
using Unity.VisualScripting;
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

    private void OnDestroy()
    {
        var dropItem = Instantiate(woodItem, transform.position + .3f * Vector3.up + transform.up * transform.lossyScale.y / 2, transform.rotation);
        dropItem.transform.localScale = transform.localScale;
        if (dropItem.TryGetComponent<ItemBehaviour>(out var itemBehaviour))
        {
            Destroy(itemBehaviour);
        }
                    
        if (!dropItem.TryGetComponent<Rigidbody>(out var rb))
        {
            rb = dropItem.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.excludeLayers = LayerMask.GetMask("Player");
        Destroy(rb, 1.4f);
                
        if (dropItem.TryGetComponent<Collider>(out var coll))
        {
            coll.isTrigger = false;
            coll.enabled = true;
            Destroy(coll, 44.4f);
        }
        else 
            Destroy(dropItem.AddComponent<SphereCollider>(), 44.4f);
    }
}
