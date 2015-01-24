using UnityEngine;
using System;
using System.Collections;
	
public class LevelMaster: MonoBehaviour{
	void Start () {		
		InitializeQueue (Application.dataPath + "/Levels/level1.txt");
	}
	// Properties
	// current beat being referenced 
	private Beat CurrentBeat;
	// Queue of beats to be poped onto Current Beat
	public Queue BeatQueue; 

	// game prefab
	public GameObject Floor = Resources.Load ("Floor") as GameObject;
	public GameObject Trap = Resources.Load ("Trap") as GameObject;
	public int speed = 50;
	public int totalBeats = 5000;
	//Methods

	// InitalizeQueue(string) -> bool
	// Takes in a fileName and reads through all lines and creates new beat objects out of them
	// Assumes file is in order and are properly space delimited as such:
	//  	start length damage
	// returns bool of whether or not it was initialized
	public bool InitializeQueue(string fileName) {
		GameObject floor = Instantiate (Floor) as GameObject;
		floor.transform.position = new Vector3 (0, 0);
		floor.transform.localScale = new Vector3 (speed * totalBeats, 2, 2);

		// open file, read all lines and loop through them
		BeatQueue = new Queue();
		string[] lines = System.IO.File.ReadAllLines (@fileName);
		foreach (string line in lines){
			Beat newBeat = new Beat();
			string[] row = line.Split(" "[0]);
			// determine format of the file
			// start duration damage
			newBeat.Start = int.Parse (row[0]);
			newBeat.Length = int.Parse (row[1]);
			newBeat.Key = row[2];
			GameObject trap = Instantiate (Trap) as GameObject;
			trap.transform.position = new Vector3(50 * newBeat.Start, 0); // start is a # of beats, beats * speed = distance
			trap.transform.localScale = new Vector3 (2, 50, 50);
			BeatQueue.Enqueue(newBeat);
		}
		return true;
	}

	// checks if the CurrentBeat is done and then sets it to a newer beat 
	public void CheckCurrentBeat(float time){
		if (CurrentBeat.Pass == false) {
			if (CurrentBeat.Start + CurrentBeat.Length > time) {
				// beat expires because the input time has passed
				PopNextBeat(true);
			}
		} 
		else { // this should not happen at all, but just in case 
			// beat was passed successfully and passed
			PopNextBeat(true);
		}
	} 

	public void PushInput(string playerInput){
		if (playerInput == CurrentBeat.Key) {
			CurrentBeat.Pass = true;
		}
	}

	// Pops first of beatqueue and sets it to CurrentBeat
	private void PopNextBeat(bool pass) {
		if (pass == false) {
			// something about failing
		}
		CurrentBeat = (Beat) BeatQueue.Dequeue();
	}
}

public class Beat {
	public int Start;
	public int Length;
	public bool Pass = false;
	public int Damage;
	public string Key;
}