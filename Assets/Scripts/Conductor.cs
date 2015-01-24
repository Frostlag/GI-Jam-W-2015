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
	public float offset;
	public GameObject levelMaster;
	private LevelMaster lms;

	List<GameObject> tbn;

	void Awake(){
		tbn = new List<GameObject> ();
	}

	void Start () {		
		beatselapsed = 0;
		crochet = 60 / bpm;
		audio.Play ();
		start = (float)(AudioSettings.dspTime) - offset ;
		lastbeat = start;
		lms = levelMaster.GetComponent<LevelMaster> ();
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

	//Checking if we are in the window of receiving input
	void CanInput(KeyValuePair<GameObject,string> caller){
		float cbt = lms.CurrentBeat * crochet;
		float ct = songposition - start;
		if (cbt - delta > ct && cbt + delta < ct){
			caller.Key.SendMessage (caller.Value);
		} 
	}

	void KeyPressed(string key){
		levelMaster.KeyPressed (key);
	}

	void Register(GameObject tbr){
		tbn.Add(tbr);
	}
}
