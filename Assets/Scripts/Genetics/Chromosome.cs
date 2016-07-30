using UnityEngine;
using LitJson;
using System.Collections;

/**
 * Author: Craig Lomax
 * Author: Barry Beckeer
 */
public class Chromosome
{
	public Color body_colour;
	public Color limb_colour;
	public Vector3 body_scale;

	public float base_joint_frequency;
	public float base_joint_amplitude;
	public float base_joint_phase;
	public float hunger_threshold;

	public int num_branches;
	public int[] num_recurrences;

	public ArrayList branches;

	public void setNumBranches(int n)
	{
		num_branches = n;
		if (num_recurrences == null)
			initNumRecurrences(n);
	}

	void initNumRecurrences(int n)
	{
		num_recurrences = new int[n];
	}

	public int getBranchCount() {
		return branches.Count;
	}

	public ArrayList getLimbs(int index) {
		return (ArrayList) branches[index];
	}

	public Color getBodyColour() {
		return body_colour;
	}

	public Color getLimbColour() {
		return limb_colour;
	}

	public Vector3 getBodyScale() {
		return body_scale;
	}

	public ArrayList getBranches() {
		return branches;
	}

	public void setBranches(ArrayList bs) {
		branches = bs;
	}

	public void setBodyColour(Color c) {
		body_colour = c;
	}

	public void setLimbColour(Color c) {
		limb_colour = c;
	}

	public void setBodyScale(Vector3 rs) {
		body_scale = rs;
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
}
