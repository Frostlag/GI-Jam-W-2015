﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Player : MonoBehaviour {

	public static GameObject instance;
	private float targetx;
	public float speed;
	public float distancemod;
	public bool slide;

	void Awake(){
		Player.instance = gameObject;
	}
	// Use this for initialization
	void Start () {
		Conductor.instance.SendMessage ("Register", gameObject);
		targetx = this.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.x > targetx) {
			Vector3 temp = this.rigidbody2D.velocity;
			temp.x = 0;
			this.rigidbody2D.velocity = temp;
		}
		if (Input.GetKeyDown ("t")) {
			using (StreamWriter w = File.AppendText("log.txt"))
			{
				w.WriteLine( Convert.ToString ((Conductor.instance.GetComponent<Conductor>().songposition-Conductor.instance.GetComponent<Conductor>().start) / Conductor.instance.GetComponent<Conductor>().crochet - 1) + " trap1", w);
			}

		}
		if (Input.GetKeyDown ("s")) {
			Conductor.instance.SendMessage ("Special", "space");
		}
		// See if we are in window to input
		KeyValuePair<GameObject,string> parameter = new KeyValuePair<GameObject,string> (gameObject, "handleControls");
		Conductor.instance.SendMessage ("CanInput", parameter);
	}

	void handleControls(){
		if (Input.GetKeyDown ("space") ) {
			Conductor.instance.SendMessage ("KeyPressed", "space");
		}
	}

	void Move(float distance){
		if (!slide) {
			Vector3 temp = this.transform.position;
			temp.x += distance;
			this.transform.position = temp;
		}
		else{
			targetx += distance*distancemod;
			Vector3 temp = this.rigidbody2D.velocity;
			temp.x = distance * speed;
			this.rigidbody2D.velocity = temp;
		}
	}

	void Beat(){
		
	}
}
