using UnityEngine;
using TMPro;

public class TurnUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text turnText;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private GameObject buttonObject;

    public void SetTurn(GridManager.Turn turn, GridManager.TimeOfDay time, string timeDescription)
    {
        if (turn == GridManager.Turn.Mine)
        {
            Debug.Log("Starting my turn UI");
            buttonObject.SetActive(true);
            timeText.text = timeDescription;
            turnText.text = "My Turn";
        }
        else
        {
            Debug.Log("Starting OPPONENT turn UI");
            buttonObject.SetActive(false);
            timeText.text = timeDescription;
            turnText.text = "Their Turn";
        }
    }
}
