using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class QuestionData : NetworkBehaviour
{
    GameObject CanvasMain;
    GameObject CanvasSend;
    
    //Antwoord in sync
    [SyncVar]
    public string ServerAnswer;
    [SyncVar]
    public string ClientAnswer;

    //timer
    [SyncVar]
    public float Timer = 60;

    //lokaal antwoord
    public string MyAnswer;
    public string EnemyAnswer;

    //Check of op verzenden gedrukt
    [SyncVar]
    public bool ClientSend = false;
    [SyncVar]
    public bool ServerSend = false;


    public string[,] QuestionList;
    public string[] ThisQuestion;


    string QuestionPath;
    string ResultPath;

    public int QuestionNr;



    WWW www;
    bool DataStringReceived = false;
    bool RequestSend = false;
    bool QuestionsReady = false;

    bool scoreAdded = false;


    void Start () {
        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }

        getDataPath(out ResultPath,out QuestionPath);
        //Debug.Log(QuestionPath);



        CanvasMain = GameObject.Find("CanvasMain");
        CanvasSend = GameObject.Find("CanvasVerzonden");
        CanvasSend.SetActive(false);




        
        
	
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
        
        //Directory.CreateDirectory("gameData");
        //Debug.Log(vragenPath);
        //GameObject.Find("Result").GetComponent<Text>().text = vragenPath;

        if (!File.Exists(QuestionPath))

        {
            clearScore();
            Debug.Log("File doenst exist");
            //GameObject.Find("Result").GetComponent<Text>().text = "path doesnt exist";

            //vragenlijst opvragen bij vraag 1
            


            if (!RequestSend)
            {
                
                www = new WWW("http://www.mixed.multimediatechnology.be/spel/genereervragen/5");
                StartCoroutine(WaitForRequest(www));
                RequestSend = true;
            }

            if (DataStringReceived)
            {
                //var vraagTest = JsonUtility.FromJson<vragenlijst>(www.text);
                var vraagTest = JsonUtility.FromJson<QuestionList>(www.text);
                //Debug.Log(vraagTest.vragen[2].vraag);
                QuestionList = new string[vraagTest.vragen.Count,15];
                for (int i = 0; i < vraagTest.vragen.Count; i++)
                {
                    QuestionList[i, 0] = vraagTest.vragen[i].vraag;
                    QuestionList[i, 1] = vraagTest.vragen[i].antwoorden.Count.ToString();
                    for (int j = 0; j < vraagTest.vragen[i].antwoorden.Count; j++)
                    {
                        QuestionList[i, j + 2] = vraagTest.vragen[i].antwoorden[j].antwoord;
                    }
                    for (int j = 0; j < vraagTest.vragen[i].antwoorden.Count; j++)
                    {
                        //percent berekenen
                        float tempTotaal = 0;
                        float tempPercent;
                        for (int p = 0; p < vraagTest.vragen[i].antwoorden.Count; p++)
                        {
                            tempTotaal += vraagTest.vragen[i].antwoorden[p].aantal_gekozen;
                        }
                        tempPercent= vraagTest.vragen[i].antwoorden[j].aantal_gekozen / tempTotaal * 100f;
                        tempPercent = (float)Math.Round(tempPercent, 1);
                        

                        QuestionList[i, j + 2 + vraagTest.vragen[i].antwoorden.Count] = tempPercent.ToString();
                    }
                }
                //Ik heb eerst gewerkt met een array als simulatie om te vragen weer te geven. Daarna heb ik pas gewerkt aan het ophalen van vragen via JSON, 
                //die dan in objecten werden geplaatst. Daarom dat ik ze nu terug in een array zet, wat misschien niet de beste methode is. 
                QuestionNr = 1;
                

                //oude array voor simulatie
                //vragenLijst = new string[5, 8] { { "In spark spoork noord zou ik graag ... zien","3", "Een speeltuin", "Een fabriek", "Een bos","15","65","20" },
                //{ "Op de keyserlei zijn er te weinig ...","3", "Bomen", "Vuilbakken", "Fietsstallingen","44","6","50" },
                //{ "Welk van de volgende evenementen zou je graag zien op de groenplaats?","3", "Tuinbouw Expo", "Metal festival", "Counter strike LAN party","56","14","30" },
                //{ "Vind je dat er te veel geluidsoverlast in de binnenstad in het weekend","3", "Ja", "Neen", "Geen meninig","15","50","35" },
                //{ "Voel je je 's avonds veilig in de stad Berchem,","3", "Ja", "Nee", "Niet van toepassing","15","12","73" },
                //};
            }



            if (!DataStringReceived)
            {
                return;
            }

        StreamWriter outputStream = File.CreateText(QuestionPath);
        outputStream.WriteLine(QuestionNr);
        outputStream.WriteLine(QuestionList.GetLength(0));
        outputStream.WriteLine(QuestionList.GetLength(1));
        for (int i = 0; i < QuestionList.GetLength(0); i++)
        {
            for (int j = 0; j < QuestionList.GetLength(1); j++)
            {
                if (QuestionList[i, j] != null)
                {
                    outputStream.Write(QuestionList[i, j] + "*");
                }
            }
            outputStream.WriteLine();
        }
        outputStream.Close();
    }
        
        else
        {
            //GameObject.Find("Result").GetComponent<Text>().text = "file does exist";
        }
        //GameObject.Find("Result1").GetComponent<Text>().text = "vragen code running";

        StreamReader inputStream = File.OpenText(QuestionPath);
            QuestionNr = int.Parse(inputStream.ReadLine());

        int tempLength = int.Parse(inputStream.ReadLine());
int tempWidth = int.Parse(inputStream.ReadLine());
string[] tempVraagArr;
QuestionList = new string[tempLength, tempWidth];
        for (int i = 0; i<tempLength; i++)
        {
            tempVraagArr = inputStream.ReadLine().Split('*');
            for (int j = 0; j<tempVraagArr.Length; j++)
            {
                QuestionList[i, j] = tempVraagArr[j];
            }
        }






        inputStream.Close();

        ThisQuestion = new string[tempWidth];
        //Debug.Log(vraagNummer);
        for (int i = 0; i < tempWidth; i++)
        {
            ThisQuestion[i] = QuestionList[QuestionNr - 1, i];
        }
        if (SceneManager.GetActiveScene().name == "Vraag2")
        {
            File.Delete(QuestionPath);
        }



        


        if (tempLength > QuestionNr && SceneManager.GetActiveScene().name == "Vraag2")
        {
            StreamWriter outputStream2 = File.CreateText(QuestionPath);
outputStream2.WriteLine(QuestionNr+1);
            outputStream2.WriteLine(tempLength);
            outputStream2.WriteLine(tempWidth);

            for (int i = 0; i<QuestionList.GetLength(0); i++)
            {
                for (int j = 0; j<QuestionList.GetLength(1); j++)
                {
                    if (QuestionList[i, j] != null)
                    {
                        outputStream2.Write(QuestionList[i, j] + "*");

                    }
                }
                outputStream2.WriteLine();
            }

            outputStream2.Close();
        }

        QuestionsReady = true;



        //GameObject.Find("Result").GetComponent<Text>().text += " " + dezeVraag[0];
        //GameObject.Find("Result1").GetComponent<Text>().text = Application.persistentDataPath;



    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            DataStringReceived = true;
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    [ClientRpc]

    void RpcVraag(string[] vraag,int vraagNr)
    {
        ThisQuestion = vraag;
        QuestionNr = vraagNr;
    }

    // Update is called once per frame
    void Update () {
        
        if (SceneManager.GetActiveScene().name != "Vraag" && SceneManager.GetActiveScene().name != "Vraag2")
        {
            return;
        }


        if (!QuestionsReady)
        {
            getVragenLijst();
        }
        
        

        if (QuestionsReady)
        {
            if (isServer)
            {
                RpcVraag(ThisQuestion, QuestionNr);
            }
        }
        
        
        //Debug.Log(dezeVraag[0]);

        


        if (isServer)
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            
            MyAnswer = ServerAnswer;
            EnemyAnswer = ClientAnswer;
        }
        else
        {
            MyAnswer = ClientAnswer;
            EnemyAnswer = ServerAnswer;
        }

        GameObject.Find("Timer").GetComponent<Text>().text = Mathf.Round(Timer) + " seconden";

        //GameObject.Find("Result").GetComponent<Text>().text = "Mijn antwoord: " + mijnAntwoord + " vijand antwoord: " + enemyAntwoord;
        //Debug.Log("Mijn antwoord: " + mijnAntwoord + " vijand antwoord: " + enemyAntwoord);

        Debug.Log(ServerSend + " " + ClientSend);
        //GameObject.Find("Result").GetComponent<Text>().text = ServerSend + " " + ClientSend;
        if (isServer && ServerSend)
        {
            CanvasSend.SetActive(true);
        }
        if (!isServer && ClientSend)
        {
            
            CanvasSend.SetActive(true);
        }


        if (ClientSend && ServerSend)
        {
            //    if (serverSend)
            //{
            if (SceneManager.GetActiveScene().name == "Vraag2")
            {
                StreamWriter outputStream = File.CreateText(ResultPath);
                outputStream.WriteLine(ServerAnswer);
                outputStream.WriteLine(ClientAnswer);
                outputStream.Close();
            }
            

            if (isServer)
            {
                if (SceneManager.GetActiveScene().name == "Vraag")
                {
                    if (!scoreAdded)
                    {
                        addScore(true);
                        addScore(false);
                        scoreAdded = true;
                    }
                    
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
        if (int.Parse(ThisQuestion[1]) == 4)
        {
            GameObject.Find("answer 4").GetComponent<Toggle>().isOn = false;
        }
        

    }
    public void answer2()
    {
        GameObject.Find("answer 1").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 3").GetComponent<Toggle>().isOn = false;
        if (int.Parse(ThisQuestion[1]) == 4)
        {
            GameObject.Find("answer 4").GetComponent<Toggle>().isOn = false;
        }
        
    }
    public void answer3()
    {
        GameObject.Find("answer 2").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 1").GetComponent<Toggle>().isOn = false;
        if (int.Parse(ThisQuestion[1]) == 4)
        {
            GameObject.Find("answer 4").GetComponent<Toggle>().isOn = false;
        }
        
    }

    public void answer4()
    {
        GameObject.Find("answer 2").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 1").GetComponent<Toggle>().isOn = false;
        GameObject.Find("answer 3").GetComponent<Toggle>().isOn = false;
    }

    public static void getDataPath(out string resultPath,out string questionPath)
    {
        questionPath = "\\vragenLijst.mixed";
        resultPath = "\\antwoordGegevens.mixed";
        if (Application.platform == RuntimePlatform.Android)
        {
            
            Directory.CreateDirectory(Application.persistentDataPath);

            resultPath = Application.persistentDataPath + resultPath;
            questionPath = Application.persistentDataPath + questionPath;
            //File.Delete(vragenPath);
            //GameObject.Find("Result").GetComponent<Text>().text = vragenPath;
            //StreamWriter outputStream = File.CreateText(vragenPath);
            //outputStream.Close();
            //if (!File.Exists(vragenPath))
            //{
            //    GameObject.Find("Result").GetComponent<Text>().text = "exists not";
            //}
            //else
            //{
            //    GameObject.Find("Result").GetComponent<Text>().text = "exists";
            //}

        }
        else
        {
            resultPath = "gameData" + resultPath;
            questionPath = "gameData" + questionPath;
            Directory.CreateDirectory("gameData");
        }
    }

    public static int addScore(bool itIsServer)
    {
        string scorePath = Application.persistentDataPath + "\\score.mixed";
        int clientScore;
        int serverScore;
        StreamReader inputStream = File.OpenText(scorePath);
        serverScore = int.Parse(inputStream.ReadLine());
        clientScore = int.Parse(inputStream.ReadLine());
        inputStream.Close();
        if (itIsServer)
        {
            serverScore++;
        }
        else
        {
            clientScore++;
        }
        File.Delete(scorePath);
        StreamWriter outputStream = File.CreateText(scorePath);
        outputStream.WriteLine(serverScore);
        outputStream.WriteLine(clientScore);
        outputStream.Close();
        if (itIsServer)
        {
            return serverScore;
        }
        return clientScore;

    }

    public static int getScore(bool itIsServer)
    {
        string scorePath = Application.persistentDataPath + "\\score.mixed";
        int clientScore;
        int serverScore;
        StreamReader inputStream = File.OpenText(scorePath);
        serverScore = int.Parse(inputStream.ReadLine());
        clientScore = int.Parse(inputStream.ReadLine());
        inputStream.Close();
        if (itIsServer)
        {
            return serverScore;
        }
        return clientScore;
    }

    public static void clearScore()
    {
        string scorePath = Application.persistentDataPath + "\\score.mixed";
        File.Delete(scorePath);
        StreamWriter outputStream = File.CreateText(scorePath);
        outputStream.WriteLine(0);
        outputStream.WriteLine(0);
        outputStream.Close();
    }




}
