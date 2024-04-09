using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public Image Icon;
    public int number;
    public TextMeshProUGUI slotNumber;
    public TextMeshProUGUI quantity;
    private bool isHighlighted;
    public Image highlight;

    public void Refresh(Sprite sprite)
    {
        Icon.sprite = sprite;
        quantity.gameObject.SetActive(false);
        highlight.gameObject.SetActive(isHighlighted);
    }
    public void Init(Sprite sprite, int number)
    {
        Icon.sprite = sprite;
        this.number = number-1;
        slotNumber.text = number.ToString();
        quantity.gameObject.SetActive(false);
        highlight.gameObject.SetActive(false);
    }

    public void Refresh(Sprite sprite, int quantity)
    {
        Icon.sprite = sprite;
        this.quantity.gameObject.SetActive(false);
        this.quantity.text = quantity.ToString();
        highlight.gameObject.SetActive(isHighlighted);
    }

    public void SetHighlighted()
    {
        isHighlighted = true;
    }

    public void SetUnhighlighted()
    {
        isHighlighted = false;
    }

    public void Equip()
    {
        CharacterControl.Instance.EquipHotbarSlot(number);
    }
}
