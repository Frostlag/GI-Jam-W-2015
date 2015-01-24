using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conductor : MonoBehaviour {
	public static GameObject instance;
	public float bpm;
	public float beatselapsed;
	public float crochet;
	public float lastbeat;
	public float songposition;
	public float start;
	public float delta;

	List<GameObject> tbn;
	void Awake(){
		Conductor.instance = gameObject;
		tbn = new List<GameObject> ();
	}

	void Start () {	
		beatselapsed = 0;
		crochet = 60 / bpm;
		audio.Play ();
		start = (float)(AudioSettings.dspTime);
		lastbeat = start;
	}
	
	// Update is called once per frame
	void Update () {
		songposition = (float)(AudioSettings.dspTime);
		if (songposition > lastbeat + crochet){
			lastbeat += crochet;
			beatselapsed++;
			for (int i = 0; i < tbn.Count; i++){
				tbn[i].SendMessage("Beat");
			}
		}		
	}

	bool CanInput(GameObject gameObject, string message){
		gameObject.SendMessage (message);
	}

	void Register(GameObject tbr){
		tbn.Add(tbr);
	}	
}
