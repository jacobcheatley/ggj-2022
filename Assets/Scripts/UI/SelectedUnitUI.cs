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

    public static SelectedUnitUI instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        layout.SetActive(false);
    }

    public void DisplayUnit(Unit unit)
    {
        layout.SetActive(true);

        // Clear
        void Clear(GameObject g)
        {
            foreach (Transform child in g.transform)
                GameObject.Destroy(child.gameObject);
        }
        Clear(unitIcons);
        Clear(actionIcons);

        // Text
        unitTitleText.text = unit.unitName;
        unitDescriptionText.text = unit.description;

        // Top Icons
        //GameObject.Instantiate(iconPrefabs.GetIcon("InfoIconHeart"), unitIcons.transform).GetComponent<InfoIconUI>().Init(unit.health.ToString());
        //GameObject.Instantiate(iconPrefabs.GetIcon("InfoIconSpeed"), unitIcons.transform).GetComponent<InfoIconUI>().Init(unit.speed.ToString());

        // Action Icons
        //GameObject.Instantiate(iconPrefabs.GetIcon("ActionIconWalk"), actionIcons.transform);
        //GameObject.Instantiate(iconPrefabs.GetIcon("ActionIconWait"), actionIcons.transform);
        //foreach (IAction action in unit.Actions.actions)
        //    GameObject.Instantiate(iconPrefabs.GetIcon(action.icon), actionIcons.transform);
    }
}
