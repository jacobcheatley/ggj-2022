using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractableAfterTextInput : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetInteractible(string text)
    {
        if (text.Length > 0)
            button.interactable = true;
        else
            button.interactable = false;
    }

    public void SetInteractible()
    {
        button.interactable = true;
    }
}
