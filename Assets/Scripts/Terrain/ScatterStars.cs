using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class StarBand
{
    [SerializeField] public float proportion, fromXZ, toXZ, fromY, toY;
}

public class ScatterStars : MonoBehaviour
{
    [SerializeField] private ParticleSystem starsParticleSystem;
    [SerializeField] private int starCount = 9000;
    [SerializeField] private StarBand[] spreads;
    public bool toggle;

    private ParticleSystem.Particle[] particles;
    private byte[] alphas;
    private Color32 color32;
    private float radius, spreadSum, spreadChooser, sum, angleXZ, angleY;
    private int chosenSpread;
    private Vector3 position;
    void Start()
    {
        toggle = true;
        alphas = new byte[starCount];
        particles = new ParticleSystem.Particle[starCount];

        starsParticleSystem.Emit(starCount);
        starsParticleSystem.GetParticles(particles, starCount, 0);
        radius = starsParticleSystem.shape.radius;
        spreadSum = 0f;
        foreach (StarBand spread in spreads)
        {
            spreadSum += spread.proportion;
        }

        for (int i = 0; i < particles.Length; i++)
        {
            alphas[i] = particles[i].startColor.a;
            spreadChooser = Random.Range(0f, spreadSum);
            sum = 0f;
            for (int j = 0; j < spreads.Length; j++)
            {
                sum += spreads[j].proportion;
                if (spreadChooser < sum)
                {
                    chosenSpread = j;
                    j = spreads.Length;
                }
            }

            angleXZ = Random.Range(Mathf.Deg2Rad * spreads[chosenSpread].fromXZ, Mathf.Deg2Rad * spreads[chosenSpread].toXZ);angleXZ = Random.Range(Mathf.Deg2Rad * spreads[chosenSpread].fromXZ, Mathf.Deg2Rad * spreads[chosenSpread].toXZ);
            angleY = Random.Range(Mathf.Deg2Rad * spreads[chosenSpread].fromY, Mathf.Deg2Rad * spreads[chosenSpread].toY);
            position.x = radius * Mathf.Cos(angleXZ) * Mathf.Sin(angleY);
            position.z = radius * Mathf.Sin(angleXZ) * Mathf.Sin(angleY);
            position.y = radius * Mathf.Cos(angleY);
            particles[i].position = position;
        }
        starsParticleSystem.SetParticles(particles, starCount);
    }


    public void Update()
    {
        if (toggle)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                color32 = particles[i].startColor;
                color32.a = (byte)Mathf.Clamp(
                    alphas[i] * (starsParticleSystem.transform.TransformPoint(particles[i].position).y -
                                 starsParticleSystem.transform.position.y) / radius, 0,
                    alphas[i]);
                particles[i].startColor = color32;
                if (color32.a <= 10 || color32.a >= 240)
                {
                    toggle = false;
                }
            }

            starsParticleSystem.SetParticles(particles, particles.Length, 0);
        }
    }
}
