using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    public bool sendAnswer = false;
    GameObject canvas;

    




    int myAnswer;




    //antwoorden


    void Start()
    {
        

        if (!isLocalPlayer)
        {
            return;
        }
        name = "localObject";

        canvas = GameObject.Find("Canvas");


    }
    void Update()
    {



        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (GameObject.Find("answer 1").GetComponent<Toggle>().isOn)
        {
            myAnswer = 1;
        }
        else if (GameObject.Find("answer 2").GetComponent<Toggle>().isOn)
        {
            myAnswer = 2;
        }
        else if (GameObject.Find("answer 3").GetComponent<Toggle>().isOn)
        {
            myAnswer = 3;
        }
        else
        {
            myAnswer = 0;
        }










        if (!isServer)
        {
            CmdClientAntwoord(myAnswer.ToString());
        }
        else if (isServer)
        {
            GameObject.Find("EventSystem").GetComponent<AntwoordGegevens>().serverAntwoord = myAnswer.ToString();
        }



        if (sendAnswer == true)
        {
            if (!isServer)
            {
                CmdSendAnswer();
            }
            else if (isServer)
            {
                GameObject.Find("EventSystem").GetComponent<AntwoordGegevens>().serverSend = true;
            }
        }


    }

    public void validateAnswer()
    {
        sendAnswer = true;
    }

    [Command]

    void CmdClientAntwoord(string antwoord)
    {
        GameObject.Find("EventSystem").GetComponent<AntwoordGegevens>().clientAntwoord = antwoord;
        Debug.Log("Client message: " + antwoord);
    }

    [Command]
    void CmdSendAnswer()
    {
        GameObject.Find("EventSystem").GetComponent<AntwoordGegevens>().clientSend = true;
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player ");
    }

}
