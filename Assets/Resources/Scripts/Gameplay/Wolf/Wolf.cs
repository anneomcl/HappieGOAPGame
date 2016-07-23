using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wolf : Enemy {

	// Use this for initialization
	void Start () {
		stamina = 100f;
		health = 50;
		speed = 20;
		strength = 10;
		regenRate = .5f;
		maxStamina = 100f;

		terminalSpeed = speed / 10;
		initialSpeed = (speed / 10) / 2;
		acceleration = (speed / 10) / 4;

		animator = GetComponent<Animator> ();
		player = GameObject.Find ("Player").GetComponent<PlayerMovement>();

	}

	public override void passiveRegen(){
		stamina += regenRate;
	}

	public override HashSet<KeyValuePair<string, object>> createGoalState(){
		HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>> ();
		goal.Add (new KeyValuePair<string, object> ("damagePlayer", true));
		goal.Add (new KeyValuePair<string, object> ("stayAlive", true));
		return goal;
	}
}
