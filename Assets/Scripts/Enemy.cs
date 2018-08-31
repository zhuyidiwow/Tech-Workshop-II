using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] private Transform[] patrolPoints;
	[SerializeField] private float moveForce;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float stoppingDistance;
	[SerializeField] private float waitingTime;

	private int patrolIndex;
	private bool isForward;

	private Rigidbody rb;
	private Animator animator;
	private Coroutine patrolCoroutine;
	
	public void Reset() {
		animator.ResetTrigger("Attack");
		animator.ResetTrigger("Walk");
		transform.position = patrolPoints[0].position;
		isForward = true;
		patrolIndex = 1;
		if (patrolCoroutine != null) {
			StopCoroutine(patrolCoroutine);
		}
		patrolCoroutine = StartCoroutine(PatrolCoroutine());
	}

	private void OnEnable() {
		EventManager.StartListening(BVWEvent.ResetGame, Reset);
	}

	private void OnDisable() {
		EventManager.StopListening(BVWEvent.ResetGame, Reset);
	}

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		Reset();
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player")) {
			AttackPlayer(other.gameObject.GetComponent<Player>());
		}
	}

	private void AttackPlayer(Player player) {
		StopCoroutine(patrolCoroutine);
		Vector3 playerPos = player.transform.position;
		Vector3 lookAtPos = new Vector3(playerPos.x , transform.position.y, playerPos.z);
		rb.velocity = Vector3.zero;
		transform.LookAt(lookAtPos);
		animator.SetTrigger("Attack");
		player.Die();
	}

	private IEnumerator PatrolCoroutine() {
		while (true) {
			Vector3 patrolTarget = patrolPoints[patrolIndex].position;
			animator.ResetTrigger("Stop");
			animator.SetTrigger("Walk");

			while(Vector3.Distance(transform.position, patrolTarget) >= stoppingDistance) {
				Move(patrolTarget);
				Debug.DrawLine(transform.position, patrolTarget, Color.red);
				yield return null;
			}

			rb.velocity = Vector3.zero;
			animator.ResetTrigger("Walk");
			animator.SetTrigger("Stop");
			yield return new WaitForSeconds(waitingTime * Random.Range(0.8f, 1.2f));

			FindNextIndex();
		}
	}

	private void Move(Vector3 target) {
		Vector3 moveDirection = target - transform.position;
		moveDirection.y = 0;
		moveDirection = moveDirection.normalized;

		rb.AddForce(moveDirection * moveForce);
		
		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}

		transform.LookAt(Vector3.Slerp(transform.position + transform.forward, transform.position + rb.velocity, 0.1f));
		Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
		Debug.DrawLine(transform.position, transform.position + rb.velocity, Color.white);
	}

	private void FindNextIndex() {
		if (isForward) {
			patrolIndex++;
			if (patrolIndex > patrolPoints.Length - 1) {
				patrolIndex -= 2;
				isForward = false;
			}
		} else {
			patrolIndex--;
			if (patrolIndex < 0) {
				patrolIndex = 1;
				isForward = true;
			}
		}
	}
}
