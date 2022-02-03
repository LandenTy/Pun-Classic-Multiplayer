using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviour {

	[SerializeField] InputField roomNameInputField;
	[SerializeField] Text roomNameText;
	[SerializeField] Text errorText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;

	private string _gameVersion = "v0.0.1";

	void Awake()
	{
		//Lets us use PhotonNetwork.LoadLevel() to properly connect/disconnect clients to the server
		PhotonNetwork.automaticallySyncScene = true;
	}

	void Start()
	{
		Debug.Log("Connecting to Master");
		PhotonNetwork.ConnectUsingSettings (_gameVersion);
	}

	public void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
	}

	public void OnJoinedLobby()
	{
		MenuManager.Instance.OpenMenu ("title");
		Debug.Log ("Joined Lobby");
	}

	public void CreateRoom()
	{
		if (string.IsNullOrEmpty (roomNameInputField.text)) 
		{
			return;
		}

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = 6;

		PhotonNetwork.CreateRoom (roomNameInputField.text);
		MenuManager.Instance.OpenMenu ("loading");
	}

	public void OnJoinedRoom()
	{
		MenuManager.Instance.OpenMenu ("room");
		roomNameText.text = PhotonNetwork.room.Name;
	}

	public void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		MenuManager.Instance.OpenMenu ("error");
	}

	public void OnRoomListUpdate(List<RoomInfo>  roomList)
	{

	}
}
