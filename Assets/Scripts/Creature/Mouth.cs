using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Mouth : MonoBehaviour {

	Foodbit fb;
	Creature crt;
	Transform _t;
	LineRenderer lr;
	Vector3 line_start;
	float line_length = 0.5f;
	Vector3 line_end;
	float line_width = 0.5F;
	int fb_detect_range = 40;
	Object[] fbits;
	GameObject cf;
	Eye eye;

	void Start () {
		_t = transform;
		crt = (Creature)_t.parent.gameObject.GetComponent("Creature");
		lr = (LineRenderer)gameObject.AddComponent("LineRenderer");
		lr.material = (Material)Resources.Load("Materials/mouth_vector");
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.renderer.enabled = true;
		eye = crt.eye.gameObject.GetComponent<Eye>();
	}

	void Update () {
		cf = eye.closestFbit;
		if(cf) {
			lr.useWorldSpace = true;
			line_end = new Vector3(cf.transform.position.x, cf.transform.position.y, cf.transform.position.z);
			line_start = _t.position;
			lr.SetPosition(1,line_end);
			resetStart();
		} else {
			lr.useWorldSpace = false;
			line_start = Vector3.zero;
			line_end = new Vector3(0,0,line_length);
			lr.SetPosition(0,line_start);
			lr.SetPosition(1,line_end);
		}
	}
	
	void resetStart () {
		line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0,line_start);
	}
	
	float getDetectRadius() {
		return fb_detect_range;
	}

}