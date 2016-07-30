using UnityEngine;
using System.Collections;


/**
 * Handles events that happen between multiple objects of the same type.
 *	Author: Craig Lomax
 *	Author: Barry Becker
 */
public class CollisionObserver : MonoBehaviour {

	public static CollisionObserver instance;
	public static GameObject container;
	public CollEvent evt;
	public ArrayList collision_events;
	public Ether ether;

	Settings settings;

	float energy_scale;
	double crossover_rate;
	double mutation_rate;
	float mutation_factor;

	void Start () {
		collision_events = new ArrayList();
		ether = Ether.getInstance();
		settings = Settings.getInstance();
		energy_scale = settings.energy_scale;
		crossover_rate = settings.crossover_rate;
		mutation_rate = settings.mutation_rate;
		mutation_factor = settings.mutation_factor;
	}

	public static CollisionObserver getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Collision Observer";
			instance = container.AddComponent<CollisionObserver>();
		}
		return instance;
	}

	/**
	 * This is called from the Eye to determine what object is closest.
	 * TODO: If both creatures have > some thresh energy
	 * then mate them and give 1/4 (or 1/3)of energy from each to offspring, else fight.
	 * Another approach would be to transfer new_child_energy/2 from each parent.
	 */
	public void observe(Creature a, Creature b) {
		collision_events.Add(new CollEvent(a.gameObject, b.gameObject));
		CollEvent bumped = findMatch(a.gameObject, b.gameObject);
		// If a duplicated (bumped) event has been found spawn a child by mating the two bumped creatures
		if (bumped != null) {
			collision_events.Clear();
			DoMating(a, b);
		} else {
			collision_events.Add(new CollEvent(b.gameObject, a.gameObject));
		}
	}

	public void DoMating(Creature a, Creature b)
	{
		Vector3 pos = (a.transform.position - b.transform.position) * 0.5F + b.transform.position;

		// Get references to the scripts of each creature
		Creature crt1 = a; //.transform.parent.GetComponent<Creature>();
		Creature crt2 = b; //.transform.parent.GetComponent<Creature>();

		float crt1_energy = crt1.getEnergy();
		float crt2_energy = crt2.getEnergy();

		Mutator mutator = new global::Mutator(crt1.chromosome);
		Chromosome newChromosome = mutator.doMutating(crt2.chromosome, crossover_rate, mutation_rate, mutation_factor);
		Debug.Log("BREEDING!     "  + newChromosome.ToString());

		float crt1_energy_to_child = (crt1_energy * energy_scale);
		float crt2_energy_to_child = (crt2_energy * energy_scale);
		float new_crt_energy = (crt1_energy_to_child + crt2_energy_to_child);

		ether.spawner.spawn(pos, Vector3.zero, new_crt_energy, newChromosome);

		crt1.setEnergy(crt1_energy - crt1_energy_to_child);
		crt2.setEnergy(crt2_energy - crt2_energy_to_child);

		crt1.num_offspring++;
		crt2.num_offspring++;
	}

	private CollEvent findMatch(GameObject crt1, GameObject crt2) {
		foreach (CollEvent e in collision_events) {
			GameObject event1 = e.getColliders()[0];
			GameObject event2 = e.getColliders()[1];
			// if the object signalling the collision exists in another event - there is a duplicate
			if (crt2.GetInstanceID() == event1.GetInstanceID() || crt1.GetInstanceID() == event2.GetInstanceID()) {
				return e;
			}
		}
		return null;
	}

}
