using UnityEngine;
using System.Collections;

public class AbigailAttackAction : GOAPAction {

	private bool attacked = false;

	public AbigailAttackAction(){
		//addPrecondition ("playerDefending", true);
		addEffect ("damagePlayer", true);
		cost = 300f;
	}

	public override void reset(){
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
		Abigail currA = agent.GetComponent<Abigail> ();

		if (target != null && target.GetComponent<PlayerMovement>().isBlocking && 
			currA.stamina >= (500 - cost)) { //to-do: make scaling system instead of magic number) 
			return true;
		} else {
			return false;
		}
	}

	public override bool perform(GameObject agent){
		Abigail currA = agent.GetComponent<Abigail> ();
		currA.stamina -= (500 - cost);

		Animator currAnim = GetComponentInParent<Animator> ();
		//spellAnim.wrapMode = WrapMode.ClampForever; //done in inspector right now
		currAnim.Play("abigailSpell");

		attacked = true;
		return attacked;
	}
}
