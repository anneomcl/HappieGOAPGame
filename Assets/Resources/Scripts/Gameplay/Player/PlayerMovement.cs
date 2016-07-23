using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private BoxCollider2D boxCollider; 
	private Animator animator;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	private float speed = 1.0f;
	private float terminalSpeed = 3.0f;
	private float acceleration = 0.5f;
	public int health;
	Vector3 moveDirection;
	public bool isBlocking = false;
	public int defense = 5;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		boxCollider = GetComponent <BoxCollider2D> ();
		rigidBody = GetComponent <Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		moveDirection = Vector3.zero;
		health = GameObject.Find ("Health").GetComponent<Health> ().startHealth;
	}

	// Update is called once per frame
	void Update () {
		moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
		if (speed < terminalSpeed) {
			speed += acceleration;
		}
		Vector3 newPosition = moveDirection * speed * Time.deltaTime;
		transform.position += newPosition;

		if (Input.GetButton ("Fire2")) {
			isBlocking = true;
			animator.Play ("playerBlock");
		} if (Input.GetButtonUp ("Fire2")) {
			isBlocking = false;
			animator.Play ("playerIdle");
		}

		if (health <= 0) {
			Application.LoadLevel (0);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

	}
}
