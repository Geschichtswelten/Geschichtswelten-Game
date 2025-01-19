using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

[RequireComponent(typeof(TMP_Text))]
public class performanceMonitor : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    private List<float> frameTimes = new List<float>();
    void Update()
    {
        frameTimes.Add(Time.unscaledDeltaTime);
    }

    private void FixedUpdate()
    {
        if (!frameTimes.Any()) return;
        var avgTime = frameTimes.Average();
        frameTimes.Clear();
        textLabel.text = (1/avgTime).ToString("000.0") + " FPS";
    }
}
