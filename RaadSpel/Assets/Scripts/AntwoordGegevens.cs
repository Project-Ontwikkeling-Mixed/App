using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class AntwoordGegevens : NetworkBehaviour
{
    GameObject canvasMain;
    GameObject canvasVerzonden;

    //Antwoord in sync
    [SyncVar]
    public string serverAntwoord;
    [SyncVar]
    public string clientAntwoord;

    //timer
    [SyncVar]
    public float timer = 60;

    //lokaal antwoord
    public string mijnAntwoord;
    public string enemyAntwoord;

    //Check of op verzenden gedrukt
    [SyncVar]
    public bool clientSend = false;
    [SyncVar]
    public bool serverSend = false;


    public string[,] vragenLijst;


    const string vragenPath = "gameData\\vragenLijst.mixed";

    public int vraagNummer;


    void Start () {
        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }


        canvasMain = GameObject.Find("CanvasMain");
        canvasVerzonden = GameObject.Find("CanvasVerzonden");
        canvasVerzonden.SetActive(false);




        getVragenLijst();
        
	
	}

    void getVragenLijst()
    {
        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }
        if (!isServer)
        {
            return;
        }
        Debug.Log("Test");
        Directory.CreateDirectory("gameData");


        if (!File.Exists(vragenPath))
        {
            //vragenlijst opvragen bij vraag 1
            vraagNummer = 1;
            vragenLijst = new string[3, 8] { { "Vraag1","3", "antw1", "antw2", "antw3","17","26","88" }, { "vraag2","3", "antw2", "antw3", "antw1","44","12","2" }, { "vraag3","3", "antw3", "antw1", "antw2","56","1","4" } };
            StreamWriter outputStream = File.CreateText(vragenPath);
            outputStream.WriteLine(vraagNummer);
            outputStream.WriteLine(vragenLijst.GetLength(0));
            outputStream.WriteLine(vragenLijst.GetLength(1));
            for (int i = 0; i < vragenLijst.GetLength(0); i++)
            {
                for (int j = 0; j < vragenLijst.GetLength(1); j++)
                {
                    if (vragenLijst[i,j] != null)
                    {
                        outputStream.Write(vragenLijst[i, j] + " ");
                    }
                }
                outputStream.WriteLine();
            }
            outputStream.Close();
        }

        StreamReader inputStream = File.OpenText(vragenPath);
        if (SceneManager.GetActiveScene().name == "Vraag")
        {
            vraagNummer = int.Parse(inputStream.ReadLine());
        }
        else
        {
            vraagNummer = int.Parse(inputStream.ReadLine())-1;
        }
        int tempLength = int.Parse(inputStream.ReadLine());
        int tempWidth = int.Parse(inputStream.ReadLine());
        string[] tempVraagArr;
        vragenLijst = new string[tempLength, tempWidth];
        for (int i = 0; i < tempLength; i++)
        {
            tempVraagArr = inputStream.ReadLine().Split(' ');
            for (int j = 0; j < tempWidth; j++)
            {
                vragenLijst[i, j] = tempVraagArr[j];
            }
        }






        inputStream.Close();
        if (SceneManager.GetActiveScene().name == "Vraag")
        {
            File.Delete(vragenPath);
        }
        





        if (tempLength > vraagNummer && SceneManager.GetActiveScene().name == "Vraag")
        {
            StreamWriter outputStream2 = File.CreateText(vragenPath);
            outputStream2.WriteLine(vraagNummer + 1);
            outputStream2.WriteLine(tempLength);
            outputStream2.WriteLine(tempWidth);

            for (int i = 0; i < vragenLijst.GetLength(0); i++)
            {
                for (int j = 0; j < vragenLijst.GetLength(1); j++)
                {
                    if (vragenLijst[i, j] != null)
                    {
                        outputStream2.Write(vragenLijst[i, j] + " ");

                    }
                }
                outputStream2.WriteLine();
            }

            outputStream2.Close();
        }





    }

    [ClientRpc]

    void RpcVraag(string[,] vraag)
    {
        //vragenLijst = vraag;
    }

    // Update is called once per frame
    void Update () {
        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }

       



        if (isServer)
        {
           // RpcVraag(vragenLijst);
        }
        
        //Debug.Log(dezeVraag[0]);

        


        if (isServer)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            
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

        Debug.Log(serverSend + " " + clientSend);
        if (isServer && serverSend)
        {
            canvasVerzonden.SetActive(true);
        }
        if (!isServer && clientSend)
        {
            canvasVerzonden.SetActive(true);
        }


        if (clientSend && serverSend)
        {
        //    if (serverSend)
        //{
            string filenaam = "wrong scene.txt";
            string sceneToLoad;
            if (SceneManager.GetActiveScene().name == "Vraag")
            {
                filenaam = "meningGegevens.mixed";
                sceneToLoad = "vraag2";
            }
            else if (SceneManager.GetActiveScene().name == "Vraag2")
            {
                filenaam = "Antwoordgegevens.mixed";
                sceneToLoad = "Result";
            }
            Directory.CreateDirectory("gameData");
            StreamWriter outputStream = File.CreateText(Path.Combine("gameData\\", filenaam));
            outputStream.WriteLine(serverAntwoord);
            outputStream.WriteLine(clientAntwoord);
            outputStream.Close();

            if (isServer)
            {
                if (SceneManager.GetActiveScene().name == "Vraag")
                {
                    GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("Vraag2");
                }
                else if (SceneManager.GetActiveScene().name == "Vraag2")
                {
                    GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("Result");
                }
                
            }
        }
    }




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
}
