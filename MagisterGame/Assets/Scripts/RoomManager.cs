using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int maxPlayersInRoom;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            // �������� ����������� ������� ������� � �����
            maxPlayersInRoom = PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    void Update()
    {
        
    }
    public void CountPlayerInRoom()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == maxPlayersInRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {              
                PhotonNetwork.CurrentRoom.IsOpen = false;
                
                // �������� ����������� � �������
                Debug.Log("ʳ����� �������.");
            }
        }
    }
}
