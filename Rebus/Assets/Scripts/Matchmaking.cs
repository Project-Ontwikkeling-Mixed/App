using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Matchmaking : MonoBehaviour {

	NetworkManager nw;

	// Use this for initialization
	void Start () {
		nw = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LANMatch()
	{
		nw.StartHost();
	}

	public void LANJoin()
	{
		nw.StartClient();
	}

	public void EnableOnline()
	{
		nw.StartMatchMaker();
	}

	void OnPlayerConnected ()
	{
		Debug.Log("Joined");
	}
}
