using UnityEngine;
using System;
using System.Collections;
	
public class LevelMaster : MonoBehaviour {
	// Properties
	// current beat being referenced 
	public Beat CurrentBeat;
	public GameObject Floor = Resources.Load ("Floor") as GameObject;
	public GameObject Trap = Resources.Load ("Trap") as GameObject;
	public float speed = 0.5f;
	public int totalBeats = 500;

	// Queue of beats to be poped onto Current Beat
	public Queue BeatQueue; 

	void Start(){
		InitializeQueue (Application.dataPath + "/Levels/level1.txt");
	}

	void Update(){
	}

	// InitalizeQueue(string) -> bool
	// Takes in a fileName and reads through all lines and creates new beat objects out of them
	// Assumes file is in order and are properly space delimited as such:
	//  	start length damage
	// returns bool of whether or not it was initialized
	public bool InitializeQueue(string fileName) {
		// open file, read all lines and loop through them
		BeatQueue = new Queue();
		string[] lines = System.IO.File.ReadAllLines (@fileName);
		GameObject floor = Instantiate (Floor) as GameObject;
		floor.transform.position = new Vector3 (0, -1);
		floor.transform.localScale = new Vector3 (speed * totalBeats, 2, 2);
		int last = 1;
		foreach (string line in lines){
			Beat newBeat = new Beat();	
			string[] row = line.Split(" "[0]);
			// determine format of the file
			// start duration damage
			newBeat.Start = float.Parse (row[0]);
			newBeat.Pass = false;
			GameObject trap = Instantiate (Trap) as GameObject;
			trap.transform.localScale = new Vector3 (1, 2, 2);
			trap.transform.position = new Vector3(speed * newBeat.Start, 1); // start is a # of beats, beats * speed = distance

			while (last < newBeat.Start){
				Beat sneakBeat = new Beat();
				sneakBeat.Start = last;
				sneakBeat.Pass = false;
				GameObject sneaktrap = Instantiate (Trap) as GameObject;
				sneaktrap.transform.localScale = new Vector3 (1, 2, 2);
				sneaktrap.transform.position = new Vector3(speed * sneakBeat.Start, 1); // start is a # of beats, beats * speed = distance

				BeatQueue.Enqueue(sneakBeat);
				last ++;
			}
			BeatQueue.Enqueue(newBeat);
			last++;
		}
		PopNextBeat();
		return true;
	}

	public void KeyPressed(string key){
		CurrentBeat.Pass = true;

		if (CurrentBeat.Pass) {
			PopNextBeat();
		}
	}
	

	// Pops first of beatqueue and sets it to CurrentBeat
	public void PopNextBeat() {
		if (CurrentBeat == null){
			CurrentBeat = (Beat) BeatQueue.Dequeue();
		}else{
			if (!CurrentBeat.Pass){
				print ("Failed");
				Player.instance.SendMessage("Kick");
			}
			CurrentBeat = (Beat) BeatQueue.Dequeue();
		}
		//print (CurrentBeat.Start);
	}
}

public class Beat {
	public float Start;
	public bool Pass;
}