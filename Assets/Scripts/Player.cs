using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private enum EPlayerState {
		Idle,
		Walking,
		Won,
	}

	[SerializeField] private float stoppingDistance;
	[SerializeField] private float moveForce;
	[SerializeField] private float maxSpeed;
	[SerializeField] private Transform startPosition;

	private EPlayerState state = EPlayerState.Idle;
	private Vector3 destination;
	private Rigidbody rb;
	private Animator animator;

	public void Win() {
		TransitToWon();
	}

	public void Restart() {
		transform.position = startPosition.position;
		transform.rotation = Quaternion.identity;
		TransitToIdle();
	}

	private void Start() {
		rb = GetComponent<Rigidbody>();	
		animator = GetComponent<Animator>();
	}

	void Update() {
		switch (state) {
			case EPlayerState.Idle:
				TickIdle();
				break;
			case EPlayerState.Walking:
				TickWalking();
				break;
			case EPlayerState.Won:
				return;
		}

		if (Input.GetMouseButtonDown(1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            Physics.Raycast(ray, out hit, 30f);

			if (hit.collider != null && hit.collider.CompareTag("Ground")) {
				TransitToWalking(hit.point);
			}
		}
	}

	private void TickIdle() {
		rb.velocity = Vector3.zero;
		state = EPlayerState.Idle;
	}

	private void TickWalking() {
		Vector3 moveDirection = destination - transform.position;
		moveDirection.y = 0;
		moveDirection = moveDirection.normalized;

		rb.AddForce(moveDirection * moveForce);
		
		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}

		Debug.DrawLine(transform.position, destination, Color.green, Time.deltaTime);
		transform.LookAt(transform.position + rb.velocity);

		if (Vector3.Distance(transform.position, destination) <= stoppingDistance) {
			TransitToIdle();
		}
	}

	private void TransitToWalking(Vector3 newDestination) {
		destination = newDestination;
		state = EPlayerState.Walking;
		animator.ResetTrigger("Stop");
		animator.SetTrigger("Run");

	}

	private void TransitToIdle() {
		rb.velocity = Vector3.zero;
		state = EPlayerState.Idle;
		animator.ResetTrigger("Run");
		animator.SetTrigger("Stop");
	}

	private void TransitToWon() {
		rb.velocity = Vector3.zero;
		state = EPlayerState.Won;
		animator.ResetTrigger("Run");
		animator.SetTrigger("Stop");
	}
}
