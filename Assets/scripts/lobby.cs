using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro; // TextMeshPro k�t�phanesini ekleyin
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class lobby : NetworkBehaviour
{
    [SerializeField] private Button hostButton, clientButton, readyHost, readyClient;
    public GameObject page1, page2, page3;
    [SerializeField] private TMP_InputField hostNameInput, hostpassInput, clientNameInput, clientPassInput; // TMP_InputField referanslar�

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            page1.SetActive(false);
            page3.SetActive(false);
        });

        clientButton.onClick.AddListener(() =>
        {
            page2.SetActive(false);
            page1.SetActive(false);
        });

        readyHost.onClick.AddListener(() =>
        {
            string userInput = hostNameInput.text; // InputField i�eri�ini al
            string passHost = hostpassInput.text; // InputField i�eri�ini al
            Debug.Log("Host Kullan�c� Giri�i: " + userInput); // Konsola yazd�r
            Debug.Log("Host �ifresi: " + passHost); // Konsola yazd�r
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(passHost);
            // Ba�lant� onaylama ayarlar�
            hostNameInput.interactable = false;
            hostpassInput.interactable = false;
            readyHost.interactable = false;
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        });

        readyClient.onClick.AddListener(() =>
        {
            string userInput = clientNameInput.text; // InputField i�eri�ini al
            string passClient = clientPassInput.text; // InputField i�eri�ini al
            Debug.Log("Client Kullan�c� Giri�i: " + userInput); // Konsola yazd�r
            Debug.Log("Client �ifresi: " + passClient); // Konsola yazd�r
            clientNameInput.interactable = false;
            clientPassInput.interactable = false;
            readyClient.interactable = false;
            // Oda �ifresini ayarlama
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(passClient);
            NetworkManager.Singleton.StartClient();
        });
    }

    private void Update()
    {
        if (IsServer)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count == 1)
            {
                NetworkManager.SceneManager.LoadScene("world", LoadSceneMode.Single);
            }
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        var connectionData = request.Payload;
        string clientPassword = System.Text.Encoding.ASCII.GetString(connectionData);

        // Host'un mevcut �ifresini almak i�in bir mekanizma ekleyin
        string hostPassword = System.Text.Encoding.ASCII.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData);

        // �ifre kontrol�
        if (clientPassword == hostPassword)
        {
            response.Approved = true;
            response.CreatePlayerObject = false; // Host bu se�ene�i ayarlayabilir
        }
        else
        {
            response.Approved = false;
            response.Reason = "Invalid room password.";
        }
    }
}
