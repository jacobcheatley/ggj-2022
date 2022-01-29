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
    private bool inited = false;

    private Dictionary<string, GameObject> iconMapping = new Dictionary<string, GameObject>();

    private void Init()
    {
        foreach (var icon in icons)
        {
            Debug.Log(icon.name);
            iconMapping.Add(icon.name, icon);
        }
        inited = true;
    }

    public GameObject GetIcon(string iconName)
    {
        if (!inited)
            Init();
        return iconMapping[iconName];
    }
}