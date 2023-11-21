using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private int maxPlayersInRoom;
    private int ConnectedPlayersInRoom;
    public bool gameStarted = false;
    public Text gameStartText;
    
    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            maxPlayersInRoom = PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }
    void Update()
    {
        if (!gameStarted)
        {
            CountPlayerInRoom();
            gameStartText.text = "Waiting all players..." + "(" + ConnectedPlayersInRoom + "/" + maxPlayersInRoom + ")";
        }
        else
        {
            CheckLivePlayerInRoom();
            if (ConnectedPlayersInRoom > 1)
            {
                gameStartText.text = "Game started! Live players " + ConnectedPlayersInRoom;
            }
            else
            {
                Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
                gameStartText.text = "Winner - " + players[0].NickName;
            }
        }
    }
    public void CountPlayerInRoom()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        ConnectedPlayersInRoom = players.Length;

        if (players.Length == maxPlayersInRoom && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.AllBuffered);
        }
    }
    public void CheckLivePlayerInRoom()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        ConnectedPlayersInRoom = players.Length;
    }

    [PunRPC]
    void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        gameStarted = true;
    }

}
