using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {

	public Color original_colour;

	void Start () {
		tag = "Creature";
	}

	public void setPosition(Vector3 p)
	{
		transform.localPosition = p;
	}

	public Vector3 getPosition () {
		return transform.localPosition;
	}

	public void setScale(Vector3 s)
	{
		transform.localScale = s;
	}

	public Vector3 getScale () {
		return transform.localScale;
	}
	
	public void setColour (Color c) {
		original_colour = c;
		gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Legacy Shaders/Diffuse");
		gameObject.GetComponent<MeshRenderer>().material.color = c;
	}
}
