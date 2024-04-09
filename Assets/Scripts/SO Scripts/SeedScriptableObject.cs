using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant_SO", menuName = "ScriptableObjects/Items/Seed", order = 2)]
public class SeedScriptableObject : ScriptableObject
{
    public Plant plant;
}
