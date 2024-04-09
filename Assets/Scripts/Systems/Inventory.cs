using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
   public class Inventory : MonoBehaviour
   {
 
      private InventorySlot[] _inventory = new InventorySlot[36];
      [FoldoutGroup("Reference Objects")]
      public ItemScriptableObject EmptySlotSO;
      [FoldoutGroup("Reference Objects")]
      public static InventorySlot EmptySlot { get; private set; }
      public static Inventory Instance { get; private set; }
   
   
      private Inventory inventory;
      [FoldoutGroup("Reference Objects")]
      public ItemScriptableObject item;
      [FoldoutGroup("Reference Objects")]
      public GameObject heldInHands;
      [FoldoutGroup("Reference Objects")]
      public Vector3 HandPosition { get; private set; }
      [FoldoutGroup("Reference Objects")]
      public GameObject backpack;
      [FoldoutGroup("Reference Objects")]
      public Vector3 BackpackPosition { get; private set; }
      [FoldoutGroup("Reference Objects")]
      public MeshFilter BackpackObject1;
      [FoldoutGroup("Reference Objects")]
      public MeshFilter BackpackObject2;
      public bool Object1;
      public bool itemEquipped;
      private int equippedSlot;

      public List<Tool> tools = new List<Tool>();
      [FoldoutGroup("Reference Objects")]
      public Hotbar hotbar;
   

      public InventoryUI UI;
      [FoldoutGroup("Hotbar")]
      public HotbarSlot hotbarSlotPrefab;
      [FoldoutGroup("Hotbar")]
      private HotbarSlot[] HotbarSlots = new HotbarSlot[9];
      [FoldoutGroup("Hotbar")]
      private Image highlight;
      [FoldoutGroup("Hotbar")]
      private HorizontalLayoutGroup HotbarGroup;
      [FoldoutGroup("Hotbar")]
      private bool hotbarInitialisied;
   
      [FoldoutGroup("Inventory")]
      public InventorySlotUI slotPrefab;
      [FoldoutGroup("Inventory")]
      public GridLayoutGroup InventoryGroup;
      [FoldoutGroup("Inventory")]
      public InventorySlotUI[] InventorySlots = new InventorySlotUI[36];
      [FoldoutGroup("Inventory")]
      public bool initialised;
      [FoldoutGroup("Inventory")]
      private bool _movingItem;
      [FoldoutGroup("Inventory")]
      private int _originalSlot;
      [FoldoutGroup("Inventory")]
      private int _stackQuantity;
      [FoldoutGroup("Inventory")]
      private ItemScriptableObject _movingSO;

      private void Awake()
      {
         Instance = this;
         EmptySlot = new InventorySlot(EmptySlotSO);
         for (var i = 0; i < _inventory.Length; i++)
         {
            InventorySlot temp = EmptySlot;
            if (i > 18)
            {
               temp.Unlocked = false;
            }
            _inventory[i] = temp;
          
         }
      }

      private void Update()
      {
         HandPosition = heldInHands.transform.position;
         BackpackPosition = backpack.transform.position;
         if (itemEquipped)
         {
            if (Object1)
            {
               BackpackObject1.transform.position = HandPosition;
            }
            else
            {
               BackpackObject2.transform.position = HandPosition;
            }
         }
         else
         {
            BackpackObject1.transform.position = BackpackPosition;
            BackpackObject2.transform.position = BackpackPosition;
         }

         if (Input.GetKeyDown(KeyCode.P))
         {
            foreach (var tool in tools)
            {
               AddItemToInventory(tool.toolSO, 1, out int unused);
            }
         }
         UpdateHotbar();
      }

      public struct InventorySlot
      {
         public int StackSize;
         public ItemScriptableObject ItemSO;
         public bool Unlocked;
         public bool IsEmptySlot => ItemSO.emptySlot;
         public bool Is( ItemScriptableObject item ) => ItemSO == item;

         public InventorySlot(ItemScriptableObject item)
         {
            ItemSO = item;
            StackSize = 0;
            Unlocked = true;
         }

         public InventorySlot(ItemScriptableObject item, int amount, bool free = true)
         {
            ItemSO = item;
            StackSize = amount;
            Unlocked = free;
         }

         public void AddToStack(ItemScriptableObject item, int amount, out int amountRemaining)
         {
            amountRemaining = amount;
            ItemSO = item;
            if (StackSize + amountRemaining <= item.stackLimit)
            {
               StackSize += amountRemaining;
               amountRemaining = 0;
            }
            else
            {
               int amountMoved = ItemSO.stackLimit - StackSize;
               StackSize = StackSize + (amountMoved);
               amountRemaining -= amountMoved;
            }
         }


         public bool RemoveFromStack(int amount)
         {
            if (StackSize - amount > 0)
            {
               StackSize -= amount;
               return true;
            }
            else if (StackSize - amount == 0)
            {
               ItemSO = Inventory.EmptySlot.ItemSO;
               StackSize = 0;
               return true;
            }
            return false;
         }
      }

      public bool AddItemToInventory(ItemScriptableObject item, int amount, out int leftover)
      {
         leftover = amount;
         int emptySlotIndex = -1;
      
         for (var i = 0; i < _inventory.Length; i++)
         {
            InventorySlot slot = _inventory[i];
            if (slot.ItemSO == item)
            {
               slot.AddToStack(item, leftover, out leftover);

               _inventory[i] = slot;
               if(leftover <= 0)
               {
                  return true;
               }

            }
            else if (emptySlotIndex == -1 && slot.ItemSO.emptySlot)
            {
               emptySlotIndex = i;
            }
         }

         if (emptySlotIndex == -1)
         {
            return false;
         }
         else
         {
            InventorySlot slot = _inventory[emptySlotIndex];
            slot.AddToStack(item, leftover, out leftover);
            _inventory[emptySlotIndex] = slot;
         }
         UpdateInventory(_inventory);
         if (leftover <= 0)
         {
            return true;
         }

         return AddItemToInventory(item, leftover, out leftover);
      }

      public bool TryRemoveItemFromInventory(ItemScriptableObject item, int amount)
      {
         int currentAmount = 0;
         System.Span<int> selectedSlots = stackalloc int[_inventory.Length];
         int numOfSlots = 0;
         for (var i = 0; i < _inventory.Length; i++)
         {
            var slot = _inventory[i];
            if (slot.ItemSO == item)
            {
               currentAmount += slot.StackSize;
               selectedSlots[numOfSlots++] = i;
               if (currentAmount >= amount)
               {
                  break;
               }
            }
         }
         if (currentAmount >= amount)
         {
            for (int x = 0; x < numOfSlots - 1; x++)
            {
               _inventory[selectedSlots[x]] = Inventory.EmptySlot;
            }
            InventorySlot slot = _inventory[selectedSlots[numOfSlots-1]];
            int leftOver = currentAmount - amount;
            slot.RemoveFromStack(slot.StackSize - leftOver);
            _inventory[selectedSlots[numOfSlots-1]] = slot;
            return true;
         }
         return false;
      }

      public bool CheckIfInInventory(ItemScriptableObject itemSO)
      {
         for (var i = 0; i < _inventory.Length; i++)
         {
            InventorySlot slot = _inventory[i];
            if (slot.ItemSO == itemSO)
            {
               return true;

            }
         }
         return false;
      }

      public bool RemoveItemFromSlot(int slot, out ItemScriptableObject itm, out int stack)
      {
         if (CheckIfSlotFree(slot))
         {
            itm = null;
            stack = 0;
            return false;
         }
         InventorySlot temp = _inventory[slot];
         itm = temp.ItemSO;
         stack = temp.StackSize;
         return true;
      }

      public bool AddItemToSlot(int slot, ItemScriptableObject itm, int stack, out int remainder)
      {
         InventorySlot temp = _inventory[slot];
         if (temp.IsEmptySlot)
         {
            temp.ItemSO = itm;
            temp.StackSize = stack;
            _inventory[slot] = temp;
            remainder = 0;
            return true;
         }
         else if (temp.Is(item))
         {
            if (temp.StackSize + stack <= itm.stackLimit)
            {
               temp.StackSize += stack;
               _inventory[slot] = temp;
               remainder = 0;
               return true;
            }
            else
            {
               remainder = itm.stackLimit - temp.StackSize;
               temp.StackSize = itm.stackLimit;
               _inventory[slot] = temp;
               return true;
            }
         }

         remainder = stack;
         return false;
      }

      public bool CheckIfSlotFree(int slotNumber)
      {
         return _inventory[slotNumber].ItemSO.emptySlot;
      }

      public InventorySlot this[int i] => _inventory[i];

      public void SetEquippedSlot(int i)
      {
         equippedSlot = i;
      }

      private void InitialiseHotbar()
      {
         HotbarGroup = Hotbar.instance.HotbarGroup;
         for (var i = 0; i < 9; i++)
         {
            HotbarSlot temp = Instantiate(hotbarSlotPrefab, HotbarGroup.transform);
            temp.Init(Inventory.EmptySlot.ItemSO.icon,i+1);
            HotbarSlots[i] = temp;
         }
         hotbarInitialisied = true;
      }

      private void UpdateHotbar()
      {
         if (!hotbarInitialisied)
         {
            InitialiseHotbar();
         }
         for (int i = 0; i < 9; i++)
         {
            HotbarSlot temp = HotbarSlots[i];
            InventorySlot tempSlot = _inventory[i];
            if (i == equippedSlot)
            {
               temp.SetHighlighted();
            }
            else
            {
               temp.SetUnhighlighted();
            }
            switch (tempSlot.ItemSO.stackLimit)
            {
               case > 1:
                  temp.Refresh(tempSlot.ItemSO.icon, tempSlot.StackSize);
                  break;
               case <= 1:
                  temp.Refresh(tempSlot.ItemSO.icon);
                  break;
            }
         
            HotbarSlots[i] = temp;

         }
      }
   
      private void InitialiseInventory()
      {
         InventoryGroup = InventoryUI.instance.inventoryGrid;
         for (var i = 0; i < InventorySlots.Length; i++)
         {
            InventorySlotUI temp = Instantiate(slotPrefab, InventoryGroup.transform);
            temp.Init(Inventory.EmptySlot.ItemSO.icon,i+1);
            InventorySlots[i] = temp;
         }
         initialised = true;
      }

      public void UpdateInventory(InventorySlot[] inventory)
      {
         if (!initialised)
         {
            InitialiseInventory();
         }
         for (int i = 0; i < InventorySlots.Length; i++)
         {
            InventorySlotUI temp = InventorySlots[i];
            InventorySlot tempSlot = inventory[i];

            switch (tempSlot.ItemSO.stackLimit)
            {
               case > 1:
                  temp.Refresh(tempSlot.ItemSO.icon, tempSlot.StackSize);
                  break;
               case <= 1:
                  temp.Refresh(tempSlot.ItemSO.icon);
                  break;
            }

            if (i > 8)
            {
               temp.slotNumber.gameObject.SetActive(false);
            }

            InventorySlots[i] = temp;
         }
      }

      public void PickupItem(ItemScriptableObject item, int slot)
      {
         if (!_movingItem)
         {
            _movingItem = true;
            _movingSO = item;
            _originalSlot = slot;
         }
      }

      public void SetDownItem(ItemScriptableObject item, int slot)
      {
         Inventory inv = Inventory.Instance;
         if (inv.CheckIfSlotFree(slot))
         {
            //Call Add to slot here
         }
      }

      public void OpenInventory()
      {
         UI.gameObject.SetActive(true);
         hotbar.gameObject.SetActive(false);
         UpdateInventory(_inventory);
      }

      public void CloseInventory()
      {
         UI.gameObject.SetActive(false);
         hotbar.gameObject.SetActive(true);
      }

   
   
   }
}
