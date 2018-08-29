using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	public static UIManager Instance;

	[SerializeField] private GameObject winPanel;
	[SerializeField] private GameObject losePanel;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Debug.LogError("An instance of UI manager already exists");
		}
	}

	void Start() {
		Restart();
	}

	public void Win() {
		winPanel.SetActive(true);
	}

	public void Lose() {
		losePanel.SetActive(true);
	}

	public void Restart() {
		winPanel.SetActive(false);
		losePanel.SetActive(false);
		Enemy[] enemies = FindObjectsOfType<Enemy>();
		foreach (Enemy enemy in enemies) {
			enemy.Reset();
		}
	}
}
