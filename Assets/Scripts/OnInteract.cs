using UnityEngine;

public class OnInteract : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact()
    {
        Debug.Log("did something lol");
    }
}
