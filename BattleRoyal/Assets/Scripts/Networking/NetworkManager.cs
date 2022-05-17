using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private string gameVersion = "1";



    private void Awake() {
        instance = this;

        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.gameVersion;
    }

    public override void OnConnectedToMaster()
    {
        if(PhotonNetwork.IsConnected)
        {
            Debug.Log("We have connected");
        }
        else
        {
            Debug.Log("We are not connecte yet...");
        }
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("We have Joined Room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join due to " + message);
    }

    [PunRPC]
    public void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
