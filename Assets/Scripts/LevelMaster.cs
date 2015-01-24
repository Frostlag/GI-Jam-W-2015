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

	// trap types

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
		floor.transform.position = new Vector3 (0, 0);
		floor.transform.localScale = new Vector3 (speed * totalBeats, 2, 2);
		float last = 1;
		foreach (string line in lines){
			string[] row = line.Split(" "[0]);

			while (last < float.Parse (row[0])){
				Beat sneakBeat = new Beat();
				sneakBeat.Start = last;
				sneakBeat.Type = "sneak";
				sneakBeat.Pass = false;
				GameObject sneaktrap = Instantiate (Trap) as GameObject;
				sneaktrap.transform.position = new Vector3(speed * sneakBeat.Start, 0); // start is a # of beats, beats * speed = distance
				sneaktrap.transform.localScale = new Vector3 (2, 50, 50);
				BeatQueue.Enqueue(sneakBeat);
				last+=1;
			}
			if (row[1] == "type1") {
				last += PlaceTrap (new float[6] {0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f}, 2, float.Parse (row[0]));
			}
		}
		PopNextBeat();
		return true;
	}
	private int PlaceTrap(float[] BeatList, int length,float start){
		float startOffset = 0;
		foreach (float i in BeatList) {
			Beat newBeat = new Beat();
			startOffset += i;
			newBeat.Start = start * speed + startOffset * speed;
			newBeat.Type = "sneak";
			newBeat.Pass = false;
			GameObject sneaktrap = Instantiate (Trap) as GameObject;
			sneaktrap.transform.position = new Vector3(start * speed + startOffset * speed, 0);
			sneaktrap.transform.localScale = new Vector3(2, 50, 50);
			BeatQueue.Enqueue (newBeat);
		}
		return length;
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
				Player.instance.SendMessage("Kick");
			}
			CurrentBeat = (Beat) BeatQueue.Dequeue();
		}
		print (CurrentBeat.Start);
	}
}

public class Beat {
	public float Start;
	public string Type;
	public bool Pass;
}
public class BeatTrap {
	public float[] BeatList;
	public int TrapLength;
}