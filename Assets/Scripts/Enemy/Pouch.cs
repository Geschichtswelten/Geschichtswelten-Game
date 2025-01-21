using UnityEngine;

public class Pouch : MonoBehaviour
{
    [SerializeField] private float despawnTime;
    void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    
}
