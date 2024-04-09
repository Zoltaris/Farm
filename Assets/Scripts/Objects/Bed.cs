using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable, IInteractable
{
    public void Interact(ItemScriptableObject item = null)
    {
        DayManager dayManager = DayManager.Instance;
        dayManager.Sleep();
    }
}
