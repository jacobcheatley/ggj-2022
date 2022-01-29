using System;
using UnityEngine;
using TMPro;

public class HoverIconUI : MonoBehaviour
{
    [SerializeField]
    private Sprite rangeIcon;
    [SerializeField]
    private GameObject topIcons;
    [SerializeField]
    private GameObject infoIconPrefab;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text descriptionText;

    public void Init(string name, string description, int range, int value, Sprite valueIcon)
    {
        transform.localPosition = new Vector2(48, 48);
        nameText.text = name;
        descriptionText.text = description;
        GameObject.Instantiate(infoIconPrefab, topIcons.transform).GetComponent<InfoIconUI>().Init(rangeIcon, range.ToString());
        if (valueIcon != null)
            GameObject.Instantiate(infoIconPrefab, topIcons.transform).GetComponent<InfoIconUI>().Init(valueIcon, value.ToString());
    }
}