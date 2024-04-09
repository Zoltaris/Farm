using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.UIElements;

public class Tool : Item, ITool, IHoldable
{
    public ToolScriptableObject toolSO;
    public Inventory inventory;
    public Mesh ToolMesh;
    public Material ToolMaterial;
    

    public void Use(IInteractable interact)
    {
        interact.Interact(toolSO);
    }

    public void Equip()
    {
        if (inventory == null)
        {
            inventory = Inventory.Instance;
        }
        inventory.Object1 = !inventory.Object1;
        if (inventory.Object1)
        {
            inventory.BackpackObject1.sharedMesh = ToolMesh;
            var transform1 = inventory.BackpackObject1.transform;
            transform1.position = inventory.HandPosition;
            transform1.localScale = new Vector3 (1, 1, 1);
        }
        else
        {
            inventory.BackpackObject2.sharedMesh = ToolMesh;
            var transform1 = inventory.BackpackObject2.transform;
            transform1.position = inventory.HandPosition;
            transform1.localScale = new Vector3 (1, 1, 1);
        }

        inventory.itemEquipped = true;
    }

    public void Unequip()
    {
        if (inventory == null)
        {
            inventory = Inventory.Instance;
        }
        if (inventory.Object1)
        {
            inventory.BackpackObject1.transform.position = inventory.HandPosition;
            inventory.BackpackObject1.transform.localScale = new Vector3 (.1f, .1f, .1f);
        }
        else
        {
            inventory.BackpackObject2.transform.position = inventory.HandPosition;
            inventory.BackpackObject2.transform.localScale = new Vector3 (.1f, .1f, .1f);
        }

        inventory.itemEquipped = false;
        //gameObject.SetActive(false);
    }
    
}
