using UnityEngine;
using LitJson;
using System.Collections;
using System.IO;


/**
 * Read in the chromosome state as json.
 */
public class ChromosomeLoader
{
	StreamReader sr;
	string raw_contents;
	public JsonData contents;


	public Chromosome load(string filePath)
	{
		Chromosome chromosome = new Chromosome();

		sr = new StreamReader(filePath);
		raw_contents = sr.ReadToEnd();
		contents = JsonMapper.ToObject(raw_contents);
		sr.Close();

		chromosome.name = contents["name"].ToString();

		Color body_col = new Color();
		body_col.r = float.Parse(contents["attributes"]["colour"]["r"].ToString());
		body_col.g = float.Parse(contents["attributes"]["colour"]["g"].ToString());
		body_col.b = float.Parse(contents["attributes"]["colour"]["b"].ToString());
		body_col.a = 1;

		Color limb_col = new Color();
		limb_col.r = float.Parse(contents["attributes"]["limb_colour"]["r"].ToString());
		limb_col.g = float.Parse(contents["attributes"]["limb_colour"]["g"].ToString());
		limb_col.b = float.Parse(contents["attributes"]["limb_colour"]["b"].ToString());
		limb_col.a = 1;

		Vector3 body_scale = new Vector3();
		body_scale.x = float.Parse(contents["attributes"]["body_scale"]["x"].ToString());
		body_scale.y = float.Parse(contents["attributes"]["body_scale"]["y"].ToString());
		body_scale.z = float.Parse(contents["attributes"]["body_scale"]["z"].ToString());

		float bjf = float.Parse(contents["attributes"]["base_joint_frequency"].ToString());
		float bja = float.Parse(contents["attributes"]["base_joint_amplitude"].ToString());
		float bjp = float.Parse(contents["attributes"]["base_joint_phase"].ToString());

		ArrayList branches = new ArrayList();
		int num_branches = (int)contents["attributes"]["branches"];
		chromosome.num_recurrences = new int[num_branches];
		for (int j = 0; j < num_branches; j++)
		{
			ArrayList limbs = new ArrayList();
			int recurrences = (int)contents["attributes"]["recurrences"][j];
			chromosome.num_recurrences[j] = recurrences;
			for (int k = 0; k < recurrences; ++k)
			{
				float x = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["position"]["x"].ToString());
				float y = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["position"]["y"].ToString());
				float z = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["position"]["z"].ToString());
				Vector3 position = new Vector3(x, y, z);

				x = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["scale"]["x"].ToString());
				y = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["scale"]["y"].ToString());
				z = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["scale"]["z"].ToString());
				Vector3 scale = new Vector3(x, y, z);

				ArrayList limb = new ArrayList();
				limb.Add(position);
				limb.Add(scale);
				limbs.Add(limb);
			}
			branches.Add(limbs);
		}

		chromosome.body_colour = body_col;
		chromosome.limb_colour = limb_col;
		chromosome.setBodyScale(body_scale);
		chromosome.setBaseFequency(bjf);
		chromosome.setBaseAmplitude(bja);
		chromosome.setBasePhase(bjp);
		chromosome.setBranches(branches);

		return chromosome;
	}
}
