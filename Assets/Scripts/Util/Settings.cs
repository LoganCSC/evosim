using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class Settings : MonoBehaviour {
	
	string settings_file = "settings.json";
	StreamReader sr;
	
	public static GameObject container;
	public static Settings instance;

	public int starting_creatures;
	public float creature_spread;

	public Vector3 max_body_scale;
	public Vector3 min_body_scale;

	public Vector3 max_limb_scale;
	public Vector3 min_limb_scale;

	public float creature_init_energy;
	public int branch_limit;
	public int recurrence_limit;
	public float hunger_threshold;

	public float angular_drag;
	public float drag;

	public double line_of_sight;
	public float metabolic_rate;
	public float low_energy_threshold;
	public float init_energy;

	public double crt_mate_range;
	public double fb_eat_range;
	public float eye_refresh_rate;

	public float total_energy;
	public int start_number_foodbits;
	public int spore_range;
	public float wide_spread;
	public float spore_time;

	public float init_energy_min;
	public float init_energy_max;

	public float init_scale_min;
	public float init_scale_max;

	public float energy_scale;
	public double crossover_rate;
	public double mutation_rate;
	public float mutation_factor;

	public float camera_sensitivity;
	public float camera_invert;
	

	public Settings()
	{
		sr = new StreamReader(Application.dataPath + "/" + settings_file);
		string raw_contents = sr.ReadToEnd();
		JsonData contents = JsonMapper.ToObject(raw_contents);
		sr.Close();

		initializeFromContents(contents);
	}
	
	public static Settings getInstance ()
	{
		if (!instance) {
			container = new GameObject();
			container.name = "Settings";
			instance = container.AddComponent(typeof(Settings)) as Settings;
		}
		return instance;
	}

	public Vector3 getRandomBodyScale()
	{
		return getRandomScale(min_body_scale, max_body_scale);
	}

	public Vector3 getRandomLimbScale()
	{
		return getRandomScale(min_limb_scale, max_limb_scale);
	}

	public Vector3 getRandomScale(Vector3 minVec, Vector3 maxVec)
	{
		return new Vector3((float)Random.Range(minVec.x, maxVec.x),
							(float)Random.Range(minVec.y, maxVec.y),
							(float)Random.Range(minVec.z, maxVec.z));
	}

	private void initializeFromContents(JsonData contents)
	{
		JsonData creature = contents["creature"];
		JsonData ether = contents["ether"];
		JsonData foodbit = contents["foodbit"];
		JsonData genetics = contents["genetics"];

		max_body_scale = new Vector3();
		max_body_scale.x = float.Parse(creature["body"]["max_body_scale"]["x"].ToString());
		max_body_scale.y = float.Parse(creature["body"]["max_body_scale"]["y"].ToString());
		max_body_scale.z = float.Parse(creature["body"]["max_body_scale"]["z"].ToString());

		min_body_scale = new Vector3();
		min_body_scale.x = float.Parse(creature["body"]["min_body_scale"]["x"].ToString());
		min_body_scale.y = float.Parse(creature["body"]["min_body_scale"]["y"].ToString());
		min_body_scale.z = float.Parse(creature["body"]["min_body_scale"]["z"].ToString());

		max_limb_scale = new Vector3();
		max_limb_scale.x = float.Parse(creature["limb"]["max_limb_scale"]["x"].ToString());
		max_limb_scale.y = float.Parse(creature["limb"]["max_limb_scale"]["y"].ToString());
		max_limb_scale.z = float.Parse(creature["limb"]["max_limb_scale"]["z"].ToString());

		min_limb_scale = new Vector3();
		min_limb_scale.x = float.Parse(creature["limb"]["min_limb_scale"]["x"].ToString());
		min_limb_scale.y = float.Parse(creature["limb"]["min_limb_scale"]["y"].ToString());
		min_limb_scale.z = float.Parse(creature["limb"]["min_limb_scale"]["z"].ToString());

		creature_init_energy = float.Parse(creature["init_energy"].ToString());
		branch_limit = (int)creature["branch_limit"];
		recurrence_limit = (int)creature["recurrence_limit"];

		hunger_threshold = float.Parse(creature["hunger_threshold"].ToString());
		angular_drag = float.Parse(creature["angular_drag"].ToString());
		drag = float.Parse(creature["drag"].ToString());

		line_of_sight = (double)creature["line_of_sight"];
		metabolic_rate = float.Parse(creature["metabolic_rate"].ToString());
		low_energy_threshold = float.Parse(creature["low_energy_threshold"].ToString());

		crt_mate_range = (double)creature["mate_range"];
		fb_eat_range = (double)creature["eat_range"];
		eye_refresh_rate = float.Parse(creature["eye_refresh_rate"].ToString());

		energy_scale = float.Parse(creature["energy_to_offspring"].ToString());
		init_energy = float.Parse(creature["init_energy"].ToString());

		crossover_rate = (double)genetics["crossover_rate"];
		mutation_rate = (double)genetics["mutation_rate"];
		mutation_factor = float.Parse(genetics["mutation_factor"].ToString());

		starting_creatures = (int)ether["starting_creatures"];
		creature_spread = float.Parse(ether["creature_spread"].ToString());
		total_energy = float.Parse(ether["total_energy"].ToString());
		start_number_foodbits = (int)ether["start_number_foodbits"];

		spore_range = (int)foodbit["spore_range"];
		wide_spread = float.Parse(foodbit["wide_spread"].ToString());
		spore_time = float.Parse(foodbit["spore_time"].ToString());

		init_energy_min = float.Parse(foodbit["init_energy_min"].ToString());
		init_energy_max = float.Parse(foodbit["init_energy_max"].ToString());
		init_scale_min = float.Parse(foodbit["init_scale_min"].ToString());
		init_scale_max = float.Parse(foodbit["init_scale_max"].ToString());

		camera_sensitivity = float.Parse(contents["config"]["camera"]["sensitivity"].ToString());
		camera_invert = float.Parse(contents["config"]["camera"]["invert"].ToString());
	}
}



