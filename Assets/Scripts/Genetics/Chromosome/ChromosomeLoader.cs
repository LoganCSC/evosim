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
	private GenotypeGraphDeserializer graphDeserializer = new GenotypeGraphDeserializer();


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

		float bjf = float.Parse(contents["attributes"]["base_joint_frequency"].ToString());
		float bja = float.Parse(contents["attributes"]["base_joint_amplitude"].ToString());
		float bjp = float.Parse(contents["attributes"]["base_joint_phase"].ToString());

		chromosome.setGraph(graphDeserializer.readNode(contents["genotype_graph"]));

		chromosome.setBodyColour(body_col);
		chromosome.setLimbColour(limb_col);
		//chromosome.setBodyScale(body_scale);
		chromosome.setBaseFequency(bjf);
		chromosome.setBaseAmplitude(bja);
		chromosome.setBasePhase(bjp);

		return chromosome;
	}
}
