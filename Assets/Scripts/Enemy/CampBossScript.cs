using System;
using UnityEngine;

public class CampBossScript : MonoBehaviour
{
    [SerializeField] private BossType bossType;

    private void Awake()
    {
        if (ButtonHandler.profile.arminiusBeaten && bossType == BossType.arminius)
        {
            Destroy(gameObject);
        }
        
        if (ButtonHandler.profile.woodCampBeaten && bossType == BossType.wood)
        {
            Destroy(gameObject);
        }
        
        if (ButtonHandler.profile.copperCampBeaten && bossType == BossType.copper)
        {
            Destroy(gameObject);
        }
        
    }

    public void FallBig()
    {
        if (bossType == BossType.arminius)
        {
            ButtonHandler.profile.arminiusBeaten = true;
        }else if (bossType == BossType.wood)
        {
            ButtonHandler.profile.woodCampBeaten = true;
        }
        else
        {
            ButtonHandler.profile.copperCampBeaten = true;
        }
    }
}

enum BossType
{
    none, wood, copper, arminius
}
