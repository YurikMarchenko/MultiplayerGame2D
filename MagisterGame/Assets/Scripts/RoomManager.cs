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
            // Отримуємо максимальну кількість гравців в кімнаті
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
                
                // Виводимо повідомлення у консоль
                Debug.Log("Кімната закрита.");
            }
        }
    }
}
