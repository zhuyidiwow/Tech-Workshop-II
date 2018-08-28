using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	public static UIManager Instance;

	[SerializeField] private GameObject winPanel;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Debug.LogError("An instance of UI manager already exists");
		}
	}

	void Start() {
		winPanel.SetActive(false);
	}

	public void Win() {
		winPanel.SetActive(true);
	}

	public void Restart() {
		winPanel.SetActive(false);
	}
}
