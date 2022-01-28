using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(RectTransform))]
class LobbyUI : MonoBehaviour
{
    [Header("Dynamics")]
    [SerializeField]
    private GameObject initialUI;
    [SerializeField]
    private GameObject createUI;
    [SerializeField]
    private GameObject joinUI;

    [Header("UI Controls")]
    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private TMP_InputField codeInput;
    [SerializeField]
    private TMP_Text codeOutput;

    private void Start()
    {
        initialUI.SetActive(true);
        createUI.SetActive(false);
        joinUI.SetActive(false);
    }

    public void InitialCreateButton()
    {
        string playerName = nameInput.text;
        initialUI.SetActive(false);
        createUI.SetActive(true);

        NetworkManager.instance.OnCodeMessage += Instance_OnCodeMessage;
        NetworkManager.instance.Create(playerName);
    }

    public void InitialJoinButton()
    {
        initialUI.SetActive(false);
        joinUI.SetActive(true);
    }

    public void JoinButton()
    {
        string playerName = nameInput.text;
        string code = codeInput.text;
        joinUI.SetActive(false);

        NetworkManager.instance.OnConnectMessage += Instance_OnConnectMessage_Join;
        NetworkManager.instance.Join(playerName, code);
    }

    private void Instance_OnCodeMessage(CodeMessage message)
    {
        Debug.Log("EXCECUTED");
        NetworkManager.instance.OnCodeMessage -= Instance_OnCodeMessage;
        codeOutput.text = $"Code: {message.code}";
        NetworkManager.instance.OnConnectMessage += Instance_OnConnectMessage_Create;
    }

    private void Instance_OnConnectMessage_Create(ConnectMessage message)
    {
        // TODO
        Debug.Log($"Connect (from Create) {message.name}");
    }

    private void Instance_OnConnectMessage_Join(ConnectMessage message)
    {
        // TODO
        Debug.Log($"Connect (from Join) {message.name}");
    }
}