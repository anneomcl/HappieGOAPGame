using UnityEngine;
using System.Collections;

public class RiverCat : Cutscene {

	float speed;
	Vector3 target;
	GameObject cat;
	GameObject player;
	float distanceFromTarget;

	// Use this for initialization
	new void Start () {
		base.Start ();
		cat = GameObject.Find ("TheCat");
		player = GameObject.Find ("Player");
		speed = 2f;
		distanceFromTarget = 2f;
		target = new Vector3 (player.transform.position.x, player.transform.position.y - distanceFromTarget, player.transform.position.z);

	}

	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		cat.transform.position = Vector3.MoveTowards(cat.transform.position, target, step);
	}
}
