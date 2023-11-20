using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public InputField inputName;
    public InputField maxCountPlayers;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        inputName.text = PlayerPrefs.GetString("name");
        PhotonNetwork.NickName = inputName.text;
    }
    public void CreateRoom()
    {
        int countPlayers;
        if (int.TryParse(maxCountPlayers.text, out countPlayers))
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = (byte)countPlayers,
                IsVisible = true,
                IsOpen = true
            };

            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            Debug.LogError("Invalid input for maxCountPlayers");
        }
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void SaveName()
    {
        PlayerPrefs.SetString("name", inputName.text);
        PhotonNetwork.NickName = inputName.text;
    }
}
