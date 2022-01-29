using UnityEngine;
using TMPro;

public class InfoIconUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    public void Init(string value)
    {
        text.SetText(value);
    }
}
