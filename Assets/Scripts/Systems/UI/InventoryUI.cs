using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
   public static InventoryUI instance;
   public GridLayoutGroup inventoryGrid;

   private void Awake()
   {
      instance = this;
   }
}
