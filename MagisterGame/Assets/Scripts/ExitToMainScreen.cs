using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LeaveRoomScript : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
