using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscene : DialogueTextSystem {

	string filename;
	List<string> dialogue;
	//add images and text-based cues for movement

	// Use this for initialization
	public void Start () {
		filename = gameObject.name;
		dialogue = LoadDialogue (filename);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
