using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DialogueTextSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public List<string> LoadDialogue(string filename)
	{
		string file = "Assets/Resources/Dialogue/" + filename + ".txt";
		string line;
		List<string> lines = new List<string>();

		StreamReader r = new StreamReader (file);

		using(r)
		{
			do
			{
				line = r.ReadLine();
				if(line != null)
				{
					lines.Add (line);
				}
			}
			while(line != null);
			r.Close();
		}

		return lines;
	}
}
