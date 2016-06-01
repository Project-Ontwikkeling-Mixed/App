using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;

public class Loading : MonoBehaviour {
    public Image placeholder;
	public Sprite[] ImageArray;
    int counter = 0;
    float timer = 0.1f;
    float tempTime = 0;
    private int playerCount;



    string QuestionPath;
    string ResultPath;


    // Use this for initialization
    void Start () {
        QuestionData.getDataPath(out ResultPath, out QuestionPath);
        if (File.Exists(QuestionPath))
        {
            File.Delete(QuestionPath);
        }

        QuestionData.clearScore();

    }
	
	// Update is called once per frame
	void Update () {
        tempTime += Time.deltaTime;
        if (tempTime >= timer)
        {
            if (counter >= ImageArray.Length - 1)
            {
                counter = 0;
            }

            else
            {
                counter++;
            }

            placeholder.sprite = ImageArray[counter];
            tempTime = 0;
        }

        Debug.Log(Network.connections.Length);

        if(Network.connections.Length >= 1)
        {

            GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("Vraag");
        }
	
	}

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
    }
}
