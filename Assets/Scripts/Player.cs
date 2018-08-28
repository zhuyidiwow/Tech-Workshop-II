using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            Physics.Raycast(ray, out hit, 30f);

			if (hit.collider != null && hit.collider.CompareTag("Ground")) {
				Debug.DrawLine(transform.position, hit.point, Color.green, 1f);
			}
		}
	}
}
