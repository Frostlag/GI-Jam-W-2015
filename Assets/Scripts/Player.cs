using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static GameObject instance;

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
		
		// See if we are in window to input
		KeyValuePair<GameObject,string> parameter = new KeyValuePair<GameObject,string> (gameObject, "handleControls");
		Conductor.instance.SendMessage ("CanInput", parameter);

	}

	void handleControls(){
		if (Input.GetKeyDown ("space")) {
			Vector3 newpos = this.transform.position;
			newpos.x += 1;
			this.transform.position = newpos;
			Conductor.instance.SendMessage ("KeyPressed", "space");
		}
	}

	void Kick(){
		Vector3 newpos = this.transform.position;
		newpos.x += 1;
		this.transform.position = newpos;
	}

	void Beat(){
		
	}
}
