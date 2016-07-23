using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour, IGOAP {

	public Animator animator;
	public Rigidbody2D rigidBody;
	public BoxCollider2D boxCollider;
	public PlayerMovement player;

	public int health;
	public int strength;
	public int speed;
	public float stamina;
	public float regenRate;
	protected float terminalSpeed;
	protected float initialSpeed;
	protected float acceleration;
	protected float minDist = 1.5f;
	protected float aggroDist = 5f;
	protected bool loop = false;
	protected float maxStamina;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (stamina <= maxStamina) {
			Invoke ("passiveRegen", 1.0f);
		} else {
			stamina = maxStamina;
		}
	}

	public abstract void passiveRegen();

	public HashSet<KeyValuePair<string, object>> getWorldState(){
		HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>> ();
		worldData.Add (new KeyValuePair<string, object> ("damagePlayer", false)); //to-do: change player's state for world data here
		worldData.Add (new KeyValuePair<string, object> ("evadePlayer", false));
		return worldData;
	}

	public abstract HashSet<KeyValuePair<string, object>> createGoalState ();

	public void planFailed (HashSet<KeyValuePair<string, object>> failedGoal){
		
	}

	public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> action){

	}

	public void actionsFinished(){
		
	}

	public void planAborted(GOAPAction aborter){

	}

	public void setSpeed(float val){
		terminalSpeed = val / 10;
		initialSpeed = (val / 10) / 2;
		acceleration = (val / 10) / 4;
		return;
	}

	public virtual bool moveAgent(GOAPAction nextAction){
		float dist = Vector3.Distance (transform.position, nextAction.target.transform.position);
		if (dist < aggroDist) {
			Vector3 moveDirection = player.transform.position - transform.position;

			setSpeed (speed);

			if (initialSpeed < terminalSpeed) {
				initialSpeed += acceleration;
			}

			Vector3 newPosition = moveDirection * initialSpeed * Time.deltaTime;
			transform.position += newPosition;
		}
		if(dist <= minDist) {
			nextAction.setInRange(true);
			return true;
		} else {
			return false;
		}
	}
}
