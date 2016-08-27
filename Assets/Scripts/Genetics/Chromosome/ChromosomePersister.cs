using UnityEngine;
using System.Collections;
using System.IO;

/**
 * Write out the chromosome state as json.
 */
public class ChromosomePersister
{

	public void save(string path, string name, Chromosome chromosome)
	{
		string json_creature;

		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);

		string filename = path + "/" + name + ".json";
		string json_creature_pattern =
@"{{
	""name"" : ""{0}"",
	""attributes"" : {{
		""colour"" : {{""r"": {1}, ""g"": {2}, ""b"": {3}}},
		""limb_colour"" : {{""r"": {4}, ""g"": {5}, ""b"": {6}}},
		""body_scale"" : {{""x"": {7}, ""y"": {8}, ""z"": {9}
		}},
		""base_joint_frequency"": {10},
		""base_joint_amplitude"": {11},
		""base_joint_phase"": {12},
		""branches"": {13},
		";

		string[] args = {
			name, //cp.Name.text,
			chromosome.body_colour.r.ToString(), chromosome.body_colour.g.ToString(), chromosome.body_colour.b.ToString(),
			chromosome.limb_colour.r.ToString(), chromosome.limb_colour.g.ToString(), chromosome.limb_colour.b.ToString(),
			chromosome.body_scale.x.ToString(), chromosome.body_scale.y.ToString(), chromosome.body_scale.z.ToString(),
			chromosome.base_joint_frequency.ToString(), chromosome.base_joint_amplitude.ToString(), chromosome.base_joint_phase.ToString(),
			chromosome.num_branches.ToString()
		};
		json_creature = string.Format(json_creature_pattern, args);

		json_creature += getRecurrencesJson(chromosome);
		json_creature += getLimbsJson(chromosome);

		json_creature +=
	@"
	}"; // close attributes
		json_creature +=
@"
}"; // close creature root

		using (var sw = new StreamWriter(filename))
		{
			sw.Write(json_creature);
			sw.Close();
		}

		//CreatureSaved();
	}

	private string getRecurrencesJson(Chromosome chromosome)
	{
		string json_recurrences =
		@"""recurrences"" : [";
		for (int i = 0; i < chromosome.num_recurrences.Length; ++i)
		{
			string r_pattern = @"{0}";
			if (!(i == chromosome.num_recurrences.Length - 1))
			{
				r_pattern += @", ";
			}
			json_recurrences += string.Format(r_pattern, chromosome.num_recurrences[i]);
		}
		json_recurrences +=
		@"],
		";
		return json_recurrences;
	}

	private string getLimbsJson(Chromosome chromosome)
	{

		string limbs_json =
		@"""limbs"": {";

		int branch_count = chromosome.getBranchCount();
		for (int i = 0; i < branch_count; ++i)
		{
			string branch_string =
			@"
			""{0}"": [
			";
			limbs_json += string.Format(branch_string, i.ToString());

			ArrayList limbs = chromosome.getLimbs(i);
			for (int k = 0; k < limbs.Count; ++k)
			{
				ArrayList attributes = (ArrayList)limbs[k];
				Vector3 position = (Vector3)attributes[0];
				Vector3 scale = (Vector3)attributes[1];

				string limb_string =
				@"	{{
					""position"" : {{""x"": {1}, ""y"": {2}, ""z"": {3}}},
					""scale"" : {{""x"": {4}, ""y"": {5}, ""z"": {6}}}
				}}";

				if (!(k == limbs.Count - 1))
					limb_string += @",
			";

				string[] l_args = {
						k.ToString(),
						position.x.ToString(), position.y.ToString(), position.z.ToString(),
						scale.x.ToString(), scale.y.ToString(), scale.z.ToString()
				};
				limbs_json += string.Format(limb_string, l_args);
			}

			limbs_json +=
			@"
			]";

			if (!(i == branch_count - 1))
			{
				limbs_json += @",";
			}
		}

		limbs_json +=
		@"
		}"; // close limbs
		return limbs_json;
	}
}
