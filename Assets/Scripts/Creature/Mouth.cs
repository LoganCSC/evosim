using UnityEngine;
using System.Collections;

/**
 *	Author: Craig Lomax
 */
public class Mouth : MonoBehaviour {

	Creature crt;
	Eye eye;
	
	Transform _t;
	LineRenderer lr;
	
	float line_length 	= 0.05F;
	float line_width 	= 0.05F;
	
	int fb_detect_range = 40;
	

	void Start ()
	{
		_t = transform;
		crt = (Creature)_t.parent.parent.gameObject.GetComponent("Creature");
		eye = crt.getEye();
		lr = (LineRenderer)gameObject.AddComponent<LineRenderer>();
		lr.material.shader = Shader.Find("Unlit/Color");
		lr.material.color = Color.green;
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.GetComponent<Renderer>().enabled = true;
	}

	void Update ()
	{
		GameObject cf = eye.targetFbit;
		if(cf && crt.state == Creature.State.persuing_food) {
			drawLineFromMouthToFood(cf);
		} else {
			drawMouth();
		}
	}

	void drawLineFromMouthToFood(GameObject cf)
	{
		lr.useWorldSpace = true;
		lr.SetPosition(1, cf.transform.position);
		resetStart();
	}

	void drawMouth()
	{
		lr.useWorldSpace = false;
		Vector3 line_start = Vector3.zero;
		Vector3 line_end = new Vector3(0, 0, line_length);
		lr.SetPosition(0, line_start);
		lr.SetPosition(1, line_end);
	}
	
	void resetStart () {
		Vector3 line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0, line_start);
	}
	
	float getDetectRadius() {
		return fb_detect_range;
	}
}
