using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ServerRoomManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinRoom(int level)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.EmptyRoomTtl = 1;
        PhotonNetwork.JoinOrCreateRoom(level + "", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("ObjectScene");
    }
}
