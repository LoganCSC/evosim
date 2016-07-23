using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour
{
	Creature crt;
	Foodbit fbit;
	public Creature targetCrt = null;
	public GameObject targetFbit= null;
	CollisionMediator co;
	public float curr_dist = 0f;
	double los;
	
	public Collider[] cs;
	
	Transform _t;
	
	Settings settings;
	Creature other_crt;

	public GameObject goal = null;
	public float distance_to_goal = 0F;
	Transform body;
	
	void Start ()
	{
		_t = transform;
		
		crt = _t.parent.parent.gameObject.GetComponent<Creature>();
		co = CollisionMediator.getInstance();
		settings = Settings.getInstance();
		los = crt.line_of_sight;

		body = _t.parent;

		InvokeRepeating("refreshVision", 0, settings.eye_refresh_rate);
	}

	void refreshVision ()
	{
		targetCrt = null;
		switch (crt.state)
		{
		case Creature.State.persuing_mate:
			most_similar_creature();
			break;
		case Creature.State.searching_for_mate:
			most_similar_creature();
			break;
		case Creature.State.persuing_food:
			closestFoodbit();
			break;
		case Creature.State.searching_for_food:
			closestFoodbit();
			break;
		}
	}
	
	void most_similar_creature()
	{
		targetCrt 				= null;	// reference to the script of the closest creature
		GameObject target 		= null;
		GameObject c 			= null; // current collider being looked at
		float similarity		= Mathf.Infinity;
		float curr_similarity;
		cs = Physics.OverlapSphere(_t.position, (float)los);


		if (cs.Length == 0)
		{
			target = null;
			return;
		}

		foreach (Collider col in cs)
		{
			c = (GameObject) col.transform.gameObject;
			if (c && c.gameObject.name == "body" && c != crt.body.gameObject)
			{
				other_crt = c.transform.parent.GetComponent<Creature>();
				curr_similarity = similar_colour(crt.chromosome, other_crt.chromosome);

				if (curr_similarity < similarity)
				{
					target = c.transform.parent.gameObject;
					similarity = curr_similarity;
				}

				Vector3 diff = c.transform.position - _t.position;
				if (diff.magnitude < (float) settings.crt_mate_range)
				{
					other_crt = c.transform.parent.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate)
					{
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.ChangeState(Creature.State.mating);
						crt.ChangeState(Creature.State.mating);
					}
					similarity = curr_similarity;
				}
			}

			distance_to_goal = 0F;
			goal = null;
			if (target)
			{
				targetCrt = target.GetComponent<Creature>();
				goal = targetCrt.body;
				distance_to_goal = distanceToGoal();
			}
		}
	}


	private float similar_colour(Chromosome c1, Chromosome c2)
	{
		Color colour1 = c1.getBodyColour();
		Color colour2 = c2.getBodyColour();

		//return Mathf.Abs((colour1.r * colour2.r) - (colour1.g * colour2.g) - (colour1.b * colour2.b)); // this seems wrong
		return Mathf.Abs(Mathf.Abs(colour1.r - colour2.r) + Mathf.Abs(colour1.g - colour2.g) + Mathf.Abs(colour1.b - colour2.b));
	}

	private void closestCreature ()
	{
		targetCrt 				= null;	// reference to the script of the closest creature
		GameObject target 		= null;
		GameObject c 			= null; // current collider being looked at
		float dist 				= Mathf.Infinity;
		cs = Physics.OverlapSphere(_t.position, (float)los);

		if (cs.Length == 0)
		{
			target = null;
			return;
		}

		foreach (Collider col in cs)
		{
			c = (GameObject) col.transform.gameObject;
			if (c && c.gameObject.name == "body" && c != crt.body.gameObject)
			{
				Vector3 diff = c.transform.position - _t.position;
				curr_dist = diff.magnitude;
				other_crt = c.transform.parent.GetComponent<Creature>();


				if (curr_dist < dist)
				{
					target = c.transform.parent.gameObject;
					dist = curr_dist;
				}
				if (curr_dist < (float) settings.crt_mate_range)
				{
					other_crt = c.transform.parent.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate)
					{
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.ChangeState(Creature.State.mating);
						crt.ChangeState(Creature.State.mating);
					}
					dist = curr_dist;
				}
			}

			distance_to_goal = 0F;
			goal = null;
			if (target)
			{
				targetCrt = target.GetComponent<Creature>();
				goal = targetCrt.body;
				distance_to_goal = distanceToGoal();
			}
		}	
	}
	
	void closestFoodbit ()
	{
		targetFbit 		= null;	// reference to the script of the closest foodbit
		GameObject closest 	= null;
		float dist 			= Mathf.Infinity;
		cs = Physics.OverlapSphere(_t.position, (float)los);

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
		}

		distance_to_goal = 0F;
		if (closest)
		{
			targetFbit = closest.gameObject;
		}

		goal = targetFbit;
		if (goal)
		{
			distance_to_goal = distanceToGoal();
		}
	}

	public float distanceToGoal ()
	{
		if (goal)
			return Vector3.Distance(body.position, goal.transform.position);
		else
			return 0F;
	}
}
