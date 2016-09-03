using UnityEngine;
using System.Collections;

/**
 * Author: Craig Lomax
 * Author: Barry Becker
 * The body is the root element of the creature. Limbs, mouth, eyes are attached to the body.
 * In the future, mouth and eyes might be on limbs.
 */
public class Torso : MonoBehaviour {
	
	Transform _t;
	public Creature crt;
	public MeshRenderer mr;
	public Material mt;
	public Color original_colour;
	
	void Start () {
		_t = transform;
		mr = gameObject.GetComponent<MeshRenderer>();
		crt = _t.parent.gameObject.GetComponent<Creature>();
		tag = "Creature";
	}

	public void setColour (Color c) {
		original_colour = c;
		mr = gameObject.GetComponent<MeshRenderer>();
		mr.material.shader = Shader.Find("Legacy Shaders/Diffuse");
		mr.material.color = c;
	}
	
	public void setScale (Vector3 scale) {
		transform.localScale = scale;	
	}
	
}
