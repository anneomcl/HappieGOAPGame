using UnityEngine;
using System.Collections;

public class PassiveDamage : MonoBehaviour {

	private int health;
	private float timer; 
	public float lowerLife;
	public int damage;

	// Use this for initialization
	void Start () {
		//change later for enemies/player
		health = GameObject.Find ("Player").GetComponent<PlayerMovement> ().health;
		timer = 0.0f;
		lowerLife = .75f;
		damage = 5;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D collision){
		timer += Time.deltaTime;
		if (collision.gameObject.name == "Player") {
			if (timer > lowerLife) {
				timer = 0;
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().health -= damage;
			}
		}
	}
}
