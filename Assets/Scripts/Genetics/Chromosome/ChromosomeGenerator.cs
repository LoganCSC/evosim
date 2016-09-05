using UnityEngine;
using LitJson;
using System.Collections;

/**
 * Author: Craig Lomax
 * Author: Barry Beckeer
 */
public class ChromosomeGenerator
{
	private GenotypeFamilyGraphTraverser traverser = new GenotypeFamilyGraphTraverser();

	/**
	 * Generate a random chromosome.
	 * The genotype graph is responsible for creating the phonotype (creature morphology).
	 */
	public Chromosome GenerateRandomChromosome()
	{
		Settings settings = Settings.getInstance();
		Chromosome chromosome = new Chromosome();

		// random colours
		Color col = Utility.RandomColor();
		chromosome.setBodyColour(col);
		chromosome.setLimbColour(col);

		//Vector3 bodyScale = settings.getRandomBodyScale();
		//chromosome.setBodyScale(bodyScale);

		// create phenotype graph based on the genotype family graph generator
		GenotypeFamilyNode familyGraph = settings.gfg_generator.CreateGenotypeFamilyGraph();

		GenotypeNode graph = traverser.TraverseFamilyGraph(familyGraph, familyGraph.selfRecursion);
		chromosome.setGraph(graph);

		chromosome.setBaseFequency(Random.Range(3, 20));
		chromosome.setBaseAmplitude(Random.Range(3, 6));
		chromosome.setBasePhase(Random.Range(0, 360));
	
		return chromosome;
	}
}
