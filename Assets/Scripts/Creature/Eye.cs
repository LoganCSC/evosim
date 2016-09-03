using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour
{
	private static System.Random RND = new System.Random();
	Creature crt;
	Foodbit fbit;
	public Creature targetCrt = null;
	public GameObject targetFbit= null;
	CollisionObserver observer;
	public float curr_dist = 0f;
	public Collider[] cs;
	Transform _t;
	Settings settings;
	Creature other_crt;
	public GameObject goal = null;
	public float distance_to_goal = 0F;
	Transform torso;
	
	void Start ()
	{
		_t = transform;
		
		crt = _t.parent.parent.gameObject.GetComponent<Creature>();
		observer = CollisionObserver.getInstance();
		settings = Settings.getInstance();
		torso = _t.parent;

		InvokeRepeating("refreshVision", 0, settings.eye_refresh_rate);
	}

	void refreshVision ()
	{
		targetCrt = null;
		switch (crt.state)
		{
			case Creature.State.persuing_food:
			case Creature.State.searching_for_food:
				closestFoodbit();
				break;
		}
	}

	private float similar_colour(Chromosome c1, Chromosome c2)
	{
		Color colour1 = c1.getBodyColour();
		Color colour2 = c2.getBodyColour();

		//return Mathf.Abs((colour1.r * colour2.r) - (colour1.g * colour2.g) - (colour1.b * colour2.b)); // this seems wrong
		return Mathf.Abs(Mathf.Abs(colour1.r - colour2.r) + Mathf.Abs(colour1.g - colour2.g) + Mathf.Abs(colour1.b - colour2.b));
	}
	
	void closestFoodbit ()
	{
		targetFbit = null; // reference to the script of the closest foodbit
		GameObject closest 	= null;
		float dist = Mathf.Infinity;
		cs = Physics.OverlapSphere(_t.position, (float)crt.line_of_sight);

		foreach (Collider c in cs) {
			GameObject f = (GameObject) c.gameObject;
			if (f && f.name == "Foodbit")
			{
				Vector3 diff = f.transform.position - _t.position;
				float curr_dist = diff.magnitude;
				if (curr_dist < dist)
				{
					closest = f;
					dist = curr_dist;
				}
				if (curr_dist < (float) settings.fb_eat_range && (crt.state == Creature.State.persuing_food))
				{
					fbit = f.GetComponent<Foodbit>();
					crt.energy += fbit.energy;
					fbit.destroy();
					crt.food_eaten++;
				}
			}

			// This mating heuristic is not very good
			// instead, just have a timer that periodically checks the number of creatures and if < threshold, mates until back to desired number.
			if (c && c.gameObject.name == "torso" && c != crt.getTorsoObject().gameObject)
			{
				other_crt = c.transform.parent.GetComponent<Creature>();
				Vector3 distDiff = c.transform.position - _t.position;
				float similarityDiff = similar_colour(crt.chromosome, other_crt.chromosome); // avoid mating with relatives
				if (distDiff.magnitude < (float)settings.crt_mate_range && similarityDiff > 0.5 && RND.NextDouble() < 0.5)
				{
					//Debug.Log("crt1: " + crt + " other crt: " + other_crt + " diff.magnitude=" + distDiff.magnitude + " similarityDiff =" + similarityDiff);
					//observer.DoMating(crt, other_crt);
					observer.observe(crt, other_crt);
				}
			}
		}

		distance_to_goal = 0F;
		if (closest)
		{
			targetFbit = closest.gameObject;
		}

		goal = targetFbit;
		if (goal)
		{
			distance_to_goal = distanceToGoal(goal);
		}
		//UpdateDistanceToGoal(goal);
	}

	public float distanceToGoal (GameObject goal)
	{
		if (goal)
			return Vector3.Distance(torso.position, goal.transform.position);
		else
			return 0F;
	}
}
