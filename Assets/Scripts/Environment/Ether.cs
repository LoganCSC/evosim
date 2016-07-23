using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/**
 * Author: Craig Lomax
 * Author: Barry Becker
 * The ether is the liquid that the vvreatures swim in.
 */
public class Ether : MonoBehaviour
{	
	public static GameObject container;
	public static Ether instance;
	
	GameObject foodbit;
	Settings settings;
	public Spawner spawner;
	
	public float total_energy;
	public float energy;
	float foodbit_energy;

	Vector3 pos;

	public ArrayList creatures;
	public ArrayList foodbits;

	public delegate void EtherInfo(float energy);
	public static event EtherInfo EnergyUpdated;
	public static event EtherInfo EnergyInitialised;

	public delegate void FoodbitInfo(int count);
	public static event FoodbitInfo FoodbitsUpdated;

	void OnEnable ()
	{
		Spawner.CreatureSpawned += OnCreatureSpawned;
		Creature.CreatureDead += OnCreatureDeath;
	}

	void OnDisable()
	{
		Spawner.CreatureSpawned -= OnCreatureSpawned;
		Creature.CreatureDead -= OnCreatureDeath;
	}

	void Start ()
	{
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");

		settings = Settings.getInstance();
		spawner = Spawner.getInstance();

		total_energy = settings.total_energy;
		energy = total_energy;
		EnergyInitialised(energy);

		creatures = new ArrayList();
		foodbits = new ArrayList();

		float spread = settings.wide_spread;
		for (int i=0; i<settings.start_number_foodbits; i++)
		{
			Vector3 pos = Utility.RandomVec(-spread, spread, spread);
			addFoodbit(pos);
		}

		// fix energy every minute
		InvokeRepeating("FixEnergyLeak", 60F, 60F);
		InvokeRepeating("fbSpawn", settings.spore_time, settings.spore_time);
	}

	void OnCreatureSpawned (Creature c)
	{
		creatures.Add(c);
	}

	void OnCreatureDeath (Creature c)
	{
		creatures.Remove(c);
	}
	
	/** add a new foodbit to the ether if the either has enough energy. */
	public void addFoodbit (Vector3 pos)
	{
		float[] energy_range = { settings.init_energy_min, settings.init_energy_max };
		foodbit_energy = (float)Random.Range(energy_range[0], energy_range[1]);
		
		if (enoughEnergy(foodbit_energy))
		{
			GameObject fb = (GameObject)Instantiate(foodbit, pos, Quaternion.identity);
			Foodbit fb_s = fb.AddComponent<Foodbit>();
			fb_s.energy = foodbit_energy;
			subtractEnergy(foodbit_energy);
			
			// The volume of the food bit is propertional to its energy value
			float[] scale_range = { settings.init_scale_min, settings.init_scale_max };
			float scale = Utility.ConvertRange(Mathf.Pow(foodbit_energy, 1.0f/3.0f),  energy_range, scale_range);
			fb.transform.localScale = new Vector3(scale, scale, scale);
			foodbits.Add(fb);
			FoodbitsUpdated(foodbits.Count);
		}
	}

	public void removeFoodbit(GameObject fb)
	{
		FoodbitsUpdated(foodbits.Count);
		foodbits.Remove(fb);
	}

	private void fbSpawn()
	{
		int fb_count = getFoodbitCount();
		if (fb_count >= 1)
		{
			int fb_index = Random.Range(0,fb_count);
			GameObject fb = (GameObject) foodbits[fb_index];
			Foodbit fb_script = fb.GetComponent<Foodbit>();
			foodbit_energy = fb_script.energy;
			Vector3 fb_pos = fb_script.transform.localPosition;
			pos = Utility.RandomVec (-settings.spore_range,
									 Foodbit.foodbitHeight / 2,
									 settings.spore_range
									);
			
			Vector3 new_pos = fb_pos + pos;
			float spread = settings.wide_spread;
			if (new_pos.x > spread  || new_pos.x < -spread
				|| new_pos.z > spread || new_pos.z < -spread)
			{
				new_pos = Utility.RandomVec(-spread, spread, spread);
			}
			
			addFoodbit(new_pos);
		}
	}
	
	public int getFoodbitCount ()
	{
		return foodbits.Count;
	}
	
	public static Ether getInstance ()
	{
		if(!instance)
		{
			container = new GameObject();
			container.name = "Ether";
			instance = container.AddComponent(typeof(Ether)) as Ether;
		}
		return instance;
	}
	
	public float getEnergy()
	{
		return energy;
	}

	public void addEnergy (float n)
	{
		energy += n;
		EnergyUpdated(energy);
	}
	
	public void subtractEnergy (float n)
	{
		energy -= n;
		EnergyUpdated(energy);
	}

	public bool enoughEnergy(float n)
	{
		return energy >= n;
	}
	
	private void FixEnergyLeak ()
	{
		float total_crt = TotalCreatureEnergy();
		float total_fb = TotalFoodbitEnergy();
		float total = energy + total_crt + total_fb;
		print("crt: " + total_crt + "	 fb: " + total_fb + "	 ether: " + energy + "		total: " + total);
		if (total != total_energy)
		{
			float fix = total - total_energy;
			print("Fixing energy leak... "+fix);
			subtractEnergy(fix);
		}
	}

	public float TotalCreatureEnergy()
	{
		float result = 0;
		foreach (Creature c in creatures)
		{
			result += c.energy;
		}
		return (result);
	}

	internal float TotalFoodbitEnergy()
	{
		float result = 0;
		foreach (GameObject f in foodbits)
		{
			result += f.GetComponent<Foodbit>().energy;
		}
		return (result);
	}
}
