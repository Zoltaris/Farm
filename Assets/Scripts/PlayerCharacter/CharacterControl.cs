using System;
using System.Collections;
using System.Collections.Generic;
using Systems;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private CharacterController _cc;
    public static CharacterControl Instance;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private float _playerSpeed = 2.0f;
    public InteractibleDetector _id;
    public IInteractable _currentInteractable{ set; private get; }
    private IHoldable Item;
    public List<Tool> tools = new List<Tool>(3);

    private Inventory inventory;
    public ItemScriptableObject item;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _id._cc = this;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        MovementCalculator();
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if( Item is ITool tool )
            {
                tool.Use(_currentInteractable);
            }
            else
            {
                _currentInteractable.Interact();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.OpenInventory();
        }
        

        for (int key = (int)KeyCode.Alpha1; key <= (int)KeyCode.Alpha9; key++)
        {
            if(Input.GetKeyDown((KeyCode)key))
            {
                int index = (key - (int)KeyCode.Alpha1);
                EquipHotbarSlot(index);
            }
        }

    }

    public void EquipHotbarSlot(int slotNumber)
    {
        Inventory.InventorySlot slot = inventory[slotNumber];
        Inventory.Instance.SetEquippedSlot(slotNumber);
        EquipScript(slot.ItemSO);
    }

    private bool EquipScript(ItemScriptableObject equippingItem)
    {
        Item?.Unequip();
        Item = (IHoldable)equippingItem.itemScript;
        if (equippingItem.emptySlot)
            return false;
        Item.Equip();
        return Item == equippingItem.itemScript;
    }

    private void MovementCalculator()
    {
        _groundedPlayer = _cc.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0f)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3
        {
            y = 0f
        };
        if (Input.GetKey(KeyCode.A))
        {
            move.x = _playerSpeed * -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.x = _playerSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            move.z = _playerSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.z = _playerSpeed * -1;
        }

        _cc.Move(move * Time.deltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        
        _cc.Move(_playerVelocity * Time.deltaTime);
    }


}
