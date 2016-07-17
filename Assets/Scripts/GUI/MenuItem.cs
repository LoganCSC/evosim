using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuItem : MonoBehaviour {

	void OnMouseEnter () {
		GetComponent<Renderer>().material.color = Color.green;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseUp () {
		switch (gameObject.name) {
			case "Quit":  Application.Quit(); break;
			case "Start": SceneManager.LoadScene(1); break;
		}
	}
	
}
