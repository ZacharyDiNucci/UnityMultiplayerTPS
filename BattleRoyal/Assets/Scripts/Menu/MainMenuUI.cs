using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField roomNameInput;

    [SerializeField]
    private TMP_InputField playerNameInput;

    [SerializeField]
    private NetworkManager networkManager;

    [SerializeField]
    private TMP_Text[] playerNames;

    [SerializeField]
    private GameObject[] playerIsReady;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private GameObject  createRoomButton, joinRoomButton;


    public override void OnConnected()
    {
        createRoomButton.SetActive(true);
        joinRoomButton.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        createRoomButton.SetActive(false);
        joinRoomButton.SetActive(false);
    }

    public void UpdateNameOnNetwork(TMP_InputField playerName)
    {
        PhotonNetwork.NickName = playerName.text;
    }

    public void CreateRoomButton()
    {
        if(roomNameInput.text.Length <= 0)
        {
            Debug.LogError("No Room Name Entered");
            return;
        }
        networkManager.CreateRoom(roomNameInput.text);
        playerNames[0].text = PhotonNetwork.NickName;
    }

    public void JoinRoomButton()
    {
        if(roomNameInput.text == null)
        {
            Debug.LogError("No Room Name Entered");
            return;
        }
        networkManager.JoinRoom(roomNameInput.text);

    }

    public override void OnJoinedRoom(){
        photonView.RPC("UpdatePlayerLobbyNameList", RpcTarget.All);

        Debug.Log("Joining room named " + roomNameInput.text);
        Debug.Log("Number of players in the room = " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    [PunRPC]
    private void UpdatePlayerLobbyNameList(){
        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerNames[i].text = "";
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerNames[i].text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public void LeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Left Room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerLobbyNameList();
    }


    public void toggleReady()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.LocalPlayer.NickName == playerNames[i].text)
            {
                photonView.RPC("isReady", Photon.Pun.RpcTarget.All, i);
            }
        }
    }

    [PunRPC]
    public void isReady(int playerSlot)
    {
        playerIsReady[playerSlot].SetActive(true);

        StartButtonAvailability();
    }

    private void StartButtonAvailability()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            int playersReady = 0;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(playerIsReady[i].activeInHierarchy)
                {
                    playersReady++;
                }
            }
            
            if(playersReady == PhotonNetwork.PlayerList.Length)
            {
                startButton.interactable = true;
            }
        }

    }
}
