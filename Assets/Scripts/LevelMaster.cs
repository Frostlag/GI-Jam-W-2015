using UnityEngine;
using System;
using System.Collections;
	
public class LevelMaster : MonoBehaviour {
	// Properties
	// current beat being referenced 
	public Beat CurrentBeat;
	public GameObject Background = Resources.Load ("Background") as GameObject;
	public GameObject Trap = Resources.Load ("Trap") as GameObject;
	public float speed ;
	public int totalBeats = 500;
	public int lives;

	// Queue of beats to be poped onto Current Beat
	public Queue BeatQueue; 

	// trap types

	void Start(){
		InitializeQueue (Application.dataPath + "/Levels/pup.txt");
	}

	void Update(){
		if (lives == 0) {
			Conductor.instance.SendMessage("DIE");
		}
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

		for (int x = 0 ; x < 100; x ++){
			GameObject Back= Instantiate (Background) as GameObject;
			Vector3 temp =  Back.transform.position;
			temp.x = (x*24);
			Back.transform.position = temp;
		}
		float last = 0;
		int id = 0;
		foreach (string line in lines){
			string[] row = line.Split(" "[0]);
			id++;
			while (last < float.Parse (row[0])){
				Beat sneakBeat = new Beat();
				sneakBeat.Start = last;
				sneakBeat.Type = "sneak";
				sneakBeat.Pass = false;
				sneakBeat.id = id;
				sneakBeat.Duration = 1;
				GameObject sneaktrap = Instantiate (Trap) as GameObject;
				DrawTrap(speed * sneakBeat.Start,Color.blue);

				BeatQueue.Enqueue(sneakBeat);
				last+=1;
				id++;
			}

			last += PlaceTrapCaller (row[1], float.Parse (row[0]),id);

		}
		PopNextBeat();
		return true;
	}
	private int PlaceTrap(float[] BeatList, int length,float start, int id){
		float startOffset = 0;
		bool first = true;
		foreach (float i in BeatList) {
			Beat newBeat = new Beat();
			startOffset += i;
			newBeat.Start = start * speed + startOffset * speed;
			newBeat.Type = "sneak";
			newBeat.Pass = false;
			newBeat.Duration = length - startOffset;
			newBeat.id = id;
			GameObject sneaktrap = Instantiate (Trap) as GameObject;

			if (first) {
				DrawTrap(speed * newBeat.Start, Color.green);
				first = false;
			}
			else {
				DrawTrap(speed * newBeat.Start, Color.red);
			}
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
			int id = CurrentBeat.id;
			if (!CurrentBeat.Pass){
				lives--;
				//print (CurrentBeat.Duration);
				print ("Fail");	
				Player.instance.GetComponent<Animator>().SetBool ("Pass", false);
				Player.instance.GetComponent<Player>().setSpeed(0.5f);
				Player.instance.SendMessage("Move",CurrentBeat.Duration);
				while (CurrentBeat.id == id){
					CurrentBeat = (Beat) BeatQueue.Dequeue();       
				}
			}
			else{
				float laststart = CurrentBeat.Start;
				Player.instance.GetComponent<Animator>().SetBool ("Pass", true);
				CurrentBeat = (Beat) BeatQueue.Dequeue();
				Player.instance.SendMessage("Move",CurrentBeat.Start - laststart);
			}
		}
	}

	private int PlaceTrapCaller( string traptype , float beat, int id){
		int last = 0;

		if (traptype == "trap1" || traptype == "double") {
			last = PlaceTrap (new float[2] {0.0f, 0.5f}, 1, beat,id);
		}
		if (traptype == "trap2") {
			last = PlaceTrap (new float[3] {0.0f, 0.5f, 0.25f}, 1, beat,id);
		}
		if (traptype == "trap3") {
			last = PlaceTrap (new float[3] {0.0f, 0.25f, 0.5f}, 1, beat,id);
		}
		if (traptype == "trap4") {
			last = PlaceTrap (new float[3] {0.0f, 0.25f, 0.25f}, 1, beat,id);
		}
		if (traptype == "trap5") {
			last = PlaceTrap (new float[3] {0.0f, 1f/3f, 1f/3f}, 1, beat,id);
		}
		if (traptype == "trap6") {
			last = PlaceTrap (new float[2] {0.0f, 4f/6f}, 1, beat,id);
		}
		if (traptype == "trap7") {
			last = PlaceTrap (new float[2] {0.0f, 1f/3f }, 1, beat,id);
		}
		if (traptype == "trap8") {
			last = PlaceTrap (new float[2] {0.0f, 0.75f}, 1, beat,id);
		}
		if (traptype == "trap9") {
			last = PlaceTrap (new float[2] {0.0f, 0.25f}, 1, beat,id);
		}
		if (traptype == "rest") {
			last = PlaceTrap (new float[2] {0.0f, 2.0f}, 2, beat,id);
		}
		if (traptype == "rest") {
			last = PlaceTrap (new float[2] {0.0f, 2.0f}, 2, beat,id);
		}
	return last;
	}

	private void DrawTrap( float pos, Color color){

		GameObject newbeat = Instantiate (Trap) as GameObject;
		newbeat.transform.position = new Vector3 (pos, -2);
		newbeat.transform.localScale = new Vector3(0.5f, 5, 10);
		newbeat.renderer.material.SetColor ("_Color", color);
	}
}

public class Beat {
	public int id;
	public float Start;
	public string Type;
	public bool Pass;
	public float Duration;
}
public class BeatTrap {
	public float[] BeatList;
	public int TrapLength;
}