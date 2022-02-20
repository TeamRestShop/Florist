using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Matching : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playerNumText;
    private byte maxPlayers = 4;

    // Start is called before the first frame update
    void Start()
    {
        playerNumText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {maxPlayers}";

        // if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
        {
            // PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("ObjectScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }
 
    public override void OnLeftRoom() {
        PhotonNetwork.LoadLevel("LevelSelectScene");
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        playerNumText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {maxPlayers}";

        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("ObjectScene");
        }
    }
    public override void OnPlayerLeftRoom(Player player)
    {
        playerNumText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {maxPlayers}";

        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("ObjectScene");
        }
    }
}
