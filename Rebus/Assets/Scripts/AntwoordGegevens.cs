using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class AntwoordGegevens : NetworkBehaviour
{
    [SyncVar]
    public string serverAntwoord;
    [SyncVar]
    public string clientAntwoord;

    [SyncVar]
    public float timer = 60;

    public string mijnAntwoord;
    public string enemyAntwoord;

    [SyncVar]
    public bool clientSend;
    [SyncVar]
    public bool serverSend;


    public int answer;

    public void answer1()
    {
        GameObject.Find("answer 2").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 3").GetComponent<Toggle>().isOn = false;

    }
    public void answer2()
    {
        GameObject.Find("answer 1").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 3").GetComponent<Toggle>().isOn = false;
    }
    public void answer3()
    {
        GameObject.Find("answer 2").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 1").GetComponent<Toggle>().isOn = false;
    }




    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        


        if (isServer)
        {
            timer -= Time.deltaTime;
            mijnAntwoord = serverAntwoord;
            enemyAntwoord = clientAntwoord;
        }
        else
        {
            mijnAntwoord = clientAntwoord;
            enemyAntwoord = serverAntwoord;
        }

        GameObject.Find("Timer").GetComponent<Text>().text = Mathf.Round(timer) + " seconden";

        GameObject.Find("Result").GetComponent<Text>().text = "Mijn antwoord: " + mijnAntwoord + " vijand antwoord: " + enemyAntwoord;
        Debug.Log("Mijn antwoord: " + mijnAntwoord + " vijand antwoord: " + enemyAntwoord);


        if (clientSend && serverSend)
        {
            string filenaam;
            string sceneToLoad;
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                filenaam = "meningGegevens.txt";
                sceneToLoad = "vraag2";
            }
            else
            {
                filenaam = "Antwoordgegevens.txt";
                sceneToLoad = "Result";
            }
            Directory.CreateDirectory("gameData");
            StreamWriter outputStream = File.CreateText(Path.Combine("gameData\\", filenaam));
            outputStream.WriteLine(serverAntwoord);
            outputStream.WriteLine(mijnAntwoord);
            outputStream.Close();

            if (isServer)
            {
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("vraag2");
            }
        }








    }
}
