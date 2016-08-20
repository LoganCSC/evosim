using UnityEngine;
using LitJson;
using System.Collections;

/**
 * Author: Craig Lomax
 * Author: Barry Beckeer
 */
public class ChromosomeGenerator
{

	/**
	 * TODO: generate chromosome (with phenotype graph) from generator in settings.
	 */
	public Chromosome GenerateRandomChromosome()
	{
		Settings settings = Settings.getInstance();
		Chromosome chromosome = new Chromosome();

		// random colours
		Color col = Utility.RandomColor();
		chromosome.setBodyColour(col);
		chromosome.setLimbColour(col);

		Vector3 bodyScale = settings.getRandomBodyScale();
		chromosome.setBodyScale(bodyScale);

		// random initial limbs
		int bs = Random.Range(1, settings.branch_limit + 1);
		chromosome.setNumBranches(bs);
		ArrayList branches = new ArrayList();

		for (int j = 0; j < bs; j++)
		{
			ArrayList limbs = new ArrayList();

			int recurrences = Random.Range(0, settings.recurrence_limit);
			chromosome.num_recurrences[j] = recurrences;
			for (int k = 0; k <= recurrences; k++)
			{
				Vector3 position = Utility.RandomPointOnCubeSurface(bodyScale);
				ArrayList limb = new ArrayList();
				limb.Add(position);
				limb.Add(settings.getRandomLimbScale());
				limbs.Add(limb);
			}
			branches.Add(limbs);
		}

		chromosome.setBaseFequency(Random.Range(3, 20));
		chromosome.setBaseAmplitude(Random.Range(3, 6));
		chromosome.setBasePhase(Random.Range(0, 360));
		chromosome.setBranches(branches);
		return chromosome;
	}
}
