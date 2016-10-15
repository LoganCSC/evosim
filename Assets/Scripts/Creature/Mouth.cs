using UnityEngine;
using System.Collections;

/**
 *	Author: Craig Lomax
 */
public class Mouth : MonoBehaviour {

	Creature creature;
	Eye eye;
	LineRenderer lineRenderer;

	// dimensions of line draw connecting creature's mouth to foodbit.
	float line_length = 0.05F;
	float line_width = 0.15F;
	
	int fb_detect_range = 40;
	

	void Start ()
	{
		creature = (Creature)transform.parent.parent.gameObject.GetComponent("Creature");
		eye = creature.getEye();
		lineRenderer = (LineRenderer)gameObject.AddComponent<LineRenderer>();
		lineRenderer.material.shader = Shader.Find("Unlit/Color");
		lineRenderer.material.color = Color.green;
		lineRenderer.SetWidth(line_width, line_width);
		lineRenderer.SetVertexCount(2);
		lineRenderer.GetComponent<Renderer>().enabled = true;
	}

	void Update ()
	{
		GameObject targetFood = eye.targetFbit;
		if (targetFood && creature.state == Creature.State.persuing_food) {
			//drawLineFromMouthToFood(targetFood);
		} else {
			drawMouth();
		}
	}

	void drawLineFromMouthToFood(GameObject targetFood)
	{
		lineRenderer.useWorldSpace = true;
		lineRenderer.SetPosition(1, targetFood.transform.position);
		resetStart();
	}

	void drawMouth()
	{
		lineRenderer.useWorldSpace = false;
		Vector3 line_start = Vector3.zero;
		Vector3 line_end = new Vector3(0.01F, 0.02F, line_length);
		lineRenderer.SetPosition(0, line_start);
		lineRenderer.SetPosition(1, line_end);
	}

	/** update the end of the line at the creatures mouth as it moves */
	void resetStart () {
		Vector3 line_start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		lineRenderer.SetPosition(0, line_start);
	}
	
	float getDetectRadius() {
		return fb_detect_range;
	}
}
