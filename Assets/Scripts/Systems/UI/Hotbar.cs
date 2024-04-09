using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public HorizontalLayoutGroup HotbarGroup;
    public static Hotbar instance;

    private void Awake()
    {
        instance = this;
    }
}