using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/**
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 */
public class Ether : MonoBehaviour
{	
	public static GameObject container;
	public static Ether instance;
	
	GameObject foodbit;

	Logger lg;
	Settings settings;
	Data data;
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
		data = Data.getInstance();
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
			newFoodbit(pos);
		}

		InvokeRepeating("FixEnergyLeak", 1F*60F, 1F*60F);
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
	
	public void newFoodbit (Vector3 pos)
	{
		foodbit_energy = (float)Random.Range(settings.init_energy_min, settings.init_energy_max);
		
		if (enoughEnergy(foodbit_energy))
		{
			GameObject fb = (GameObject)Instantiate(foodbit, pos, Quaternion.identity);
			Foodbit fb_s = fb.AddComponent<Foodbit>();
			fb_s.energy = foodbit_energy;
			subtractEnergy(foodbit_energy);
			float scale = Utility.ConvertRange((float)foodbit_energy, 
				settings.init_energy_min, settings.init_energy_max, 
				settings.init_scale_min, settings.init_scale_max);
			fb.transform.localScale = new Vector3(scale, scale, scale);
			foodbits.Add(fb);
			FoodbitsUpdated(foodbits.Count);
		}
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
			
			newFoodbit(new_pos);
		}
	}
	
	public void removeFoodbit (GameObject fb)
	{
		FoodbitsUpdated(foodbits.Count);
		foodbits.Remove(fb);
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
		float total_crt = data.TotalCreatureEnergy();
		float total_fb = data.TotalFoodbitEnergy();
		float total = energy + total_crt + total_fb;
		print("crt: " + total_crt + "	 fb: " + total_fb + "	 ether: " + energy + "		total: " + total);
		if (total != total_energy)
		{
			float fix = total - total_energy;
			print("Fixing energy leak... "+fix);
			subtractEnergy(fix);
		}
	}
}
