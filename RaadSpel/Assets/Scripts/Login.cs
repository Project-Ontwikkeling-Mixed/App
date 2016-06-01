using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {


	public InputField emailIField;
	public InputField passwordIField;

	public GameObject panelUI;


	public Text warningMsg;

	string email;
	string password;

	string userName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void getLoginInfo()
	{

		email = emailIField.text;
		password = passwordIField.text;
		
	}

	public IEnumerator TryLogin ()
	{
		
		
		
		getLoginInfo();

		//Debug.Log("name= " + email);
		//Debug.Log("pass= " + password);

		if(email != null || password != null)
		{
			WWWForm form = new WWWForm();
			form.AddField("email", email);
			form.AddField("password", password);

			WWW www = new WWW("http://mixed.multimediatechnology.be/spel/login", form);

			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.Log(www.error);
			}
			else
			{
				
				Debug.Log(www.text);
				if(!www.text.Contains("failed"))
				{
					var jsonString = JsonUtility.FromJson<EmailName>(www.text);
					userName = jsonString.email;

					PlayerPrefs.SetString("username", userName);
					SceneManager.LoadScene("Menu ingelogd");
				}

				else
				{
					SceneManager.LoadScene("Menu uitgelogd");
					OpenWarningMsg();

					
					//PanelUI.gameObject.SetActive(false);
				}
				
				
			}
		}

		else
		{
			Debug.Log("No email or password");
		}

		
	   
		
	}

	public void StartRoutine()
	{
		StartCoroutine(TryLogin());
	}

	void OpenWarningMsg()
	{
		warningMsg.gameObject.SetActive(true);
	}

	 
}

[Serializable]
public class EmailName{

	public string email;

}
