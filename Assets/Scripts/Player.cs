﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static GameObject instance;
	public float speed;
	public float distancemod;
	public bool teleport;

	void Awake(){
		Player.instance = gameObject;
	}
	// Use this for initialization
	void Start () {
		Conductor.instance.SendMessage ("Register", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("t")) {
			Conductor.instance.SendMessage ("Test", "space");
		}
		if (Input.GetKeyDown ("s")) {
			Conductor.instance.SendMessage ("Special", "space");
		}
		// See if we are in window to input
		KeyValuePair<GameObject,string> parameter = new KeyValuePair<GameObject,string> (gameObject, "handleControls");
		Conductor.instance.SendMessage ("CanInput", parameter);

		if (!teleport){
			Vector3 temp = this.rigidbody2D.velocity;
			temp.x = speed/(60/Conductor.instance.GetComponent<Conductor>().bpm);
			this.rigidbody2D.velocity = temp;
		}
	}

	void handleControls(){
		if (Input.GetKeyDown ("space") ) {
			Conductor.instance.SendMessage ("KeyPressed", "space");
		}
	}

	void Move(float distance){
		if (teleport){
			Vector3 temp = this.transform.position;
			temp.x += distance;
			this.transform.position = temp;
		}else{
			Vector3 temp = this.rigidbody2D.velocity;
			temp.x = speed;
			this.rigidbody2D.velocity = temp;
		}
	}

	public void setSpeed(float ratio){
		gameObject.GetComponent<Animator> ().speed = speed / ratio;
	}
	
	void Beat(){
		//Beat nextBeat = (Beat)Conductor.instance.GetComponent<Conductor> ().lms.BeatQueue.Peek ();
		Beat nextBeat = Conductor.instance.GetComponent<Conductor> ().lms.CurrentBeat;
		setSpeed (nextBeat.Duration);
	}
}
