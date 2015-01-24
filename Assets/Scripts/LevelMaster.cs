using UnityEngine;
using System;
using System.Collections;
	
public class LevelMaster : MonoBehaviour {
	// Properties
	// current beat being referenced 
	public Beat CurrentBeat;
	public Beat NextBeat;
	// Queue of beats to be poped onto Current Beat
	public Queue BeatQueue; 

	void Start(){
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
		foreach (string line in lines){
			Beat newBeat = new Beat();
			string[] row = line.Split(" "[0]);
			// determine format of the file
			// start duration damage
			newBeat.Start = int.Parse (row[0]);
			newBeat.Type = row[1];
			newBeat.Pass = false;
			BeatQueue.Enqueue(newBeat);
		}
		PopNextBeat();
		return true;
	}

	public void KeyPressed(string key){
		CurrentBeat.Pass = true;
	}

	// Pops first of beatqueue and sets it to CurrentBeat
	public void PopNextBeat() {
		if (CurrentBeat == null){
			CurrentBeat = BeatQueue.Dequeue();
			NextBeat = (Beat) BeatQueue.Dequeue();
		}else{
			CurrentBeat = NextBeat;
			NextBeat = BeatQueue.Dequeue();
		}
	}
}

public class Beat {
	public int Start;
	public string Type;
	public bool Pass;
}