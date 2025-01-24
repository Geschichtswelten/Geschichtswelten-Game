using System;
using Mono.CSharp;
using UnityEditor.TerrainTools;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(AudioSource))]
public class ArrowScript : MonoBehaviour
{
    public float despawnTime;
    private float arrowSpeed;
    private Vector3 targetPos;
    private Rigidbody rb;
    private Animator animator;
    private Collider collider;
    [HideInInspector] public EnemyArcher archer;
    private AudioSource audioSource;
    [SerializeField] private Transform forcePoint;
    [SerializeField] private AudioClip[] hitClips;
    private Quaternion rot;
    private bool shot = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = ButtonHandler.settings.masterVolume;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rot = transform.rotation;
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

    private void Update()
    {
        if(shot)
        {
            transform.rotation = rot;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) { //Nur f�r Debugging
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = hitClips[Random.Range(0, hitClips.Length)];
            audioSource.Play();
            if (other.CompareTag("Player"))
            {
                Destroy(gameObject, 3);
            }
            else {

                rb.constraints = RigidbodyConstraints.FreezeAll;
                animator.SetTrigger("Impact");
                Destroy(collider);
            }
        }
    }

    public void ShootArrow(Vector3 targetPos)
    {
        if (archer != null)
        {
            
            audioSource.Play();
            arrowSpeed = archer.arrowSpeed;
            this.targetPos = targetPos;
            animator.SetTrigger("shootArrow");
            rb.constraints = RigidbodyConstraints.None;
            transform.LookAt(new Vector3(targetPos.x, targetPos.y + 2, targetPos.z));
            rot = transform.rotation;
            rb.AddForceAtPosition((transform.position - forcePoint.position).normalized * arrowSpeed, forcePoint.position,ForceMode.Impulse);
            shot = true;
        }
        Destroy(gameObject, despawnTime);
    }
}
