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

	public Vector3 max_root_scale;
	public Vector3 min_root_scale;

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
	public int age_sexual_maturity;
	public float low_energy_threshold;

	public Settings ()
	{
		sr = new StreamReader(Application.dataPath + "/" + settings_file);
		string raw_contents = sr.ReadToEnd();
		JsonData contents = JsonMapper.ToObject(raw_contents);
		sr.Close();

		initializeFromContents(contents);
	}
	
	public static Settings getInstance ()
	{
		if(!instance) {
			container = new GameObject();
			container.name = "Settings";
			instance = container.AddComponent(typeof(Settings)) as Settings;
		}
		return instance;
	}

	public Vector3 getRandomRootScale()
	{
		return getRandomScale(min_root_scale, max_root_scale);
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
		max_root_scale = new Vector3();
		max_root_scale.x = float.Parse(creature["root"]["max_root_scale"]["x"].ToString());
		max_root_scale.y = float.Parse(creature["root"]["max_root_scale"]["y"].ToString());
		max_root_scale.z = float.Parse(creature["root"]["max_root_scale"]["z"].ToString());

		min_root_scale = new Vector3();
		min_root_scale.x = float.Parse(creature["root"]["min_root_scale"]["x"].ToString());
		min_root_scale.y = float.Parse(creature["root"]["min_root_scale"]["y"].ToString());
		min_root_scale.z = float.Parse(creature["root"]["min_root_scale"]["z"].ToString());

		max_limb_scale = new Vector3();
		max_limb_scale.x = float.Parse(creature["limb"]["max_limb_scale"]["x"].ToString());
		max_limb_scale.y = float.Parse(creature["limb"]["max_limb_scale"]["y"].ToString());
		max_limb_scale.z = float.Parse(creature["limb"]["max_limb_scale"]["z"].ToString());

		min_limb_scale = new Vector3();
		min_limb_scale.x = float.Parse(creature["limb"]["min_limb_scale"]["x"].ToString());
		min_limb_scale.y = float.Parse(creature["limb"]["min_limb_scale"]["y"].ToString());
		min_limb_scale.z = float.Parse(creature["limb"]["min_limb_scale"]["z"].ToString());

		starting_creatures = (int)contents["ether"]["starting_creatures"];
		creature_spread = float.Parse(contents["ether"]["creature_spread"].ToString());
		creature_init_energy = float.Parse(creature["init_energy"].ToString());
		branch_limit = (int)creature["branch_limit"];
		recurrence_limit = (int)creature["recurrence_limit"];

		hunger_threshold = float.Parse(creature["hunger_threshold"].ToString());
		angular_drag = float.Parse(creature["angular_drag"].ToString());
		drag = float.Parse(creature["drag"].ToString());

		line_of_sight = (double)creature["line_of_sight"];
		metabolic_rate = float.Parse(creature["metabolic_rate"].ToString());
		age_sexual_maturity = (int)creature["age_sexual_maturity"];
		low_energy_threshold = float.Parse(creature["low_energy_threshold"].ToString());
	}
}
