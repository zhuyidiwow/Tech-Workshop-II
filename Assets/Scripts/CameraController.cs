using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private Player player;
	[SerializeField] private Vector3 offset;

	void Update() {
		transform.position = player.transform.position + offset;
	}
}
