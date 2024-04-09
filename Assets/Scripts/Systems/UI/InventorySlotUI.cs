using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image Icon;
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
        slotNumber.text = number.ToString();
        quantity.gameObject.SetActive(false);
        highlight.gameObject.SetActive(false);
    }

    public void Refresh(Sprite sprite, int amount)
    {
        Icon.sprite = sprite;
        quantity.gameObject.SetActive(true);
        quantity.text = amount.ToString();
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
}
