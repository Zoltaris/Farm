using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Item_SO", menuName = "ScriptableObjects/Items/Item", order = 4)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public int stackLimit;
    public bool emptySlot;
    public Mesh itemMesh;
    public Item itemScript;
    public Sprite icon;
}
