using UnityEngine;
using System.Collections;

/**
 * Spawns new creatures when they are needed.
 */
public class Spawner : MonoBehaviour
{
#pragma warning disable 0414
	public static Spawner instance;
	Logger lg;
	Data d;
	CreatureCount crt_count;
	Ether eth;
	Settings settings;
	GameObject crt;
	static GameObject container;
	Vector3 pos;
#pragma warning restore 0414

	public delegate void Crt(Creature c);
	public static event Crt CreatureSpawned;
	
	void Start()
	{
		settings = Settings.getInstance();
		lg = Logger.getInstance();
		d = Data.getInstance();
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		eth = Ether.getInstance();
		spawnInitialCreatures();
	}
	
	public static Spawner getInstance()
	{
		if (!instance)
		{
			container = new GameObject();
			container.name = "Spawner";
			instance = container.AddComponent(typeof(Spawner)) as Spawner;
		}
		return instance;
	}

	public void spawnInitialCreatures()
	{
		// For each new starting creature, generate random genes and spawn the bugger
		for (int i = 0; i < settings.starting_creatures; i++)
		{
			Debug.Log("spwaning initian " + i);
			float init_energy = settings.creature_init_energy;
			float spread = settings.creature_spread;
			if (eth.enoughEnergy(init_energy))
			{
				Chromosome chromosome = generateRandomChromosome();
				spawn(Utility.RandomVec(-spread, spread, spread), Utility.RandomRotVec(), init_energy, chromosome);
				eth.subtractEnergy(init_energy);
			}
		}
	}
	
	// spawn a new creature based on the attributes specified.
	public void spawn(Vector3 pos, Vector3 rot, float energy, Chromosome chromosome)
	{
		GameObject child = new GameObject();
		child.transform.localPosition = pos;
		child.transform.eulerAngles = Utility.RandomRotVec();
		Creature crt_script = child.AddComponent<Creature>();
		child.tag = "Creature";
		crt_script.invokechromosome(chromosome);
		crt_script.setEnergy(energy);
		CreatureSpawned(crt_script);
	}

	private Chromosome generateRandomChromosome()
	{
		Chromosome chromosome = new Chromosome();

		// random colours
		Color col = Utility.RandomColor();
		chromosome.setColour(col);
		chromosome.setLimbColour(col);

		chromosome.hunger_threshold = settings.hunger_threshold;

		Vector3 rootScale = settings.getRandomRootScale();
		chromosome.setRootScale(rootScale);

		// random initial limbs
		int bs = Random.Range(1, settings.branch_limit + 1);
		chromosome.setNumBranches(bs);
		ArrayList branches = new ArrayList();

		for (int j = 0; j < bs; j++)
		{
			ArrayList limbs = new ArrayList();

			int recurrences = Random.Range(0, settings.recurrence_limit);
			chromosome.num_recurrences[j] = recurrences;
			for (int k = 0; k <= recurrences; k++)
			{
				Vector3 position = Utility.RandomPointOnCubeSurface(rootScale);
				ArrayList limb = new ArrayList();
				limb.Add(position);
				limb.Add(settings.getRandomLimbScale());
				limbs.Add(limb);
			}
			branches.Add(limbs);
		}

		chromosome.setBaseFequency(Random.Range(3, 20));
		chromosome.setBaseAmplitude(Random.Range(3, 6));
		chromosome.setBasePhase(Random.Range(0, 360));
		chromosome.setBranches(branches);
		return chromosome;
	}

}