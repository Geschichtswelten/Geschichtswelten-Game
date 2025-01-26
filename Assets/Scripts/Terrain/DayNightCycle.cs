using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Material skyboxDay;
    [SerializeField] private Material skyboxNight;
    [SerializeField] private Transform sunTransform;
    [SerializeField] private Transform moonTransform;
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;
    [SerializeField] private float angleAtNoon;
    [SerializeField] private Transform starTransform;
    [SerializeField] private Vector3 hmsStarLight = new Vector3(19f, 30f, 0), hmsStarsExtinguish = new Vector3(03f, 30f, 0f);
    [SerializeField] private float starsFadeInTime = 7200f, starsFadeOutTime = 7200f;
    [SerializeField] private Vector3 hourMinuteSecond = new Vector3(6f, 0f, 0f), hasSunSet = new Vector3(18f, 0f, 0f);
    public int days = 0;
    [SerializeField] private float speed = 100;
    [SerializeField] private float intensityAtNoon = 1f, intensityAtSunSet = 0.5f;
    [SerializeField] private Color fogColorDay = Color.grey, fogColorNight = Color.black;
    [NonSerialized] public float time;
    public Vector2 timeOfDay = Vector2.zero;

    private float intensity, rotation, prev_rotation = -1f, sunSet, sunRise, sunDayRatio, moon_rotation, prev_moon_rotation = -1f, timeLight, timeExtinguish;
    private Vector3 dir;
    private int act = 0;
    private Color tintColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Renderer starRenderer;
    [SerializeField] private float fade;
    public ScatterStars scatterStars;
    public List<List<Item>> items = new List<List<Item>>();
    public StorageInventory[] inventorys;
    void Start()
    {
        for (int i = 0; i< inventorys.Length; i++)
        {
            items[i] = inventorys[i].storageItems;
        }
        
        
        
        time = ButtonHandler.profile.time;
        sunSet = HMS_to_Time(hasSunSet.x, hasSunSet.y, hasSunSet.z);
        days = ButtonHandler.profile.day;
        sunRise = 86400f - sunSet;
        if (time >= sunRise && time < sunSet)
        {
            RenderSettings.skybox = skyboxDay;
            RenderSettings.sun = sun;
            moon.gameObject.SetActive(false);
        }
        else
        {
            RenderSettings.skybox = skyboxNight;
            RenderSettings.sun = moon;
            sun.gameObject.SetActive(false);
        }
        sunDayRatio = (sunSet - sunRise) / 43200f;
        dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angleAtNoon), Mathf.Sin(Mathf.Deg2Rad * angleAtNoon), 0f);
        starsFadeInTime /= speed;
        starsFadeOutTime /= speed;
        fade = 0;
        timeLight = HMS_to_Time(hmsStarLight.x, hmsStarLight.y, hmsStarLight.z);
        timeExtinguish = HMS_to_Time(hmsStarsExtinguish.x, hmsStarsExtinguish.y, hmsStarsExtinguish.z);
        starRenderer = GetComponent<ParticleSystem>().GetComponent<Renderer>();
    }

    private void Update()
    {
        time += Time.deltaTime * speed;
        UpdateTimeOfDay(time);
        if (time > 86400f)
        {
            days += 1;
            time -= 86400f;
            for (int i = 0; i< inventorys.Length; i++)
            {
                inventorys[i].storageItems = items[i];
            }
        }

        if (prev_moon_rotation == -1f)
        {
            moonTransform.eulerAngles = Vector3.zero;
            prev_moon_rotation = 0f;
        }
        else prev_moon_rotation = moon_rotation;


        if (prev_rotation == -1f)
        {
            sunTransform.eulerAngles = Vector3.zero;
            prev_rotation = 0f;
        }
        else prev_rotation = rotation;

        rotation = (time - 21600f) / 86400f * 360f;
        sunTransform.Rotate(dir, rotation - prev_rotation);

        moon_rotation = ((time + 43200f) - 21600f) / 86400f * 360f;
        moonTransform.Rotate(dir, moon_rotation - prev_moon_rotation);

        starTransform.Rotate(dir, rotation - prev_rotation);

        if (time < sunRise)
        {
            scatterStars.toggle = true;
            intensity = intensityAtSunSet * time / sunRise;
            if (act == 1 && time >= 18000f)
            {
                act = 0;
                sun.gameObject.SetActive(true);
                moon.gameObject.SetActive(false);
                RenderSettings.skybox = skyboxDay;
                RenderSettings.sun = sun;
                
            }
            
        }
        else if (time < 43200f)
        {

            intensity = intensityAtSunSet +
                        (intensityAtNoon - intensityAtSunSet) * (time - sunRise) / (43200f - sunRise);
        }
        else if (time < sunSet)
            intensity = intensityAtNoon - (intensityAtNoon - intensityAtSunSet) * (time - 43200f) / (sunSet - 43200f);
        else
        {
            scatterStars.toggle = true;
            intensity = intensityAtSunSet - (1f - intensityAtSunSet) * (time - sunSet) / (86400f - sunSet);
            if (act == 0 && time >= 68820f)
            {
                act = 1;
                sun.gameObject.SetActive(false);
                moon.gameObject.SetActive(true);
                RenderSettings.skybox = skyboxNight;
                RenderSettings.sun = moon;
            }
        }

        RenderSettings.fogColor = Color.Lerp(fogColorNight, fogColorDay, intensity * intensity);
        if (sun != null) sun.intensity = intensity;

        if (Time_Falls_Between(time, timeLight, timeExtinguish))
        {
            fade += Time.deltaTime / starsFadeInTime;
            if (fade > 1f) fade = 1f;
        }
        else
        {
            fade -= Time.deltaTime / starsFadeOutTime;
            if (fade < 0f) fade = 0f;
        }

        tintColor.a = fade;
        starRenderer.material.SetColor("_TintColor", tintColor);
    }

    private float HMS_to_Time(float hour, float minute, float second)
    {
        return 3600 * hour + 60 * minute + second;
    }

    private void UpdateTimeOfDay(float time)
    {
        timeOfDay.x = Mathf.Floor(time / 3600);
        float y = Mathf.Floor(time % 3600);
        timeOfDay.y = MathF.Floor(y / 60);
    }

    private bool Time_Falls_Between(float currentTime, float startTime, float endTime)
    {
        if (startTime < endTime)
        {
            if (currentTime >= startTime && currentTime <= endTime) return true;
            else return false;

        }
        else
        {
            if (currentTime > startTime || currentTime < endTime) return true;
            else return false;
        }
    }




}
