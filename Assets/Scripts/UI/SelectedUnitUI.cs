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

    [Header("Info Icons")]
    [SerializeField]
    private GameObject infoIconPrefab;
    [SerializeField]
    private Sprite healthIcon;
    [SerializeField]
    private Sprite speedIcon;

    [Header("Action Icons")]
    [SerializeField]
    private GameObject actionIconPrefab;

    private InfoIconUI healthIconObject;
    private InfoIconUI speedIconObject;

    public static SelectedUnitUI instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // Top Icons
        healthIconObject = GameObject.Instantiate(infoIconPrefab, unitIcons.transform).GetComponent<InfoIconUI>().Init(healthIcon, "X");
        speedIconObject = GameObject.Instantiate(infoIconPrefab, unitIcons.transform).GetComponent<InfoIconUI>().Init(speedIcon, "X");
        layout.SetActive(false);
    }

    public void DisplayUnit(Unit unit)
    {
        layout.SetActive(true);


        // Text
        unitTitleText.text = unit.unitName;
        unitDescriptionText.text = unit.description;

        // Basic stats
        healthIconObject.Init(healthIcon, unit.currentHealth.ToString());
        speedIconObject.Init(speedIcon, unit.speed.ToString());

        // Action Icons
        // Clear
        void Clear(GameObject g)
        {
            foreach (Transform child in g.transform)
                GameObject.Destroy(child.gameObject);
        }
        Clear(actionIcons);
        //GameObject.Instantiate(iconPrefabs.GetIcon("ActionIconWalk"), actionIcons.transform);
        //GameObject.Instantiate(iconPrefabs.GetIcon("ActionIconWait"), actionIcons.transform);
        //foreach (IAction action in unit.Actions.actions)
        //    GameObject.Instantiate(iconPrefabs.GetIcon(action.icon), actionIcons.transform);
    }
}
