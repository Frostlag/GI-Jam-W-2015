using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Conductor.instance.SendMessage ("Register", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		// See if we can handle input
		Conductor.instance.SendMessage ("CanInput", gameObject,"handleControls");

	}

	void handleControls(){
		if (Input.GetKeyDown("space")){
			print ("Space");
		}
	}

	void Beat(){
		
	}
}
