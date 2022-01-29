using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoIconUI : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text text;

    public InfoIconUI Init(Sprite sprite, string value)
    {
        image.sprite = sprite;
        text.text = value;
        return this;
    }
}
