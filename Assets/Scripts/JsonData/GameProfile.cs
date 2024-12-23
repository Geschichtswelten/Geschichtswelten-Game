using System;
using System.Numerics;

[Serializable]
public class GameProfile
{
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    //1. index: item id ; 2. index: amount; 3. index: inventory Position
    public Vector3[] playerItems;
    public bool woodCampBeaten;
    public bool copperCampBeaten;
    public bool arminiusBeaten;

    //might want to save constructions
}
