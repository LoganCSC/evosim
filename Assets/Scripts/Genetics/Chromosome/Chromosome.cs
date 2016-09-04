using UnityEngine;
using LitJson;
using System.Collections;

/**
 * The chormosome is the phenotype for the creature. 
 * It corresponds directly to the creatures physical characteristics.
 * This is what will be saved/restored and mutated as the creatures evolve.
 * Author: Craig Lomax
 * Author: Barry Becker
 */
public class Chromosome
{
	public string name;

	private Color body_colour;
	private Color limb_colour;
	private Vector3 body_scale;

	public float base_joint_frequency;
	public float base_joint_amplitude;
	public float base_joint_phase;

	private GenotypeNode genotype_graph;


	public Color getBodyColour() {
		return body_colour;
	}
	public void setBodyColour(Color c)
	{
		body_colour = c;
	}

	public Color getLimbColour() {
		return limb_colour;
	}
	public void setLimbColour(Color c)
	{
		limb_colour = c;
	}

	public Vector3 getBodyScale() {
		return body_scale;
	}
	public void setBodyScale(Vector3 bs) {
		body_scale = bs;
	}

	public void setBaseFequency(float freq) {
		base_joint_frequency = freq;
	}

	public void setBaseAmplitude (float amp) {
		base_joint_amplitude = amp;
	}

	public void setBasePhase (float phase)
	{
		base_joint_phase = phase;
	}

	public GenotypeNode getGraph()
	{
		return genotype_graph;
	}

	public void setGraph(GenotypeNode graph)
	{
		genotype_graph = graph;
	}
}
