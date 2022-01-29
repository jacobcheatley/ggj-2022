using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionIconUI : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject hoverPrefab;

    private GameObject hoverObject;

    private string actionName;
    private string description;
    private int range;
    private int value;
    private Sprite valueIcon;

    public void Init(Sprite icon, string name, string description, int range, Action p)
    {
        image.sprite = icon;
        button.onClick.AddListener(delegate { p(); });

        this.actionName = name;
        this.description = description;
        this.range = range;
        this.value = 0;
        this.valueIcon = null;
    }

    public void Init(IAction action, Action p)
    {
        image.sprite = action.icon;
        button.onClick.AddListener(delegate { p(); });

        this.actionName = action.actionName;
        this.description = action.description;
        this.range = action.range;
        this.value = action.value;
        this.valueIcon = action.valueIcon;
    }

    public void OnHover()
    {
        hoverObject = GameObject.Instantiate(hoverPrefab, transform);
        hoverObject.GetComponent<HoverIconUI>().Init(this.actionName, this.description, this.range, this.value, this.valueIcon);
    }

    public void OnUnhover()
    {
        Destroy(hoverObject);
    }
}