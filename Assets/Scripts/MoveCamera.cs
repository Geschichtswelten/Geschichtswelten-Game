using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform camPos;

    // Update is called once per frame
    private void Update()
    {
        transform.position = camPos.position;
    }
}
