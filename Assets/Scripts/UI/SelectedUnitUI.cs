using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectedUnitUI : MonoBehaviour
{
    [Header("Parents")]
    [SerializeField]
    private GameObject layout;
    [SerializeField]
    private GameObject unitIcons;
    [SerializeField]
    private GameObject actionIcons;

    [Header("Direct Dynamics")]
    [SerializeField]
    private TMP_Text unitTitleText;
    [SerializeField]
    private TMP_Text unitDescriptionText;

    [Header("Prefabs")]
    [SerializeField]
    private UIIcons iconPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        layout.SetActive(false);
    }

}
