using UnityEngine;
using System.Collections;

public class WolfAttackAction : GOAPAction {

	private bool attacked = false;

	public WolfAttackAction(){
		addEffect ("damagePlayer", true);
		cost = 100f;
	}

	public override void reset() {
		attacked = false;
		target = null;
	}

	public override bool isDone(){
		return attacked;
	}

	public override bool requiresInRange(){
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent){
		target = GameObject.Find ("Player");
		return target != null;
	}

	public override bool perform(GameObject agent){
		Wolf currWolf = agent.GetComponent<Wolf> ();
		if (currWolf.stamina >= (cost)) {
			
			currWolf.animator.Play ("wolfAttack");

			int damage = currWolf.strength;
			if (currWolf.player.isBlocking) {
				damage -= currWolf.player.defense;
			}

			currWolf.player.health -= damage;
			currWolf.stamina -= cost;

			attacked = true;
			return true;
		} else {
			return false;
		}
	}
}
