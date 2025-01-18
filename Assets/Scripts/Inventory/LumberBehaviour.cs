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
        audioSource.volume = ButtonHandler.settings.masterVolume;
        audioSource.clip = clip[Random.Range(0, clip.Length)];
        isTimber = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isTimber = true;
    }

    private IEnumerator TransformToTimber()
    {
        while (timberTime > 0 || !isTimber)
        {
            yield return new WaitForSeconds(1);
            timberTime--;
        }
        if (clip.Length > 0)
        {
            audioSource.Play();
        }
        Instantiate(woodItem, transform.position, Quaternion.identity);
        Instantiate(woodItem, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        Instantiate(woodItem, new Vector3(transform.position.x, transform.position.y + 6, transform.position.z), Quaternion.identity);
        Destroy(gameObject);

    }



}
