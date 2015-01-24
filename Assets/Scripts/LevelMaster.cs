using UnityEngine;
using System;
using System.Collections;
	
public class LevelMaster : MonoBehaviour {
	// Properties
	// current beat being referenced 
	public Beat CurrentBeat;
	public GameObject Floor = Resources.Load ("Floor") as GameObject;
	public GameObject Trap = Resources.Load ("Trap") as GameObject;
	public int speed = 1;
	public int totalBeats = 500;

	// Queue of beats to be poped onto Current Beat
	public Queue BeatQueue; 

	void Start(){
		print ("Started");
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
		floor.transform.position = new Vector3 (0, 0);
		floor.transform.localScale = new Vector3 (speed * totalBeats, 2, 2);
		int last = 1;
		foreach (string line in lines){
			Beat newBeat = new Beat();
			string[] row = line.Split(" "[0]);
			// determine format of the file
			// start duration damage
			newBeat.Start = int.Parse (row[0]);
			newBeat.Type = row[1];
			newBeat.Pass = false;
			GameObject trap = Instantiate (Trap) as GameObject;
			trap.transform.position = new Vector3(speed * newBeat.Start, 0); // start is a # of beats, beats * speed = distance
			trap.transform.localScale = new Vector3 (2, 50, 50);
			while (last < newBeat.Start){
				Beat sneakBeat = new Beat();
				sneakBeat.Start = last;
				sneakBeat.Type = "sneak";
				sneakBeat.Pass = false;
				GameObject sneaktrap = Instantiate (Trap) as GameObject;
				sneaktrap.transform.position = new Vector3(speed * sneakBeat.Start, 0); // start is a # of beats, beats * speed = distance
				sneaktrap.transform.localScale = new Vector3 (2, 50, 50);
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
				print ("fail");
				Player.instance.SendMessage("Kick");
			}
			print (CurrentBeat.Pass);
			CurrentBeat = (Beat) BeatQueue.Dequeue();
		}
	}
}

public class Beat {
	public int Start;
	public string Type;
	public bool Pass;
}