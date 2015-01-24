﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine;
	
public class BeatMaster{
	// Properties
	// current beat being referenced 
	private Beat CurrentBeat;
	// Queue of beats to be poped onto Current Beat
	private Queue BeatQueue; 

	//Methods

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
			newBeat.Length = int.Parse (row[1]);
			newBeat.Damage = int.Parse (row[2]);
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