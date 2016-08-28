using UnityEngine;
using System.Collections;
using System.IO;

/**
 * Write out the chromosome state as json.
 * See https://msdn.microsoft.com/en-us/library/bb299886.aspx#intro%5Fto%5Fjson%5Ftopic5
 */
public class ChromosomePersister
{
	private GenotypeGraphSerializer graphSerializer = new GenotypeGraphSerializer();

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
		}},
		""base_joint_frequency"": {7},
		""base_joint_amplitude"": {8},
		""base_joint_phase"": {9},
		";

		Color bodyColor = chromosome.getBodyColour();
		Color limbColor = chromosome.getLimbColour();

		string[] args = {
			name, //cp.Name.text,
			bodyColor.r.ToString(), bodyColor.g.ToString(), bodyColor.b.ToString(),
			limbColor.r.ToString(), limbColor.g.ToString(), limbColor.b.ToString(),
			//chromosome.body_scale.x.ToString(), chromosome.body_scale.y.ToString(), chromosome.body_scale.z.ToString(),
			chromosome.base_joint_frequency.ToString(), chromosome.base_joint_amplitude.ToString(), chromosome.base_joint_phase.ToString()
		};
		json_creature = string.Format(json_creature_pattern, args);

		//json_creature += getRecurrencesJson(chromosome);
		//json_creature += getLimbsJson(chromosome);
		json_creature += graphSerializer.writeAsJsonProperty("genotype_graph", chromosome.getGraph(), "   ");

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
}
