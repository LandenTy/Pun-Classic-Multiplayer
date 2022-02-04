using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviour {

	public static Launcher Instance;

	[SerializeField] Text roomNameText;
	[SerializeField] Text errorText;
	[SerializeField] Text enteringText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;

	public RoomListItem roomListItem;

	private string _gameVersion = "v0.0.1";

	void Awake()
	{
		Instance = this;

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
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = 6;

		PhotonNetwork.CreateRoom ("");
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

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom ();
		MenuManager.Instance.OpenMenu ("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom (info.Name);
		MenuManager.Instance.OpenMenu ("loading");
	}

	public void OnRoomListUpdate(List<RoomInfo>  roomList)
	{
		foreach (Transform trans in roomListContent) 
		{
			Destroy (trans.gameObject);
		}

		for (int i = 0; i < roomList.Count; i++) {
			Instantiate (roomListItemPrefab);
			Instantiate (roomListContent);
			roomListItem.SetUp (roomList[i]);
		}
	}
}
