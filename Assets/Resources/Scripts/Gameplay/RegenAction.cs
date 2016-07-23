using UnityEngine;
using System.Collections;

public class RegenAction : GOAPAction {

	private bool regened = false;

	public RegenAction(){
		addEffect ("stayAlive", true);
		cost = 500f;
	}

	public override void reset() {
		regened = false;
	}

	public override bool isDone(){
		return regened;
	}

	public override bool requiresInRange(){
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent){
		target = GameObject.Find ("Player");
		return target != null;
	}

	public override bool perform(GameObject agent){
		Enemy e = agent.GetComponent<Enemy> (); 
		e.passiveRegen ();
		regened = true;
		return true;
	}
}
