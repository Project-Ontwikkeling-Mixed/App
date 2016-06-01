using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ResultScript : NetworkBehaviour
{

    int RightAnswer;
    float HighestPercent = -1;

    int ClientAnswer;
    int ServerAnswer;

    [SyncVar]
    int ServerScore;
    [SyncVar]
    int ClientScore;

    [SyncVar]
    bool ClientBool;
    [SyncVar]
    bool ServerBool;


    [SyncVar]
    public bool ClientSend = false;
    [SyncVar]
    public bool ServerSend = false;




    int QuestionNr;
    string QuestionPath;
    string ResultPath;
    string[,] QuestionList;
    string[] ThisQuestion;
    string[] Answers;
    float[] Percents;
    int AnswerAmount;


    bool resultShowed = false;


    public void send()
    {
        ServerSend = true;
    }


    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Result")
        {
            return;
        }
        QuestionData.getDataPath(out ResultPath, out QuestionPath);

        if (isServer)
        {
            readAnswers();
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name != "Result")
        {
            return;
        }


        if (isServer)
        {
            if (ServerAnswer == RightAnswer)
            {
                ServerBool = true;

            }
            else
            {
                ServerBool = false;

            }

            if (ClientAnswer == RightAnswer)
            {
                ClientBool = true;
            }
            else
            {
                ClientBool = false;
            }
            if (ServerSend)
            {
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>().ServerChangeScene("Vraag");
            }

        }



        if (Answers != null && !resultShowed)
        {
            getResult();
        }



        if (isServer)
        {
            RpcVraag(ThisQuestion, Answers, Percents, QuestionNr);
        }
        else
        {
            GameObject.Find("Scores").GetComponent<Text>().text = "Jouw score: " + ClientScore + "\nTegenstander score: " + ServerScore;
        }

    }



    void readAnswers()
    {
        StreamReader inputStream = File.OpenText(QuestionPath);
        QuestionNr = int.Parse(inputStream.ReadLine());
        //question number is al geincrement, dus nu nummer verlagen
        QuestionNr--;
        int tempLength = int.Parse(inputStream.ReadLine());
        int tempHeight = int.Parse(inputStream.ReadLine());
        string[] tempVraagArr;
        QuestionList = new string[tempLength, tempHeight];
        for (int i = 0; i < tempLength; i++)
        {
            tempVraagArr = inputStream.ReadLine().Split('*');
            for (int j = 0; j < tempVraagArr.Length; j++)
            {
                QuestionList[i, j] = tempVraagArr[j];
            }
        }
        inputStream.Close();

        ThisQuestion = new string[tempHeight];

        for (int i = 0; i < tempHeight; i++)
        {
            ThisQuestion[i] = QuestionList[QuestionNr - 1, i];
        }




        try
        {
            AnswerAmount = int.Parse(ThisQuestion[1]);
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }



        Answers = new string[AnswerAmount];
        Percents = new float[AnswerAmount];
        //for (int i = 0; i < vragenLijst.GetLength(1); i++)
        //{
        //    Debug.Log("Vraag Debug: " + vragenLijst[vraagNummer - 1, i]);
        //}



        for (int i = 2; i < 2 + AnswerAmount; i++)
        {
            Debug.Log("VraagNummer= " + QuestionNr);
            Answers[i - 2] = ThisQuestion[i];
        }

        try
        {
            for (int i = 2 + AnswerAmount; i < 2 + AnswerAmount * 2; i++)
            {
                //Debug.Log("Vraag Debug: " + vragenLijst[vraagNummer - 1, i]);
                Percents[i - 2 - AnswerAmount] = float.Parse(ThisQuestion[i]);
            }

        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }

        for (int i = 0; i < Percents.Length; i++)
        {
            if (Percents[i] > HighestPercent)
            {
                HighestPercent = Percents[i];
                RightAnswer = i + 1;
            }
        }

        //for (int i = 0; i < percenten.Length; i++)
        //{
        //    Debug.Log("Percent: " + percenten[i]);
        //}







        StreamReader reader = File.OpenText(ResultPath);
        ServerAnswer = int.Parse(reader.ReadLine());
        ClientAnswer = int.Parse(reader.ReadLine());
        reader.Close();
    }


    [ClientRpc]

    void RpcVraag(string[] vraag, string[] answers, float[] percents, int vraagNr)
    {
        ThisQuestion = vraag;
        QuestionNr = vraagNr;
        Answers = answers;
        Percents = percents;
    }





    void getResult()
    {
        if (isServer && ServerBool)
        {
            if (ClientBool)
            {
                QuestionData.addScore(true);
                QuestionData.addScore(false);
                Debug.Log("Score added server"); Debug.Log("Score added client");
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft ook juist geraden!";
            }
            else
            {
                Debug.Log("Score added sserver2");
                QuestionData.addScore(true);
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft fout geraden!";
            }

        }
        else if (isServer)
        {
            if (ClientBool)
            {
                QuestionData.addScore(false);
                Debug.Log("Score added client 2");
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft ook fout geraden!";
            }

        }

        if (!isServer && ClientBool)
        {
            if (ServerBool)
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft ook juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft fout geraden!";
            }
        }
        else if (!isServer)
        {
            if (ServerBool)
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft ook fout geraden!";
            }
        }
        resultShowed = true;
        Debug.Log(this);

        if (isServer)
        {
            ServerScore = QuestionData.getScore(true);
            ClientScore = QuestionData.getScore(false);
            GameObject.Find("Scores").GetComponent<Text>().text = "Jouw score: " + ServerScore + "\nTegenstander score: " + ClientScore;
        }
        else
        {
            GameObject.Find("Scores").GetComponent<Text>().text = "Jouw score: " + ClientScore + "\nTegenstander score: " + ServerScore;
        }






        for (int i = 0; i < Percents.Length; i++)
        {
            if (Percents[i] > HighestPercent)
            {
                HighestPercent = Percents[i];
                RightAnswer = i + 1;
            }
        }

        //for (int i = 0; i < percenten.Length; i++)
        //{
        //    Debug.Log("Percent: " + percenten[i]);
        //}

        if (!isServer)
        {
            AnswerAmount = Answers.Length;
        }

        GameObject.Find("Results").GetComponent<Text>().text = "Resultaten:\n";

        for (int i = 0; i < AnswerAmount; i++)
        {
            GameObject.Find("Results").GetComponent<Text>().text = GameObject.Find("Results").GetComponent<Text>().text + Answers[i] + ": " + Percents[i] + "%\n";
        }


    }
}
