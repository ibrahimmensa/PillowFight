using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public InputField CreateInput;
    public InputField JoinInput;
   // public Button CreateRoom;
    
    


    private void Update()
    {
       // Debug.Log(CreateInput);

    }
   public void CreateManualRoom()
    {
        if(CreateInput.text!=null)
        {
            RoomOptions newRoom = new RoomOptions() { MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(CreateInput.text, newRoom);//make input field
        }
        
    }
    public void JoinManualRoom()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);

    }
    public void CreateRandomRoom()
    {
        RoomOptions newRoom=new RoomOptions (){MaxPlayers=4 };
        //CreateInput = Random.Range(min, max);
        // PhotonNetwork.CreateRoom("4");
        // PhotonNetwork.CreateRoom("", newRoom);//make input field
      
        PhotonNetwork.CreateRoom(null, newRoom, TypedLobby.Default);
        //PhotonNetwork.JoinRandomOrCreateRoom();
        
    }

    public void JoinRandomRoom()
    {
        // 
        
        PhotonNetwork.JoinRandomRoom();
    }
    public void closeRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LoadingScreen 1");
    }
    
}
