using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool
{
    void Use(IInteractable interact);
}

public interface IInteractable
{
    //ToDo: Convert this to use ItemScriptableObject, not ToolScriptableObject
    void Interact(ItemScriptableObject item = null);
}

public interface IHoldable
{
    void Equip();

    void Unequip();
}

public interface IUI
{
    void Initialise();
}

