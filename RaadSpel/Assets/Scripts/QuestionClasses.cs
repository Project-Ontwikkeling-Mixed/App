using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//variabelen moeten in het nederlands zijn door JSON
[Serializable]
public class QuestionList{
    public List<Question> vragen;
}

[Serializable]
public class Question
{

    public int id;
    public string vraag;
    public int fase_id;
    public List<answer> antwoorden;

}

[Serializable]
public class answer
{
    public int id;
    public string antwoord;
    public int inspraakvraag_id;
    public int aantal_gekozen;
}




