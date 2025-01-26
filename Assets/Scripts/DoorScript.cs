using System;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public MainGameLoop loop;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            loop.Entered();
        }
    }
}
