using UnityEngine;
using System.Collections;

/**
 * Spawns new creatures when they are needed.
 */
public class Spawner : MonoBehaviour
{
#pragma warning disable 0414
	public static Spawner instance;
	CreatureCount crt_count;
	Ether eth;
	Settings settings;
	ChromosomeGenerator cGenerator;
	static GameObject container;
#pragma warning restore 0414

	public delegate void Crt(Creature c);
	public static event Crt CreatureSpawned;
	
	void Start()
	{
		settings = Settings.getInstance();
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		eth = Ether.getInstance();
		cGenerator = new ChromosomeGenerator();
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

	/** At startup, add some initial random creatures */
	public void spawnInitialCreatures()
	{
		// For each new starting creature, generate random genes and spawn the bugger
		for (int i = 0; i < settings.starting_creatures; i++)
		{
			float init_energy = settings.creature_init_energy;
			float spread = settings.creature_spread;
			if (eth.enoughEnergy(init_energy))
			{
				Chromosome chromosome = cGenerator.GenerateRandomChromosome();
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
		crt_script.SetChromosome(chromosome);
		crt_script.setEnergy(energy);
		CreatureSpawned(crt_script);
	}

}