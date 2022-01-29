using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEditor;

[CreateAssetMenu(menuName = "Custom/UIIcons")]
public class UIIcons : ScriptableObject
{
    [SerializeField]
    private GameObject[] icons;

    private Dictionary<string, GameObject> iconMapping;

    private void Awake()
    {
        foreach (var icon in icons)
            iconMapping.Add(icon.name, icon);
    }

    public GameObject GetIcon(string iconName)
    {
        return iconMapping[iconName];
    }
}