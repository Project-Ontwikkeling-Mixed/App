using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMove : NetworkBehaviour {
	Rigidbody2D rb;
	float jumpSpeed = 50f;
	float velocity = 250f;
	public Sprite localSprite;
	public GameObject fireB;
	SpriteRenderer sr;

	float fireTimer = 1f;
	float tempTime = 0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		
	}

	public override void OnStartLocalPlayer()
	{
		//sr.sprite = localSprite;
	}
	
	// Update is called once per frame
	void Update () {

		//if (Input.touchCount > 0)
		//{
		//    CmdFire();
		//}

		
		tempTime += Time.deltaTime;

		if (!isLocalPlayer)
			return;
		
		if(Input.touchCount > 0 || Input.GetButton("Fire1"))
		{
			rb.AddForce(Vector2.up * jumpSpeed);
            GameObject.Find("Counter").GetComponent<Text>().text = (int.Parse(GameObject.Find("Counter").GetComponent<Text>().text) + 1).ToString();
		}

	}

	//[Command]
	//void CmdFire()
	//{ 
		
		
	//        if(tempTime > fireTimer)
	//        {
	//            tempTime = 0f;

	//            GameObject Fireball;
	//            Fireball = Instantiate(fireB, transform.position, transform.rotation) as GameObject;
	//            Fireball.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3 (-velocity, 0));

	//            NetworkServer.Spawn(Fireball);
	//        }
		
	//}
}
