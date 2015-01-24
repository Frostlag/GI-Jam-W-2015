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

	private BeatMaster beatMaster;

	List<GameObject> tbn;

	void Awake(){
		Conductor.instance = gameObject;
		tbn = new List<GameObject> ();
	}

	void Start () {		
		beatselapsed = 0;
		crochet = 60 / bpm;
		audio.Play ();
		start = (float)(AudioSettings.dspTime) - offset ;
		lastbeat = start;

		beatMaster = new BeatMaster();
	}
	
	// Update is called once per frame
	void Update () {
		songposition = (float)(AudioSettings.dspTime) ;

		//beatMaster.CheckCurrentBeat (songposition - start);

		if (songposition > lastbeat + crochet){
			lastbeat += crochet;
			beatselapsed++;
			for (int i = 0; i < tbn.Count; i++){
				tbn[i].SendMessage("Beat");
			}
		}		
	}

	void CanInput(KeyValuePair<GameObject,string> caller){
		if ((songposition > lastbeat - delta && songposition < lastbeat + delta)
			|| (songposition > lastbeat + crochet - delta && songposition < lastbeat + crochet + delta)) {
			caller.Key.SendMessage (caller.Value);

		} 
	}

	void Register(GameObject tbr){
		tbn.Add(tbr);
	}	
}
