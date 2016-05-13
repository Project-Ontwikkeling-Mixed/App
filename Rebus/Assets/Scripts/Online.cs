using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
public class Online : MonoBehaviour {

	NetworkManager nw;
	NetworkMatch nwMatch;

	bool matchCreated;

	// Use this for initialization
	void Start () {
		nw = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		nwMatch = gameObject.AddComponent<NetworkMatch>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	

	public void OnMatchCreate(CreateMatchResponse matchResponse)
	{
		if (matchResponse.success)
		{
			Debug.Log("Create match succeeded");
			matchCreated = true;
			Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
			NetworkServer.Listen(new MatchInfo(matchResponse), 9000);
		}
		else
		{
			Debug.LogError("Create match failed");
		}
	}

	public void OnMatchList(ListMatchResponse matchListResponse)
	{
		if (matchListResponse.success && matchListResponse.matches != null)
		{
			nwMatch.JoinMatch(matchListResponse.matches[0].networkId, "", OnMatchJoined);
		}
	}

	public void OnMatchJoined(JoinMatchResponse matchJoin)
	{
		if (matchJoin.success)
		{
			Debug.Log("Join match succeeded");
			if (matchCreated)
			{
				Debug.LogWarning("Match already set up, aborting...");
				return;
			}
			Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));
			NetworkClient myClient = new NetworkClient();
			myClient.RegisterHandler(MsgType.Connect, OnConnected);
			myClient.Connect(new MatchInfo(matchJoin));
		}
		else
		{
			Debug.LogError("Join match failed");
		}
	}

    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }


    public void CreateMatch()
    {
        CreateMatchRequest create = new CreateMatchRequest();
        create.name = "NewRoom";
        create.size = 2;
        create.advertise = true;
        create.password = "";

        nwMatch.CreateMatch(create, OnMatchCreate);
    }

	public void JoinMatch()
	{
        
        
        
	}

	public void Matchlist()
	{
        nwMatch.ListMatches(0, 20, "", OnMatchList);
	}



    public void play()
    {
        nw.StartMatchMaker();
        //serverSelectCanvas.SetActive (true);
        

        nw.matchMaker.ListMatches(0, 20, "", nw.OnMatchList);

        StartCoroutine(joinOrCreate());
    }

    public IEnumerator joinOrCreate()
    {
        //waits for response from list matches
        yield return new WaitForSeconds(5f);
        Debug.Log("Done waiting");
        if (nw.matchInfo == null)
        {
            Debug.Log("Join or create?");
            if (nw.matches.Count == 0)
            {
                Debug.Log("Create");
                nw.matchMaker.CreateMatch(nw.matchName, nw.matchSize, true, "", nw.OnMatchCreate);
            }
            else
            {
                Debug.Log("Joining");
                nw.matchMaker.JoinMatch(nw.matches[0].networkId, "", nw.OnMatchJoined);
            }
        }
    }
}
