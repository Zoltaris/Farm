using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant_SO", menuName = "ScriptableObjects/Items/Plant", order = 1)]
public class PlantScriptableObject : ScriptableObject
{
    public int stages;
    public bool multipleHarvests;
    public int daysToGrow;
    public int daysBetweenHarvest;
    public bool needToolToHarvest;
}
