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
        Debug.Log("Local player present");
        name = "localObject";

        canvas = GameObject.Find("Canvas");





    }
    void Update()
    {



        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log("Local player present");

        if (SceneManager.GetActiveScene().name == "Loading" && !isServer)
        {
            CmdConnected();
        }
        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }

        try
        {
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
            //if (GameObject.Find("answer 4").GetComponent<Toggle>().isOn)
            //{
            //    myAnswer = 4;
            //}
        }
        catch (System.Exception e)
        {

            //Debug.Log(e.Message);
        }

        //if (myAnswer == 0)
        //{
        //    GameObject.Find("Volgende").GetComponent<Button>().interactable = false;
        //}
        //else
        //{
        //    GameObject.Find("Volgende").GetComponent<Button>().interactable = true;
        //}











        if (!isServer)
        {
            CmdClientAntwoord(myAnswer.ToString());
        }
        else if (isServer)
        {
            GameObject.Find("EventSystem").GetComponent<QuestionData>().ServerAnswer = myAnswer.ToString();
        }



        if (sendAnswer == true)
        {
            //GameObject.Find("Result1").GetComponent<Text>().text = "send answer true";
            if (!isServer)
            {
                CmdSendAnswer();
            }
            else if (isServer)
            {
                GameObject.Find("EventSystem").GetComponent<QuestionData>().ServerSend = true;
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
        GameObject.Find("EventSystem").GetComponent<QuestionData>().ClientAnswer = antwoord;
        Debug.Log("Client message: " + antwoord);
    }

    [Command]
    void CmdSendAnswer()
    {
        GameObject.Find("EventSystem").GetComponent<QuestionData>().ClientSend = true;
    }


    [Command]

    void CmdConnected()
    {
        Debug.Log("Starting game...");
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("Vraag");
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player ");
    }

}
