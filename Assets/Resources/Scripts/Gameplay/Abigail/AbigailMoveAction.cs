using UnityEngine;
using System.Collections;

public class AbigailMoveAction : GOAPAction {
	
	private bool moved = false;
	private float dashSpeed;
	private Vector3 dashTarget;
	private bool isDashing = false;

	public AbigailMoveAction(){
		//addPrecondition ("playerAttacking", true);
		//addPrecondition("canAct", true);
		addEffect ("stayAlive", true);
		cost = 400f;
		dashSpeed = .1f;
	}

	void Update(){

		Transform curr = GetComponentInParent<Transform> ();
		if (isDashing) {
			curr.position = Vector3.MoveTowards (curr.position, dashTarget, dashSpeed);
		}
		if (curr.position == dashTarget) {
			isDashing = false;
		}
	}

	public override void reset() {
		moved = false;
		target = null;
	}

	public override bool isDone(){
		return moved;
	}

	public override bool requiresInRange(){
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent){
		target = GameObject.Find ("Player");
		Abigail currA = agent.GetComponent<Abigail> ();
		if (target != null && currA.stamina >= (500 - cost) 
			&& !target.GetComponent<PlayerMovement>().isBlocking) {
				return true;
		}
		return false;
	}

	public override bool perform(GameObject agent){
		Abigail currA = agent.GetComponent<Abigail> ();
		if (currA.stamina >= (500 - cost) && !isDashing) {
			
			currA.stamina -= (500 - cost); //to-do: magic number

			Animator currAnim = GetComponentInParent<Animator> ();
			currAnim.Play ("abigailEvade");

			currA.setSpeed (dashSpeed);
			Vector2 point = Random.insideUnitCircle * 5;
			Vector3 targetPoint = new Vector3 (target.transform.position.x + point.x, 
				target.transform.position.y + point.y,
				target.transform.position.z); 

			isDashing = true;
			dashTarget = targetPoint;

			currAnim.SetTrigger ("dashComplete");

			moved = true;
			return true;
		} else {
			return false;
		}
	}
}
