using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static GameObject instance;
	private float targetx;
	public float speed;
	public float runSpeed;
	public float distancemod;

	void Awake(){
		Player.instance = gameObject;
	}
	// Use this for initialization
	void Start () {
		Conductor.instance.SendMessage ("Register", gameObject);
		targetx = this.transform.position.x;
		runSpeed = Conductor.instance.GetComponent<Conductor> ().bpm / 120;
		gameObject.GetComponent<Animator> ().speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.x > targetx) {
			Vector3 temp = this.rigidbody2D.velocity;
			temp.x = 0;
			this.rigidbody2D.velocity = temp;
		}
		if (Input.GetKeyDown ("t")) {
			Conductor.instance.SendMessage ("Test", "space");
		}
		
		// See if we are in window to input
		KeyValuePair<GameObject,string> parameter = new KeyValuePair<GameObject,string> (gameObject, "handleControls");
		Conductor.instance.SendMessage ("CanInput", parameter);

	}

	void handleControls(){
		if (Input.GetKeyDown ("space")) {
			Conductor.instance.SendMessage ("KeyPressed", "space");
		}
	}

	void Move(float distance){
//		Vector3 temp = this.transform.position;
//		temp.x += distance;
//		this.transform.position = temp;

		targetx += distance*distancemod;
		Vector3 temp = this.rigidbody2D.velocity;
		temp.x = distance * speed;
		this.rigidbody2D.velocity = temp;
	}

	void setSpeed(float ratio){
		gameObject.GetComponent<Animator> ().speed = runSpeed / ratio;
	}

	void Beat(){
		//Beat nextBeat = (Beat)Conductor.instance.GetComponent<Conductor> ().lms.BeatQueue.Peek ();
		Beat nextBeat = Conductor.instance.GetComponent<Conductor> ().lms.CurrentBeat;
		setSpeed (nextBeat.Duration);
	}
}