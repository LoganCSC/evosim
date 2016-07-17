using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *		Author: 	Craig Lomax
 *		Author:     Barry Becker
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 */
public class Mutator
{
	static System.Random rnd = new System.Random();

    private Chromosome chromo;

    /** Constructor that takes teh chomosome to mutate */
    public Mutator(Chromosome chromosome)
    {
        chromo = chromosome;
    }

    /** Do crossover and mutation with another chromosome (the mate) */
    public Chromosome doMutating(Chromosome otherChromosome, double crossover_rate, double mutation_rate, float mutation_factor)
    {
        Chromosome newChromosome;
        newChromosome = crossover(otherChromosome, crossover_rate);
        newChromosome = mutate(newChromosome, mutation_rate, mutation_factor);
        return newChromosome;
    }

    private Chromosome mutate(Chromosome newChromo, double rate, float factor)
    {
		// Mutate colour
		float[] cs = new float[3];
		Color cc = newChromo.getColour();
        double rand;

		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++)
        {
			rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		newChromo.setColour(new Color(cs[0], cs[1], cs[2]));
		
		// Mutate root scale
		Vector3 rc = newChromo.getRootScale();

		if (rc.x > 1F && rc.y > 1F && rc.z > 1F)
        {
			float[] rs = new float[3];
			rs[0] = rc.x;
			rs[1] = rc.y;
			rs[2] = rc.z;
			for (int i=0; i<3; i++)
            {
				rand = rnd.NextDouble();
				if (rand < rate)
					rs[i] += randomiseGene(factor);
			}
			Vector3 rootScale = new Vector3 (rs[0], rs[1], rs[2]);
			newChromo.setRootScale(rootScale);
		}
		
		// mutate limbs
		cc = newChromo.getLimbColour();
		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++)
        {
			rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		newChromo.setLimbColour(new Color(cs[0], cs[1], cs[2]));

		ArrayList branches = newChromo.branches;
		for (int b=0; b<branches.Count; b++)
        {
			ArrayList limbs = (ArrayList) branches[b];
			for (int i=0; i<limbs.Count; i++)
            {
				ArrayList limb = (ArrayList) limbs[i];
				Vector3 v = (Vector3) limb[1];
				for (int k=0; k<3; k++)
                {
					rand = rnd.NextDouble();
					if(rand < rate)
						v[k] += randomiseGene(factor);
					}

			}
		}

		// mutate base frequency and amplitude
		rand = rnd.NextDouble();
		if(rand < rate)
        {
			newChromo.base_joint_amplitude += randomiseGene(factor);
		}

		rand = rnd.NextDouble();
		if(rand < rate)
        {
			newChromo.base_joint_frequency += randomiseGene(factor);
		}

		rand = rnd.NextDouble();
		if(rand < rate)
        {
			newChromo.base_joint_phase += randomiseGene(factor);
		}

		rand = rnd.NextDouble();
		if(rand < rate)
        {
			newChromo.hunger_threshold += (decimal)randomiseGene(factor);
		}

		newChromo.setBranches(branches);
		return newChromo;
	}

    private Color getMutatedColor(Color c1, Color c2)
    {
        float r = (.5F * c1.r) + (.5F * c2.r);
        float g = (.5F * c1.g) + (.5F * c2.g);
        float b = (.5F * c1.b) + (.5F * c2.b);
        return new Color(r, g, b);
    }
	
	private Chromosome crossover(Chromosome c2, double rate)
    {
		Chromosome newChromo = new Chromosome();
		
		// Crossover colours
		newChromo.setColour(getMutatedColor(chromo.getColour(), c2.getColour()));
        newChromo.setLimbColour(getMutatedColor(chromo.getLimbColour(), c2.getLimbColour()));

        crossoverLimbs(c2, newChromo, rate);

        crossoverSine(c2, newChromo);

        return (newChromo);
	}

    private void crossoverLimbs(Chromosome c2, Chromosome newChromo, double rate)
    {
        ArrayList c1_branches = chromo.branches;
        ArrayList c2_branches = c2.branches;
        ArrayList c_branches;

        // Randomly select the parent from which the child will derive its limb structure
        int select = Random.Range(0, 2);
        ArrayList other_crt_branches;
        if (select == 0)
        {
            c_branches = c1_branches;
            other_crt_branches = c2_branches;
        }
        else
        {
            c_branches = c2_branches;
            other_crt_branches = c1_branches;
        }

        select = Random.Range(0, 2);
        if (select == 0)
        {
            newChromo.setRootScale(chromo.getRootScale());
        }
        else
        {
            newChromo.setRootScale(c2.getRootScale());
        }

        // Randomly select attributes from the selected creature's limbs to
        //	assign to child creature's limbs

        newChromo.num_recurrences = new int[c_branches.Count];
        for (int i = 0; i < c_branches.Count; i++)
        {
            ArrayList c_limbs = (ArrayList)c_branches[i];
            newChromo.num_recurrences[i] = c_limbs.Count;

            int index;
            for (int j = 1; j < c_limbs.Count; j++)
            {
                ArrayList c_attributes = (ArrayList)c_limbs[j];

                //select random limb segment from other creature
                index = Random.Range(0, other_crt_branches.Count);
                ArrayList other_crt_limbs = (ArrayList)other_crt_branches[index];

                index = Random.Range(0, other_crt_limbs.Count);
                ArrayList other_crt_attributes = (ArrayList)other_crt_limbs[index];

                Vector3 c_scale = (Vector3)c_attributes[1];
                Vector3 other_crt_scale = (Vector3)other_crt_attributes[1];
                for (int s = 0; s < 3; s++)
                {
                    double rand = rnd.NextDouble();
                    if (rand < rate)
                    {
                        c_scale[s] = other_crt_scale[s];
                    }
                }

                //select random limb segment from other creature
                other_crt_limbs = (ArrayList)other_crt_branches[Random.Range(0, other_crt_branches.Count)];
                other_crt_attributes = (ArrayList)other_crt_limbs[Random.Range(0, other_crt_limbs.Count)];

                Vector3 c_pos = (Vector3)c_attributes[0];
                Vector3 other_crt_pos = (Vector3)other_crt_attributes[0];
                for (int p = 0; p < 3; p++)
                {
                    double rand = rnd.NextDouble();
                    if (rand < rate)
                    {
                        c_pos[p] = other_crt_pos[p];
                    }
                }
            }
            c_branches[i] = c_limbs;
            newChromo.num_branches = c_branches.Count;
        }
        newChromo.setBranches(c_branches);
    }

    private void crossoverSine(Chromosome c2, Chromosome newChromo)
    {
        // Crossover frequency and amplitude
        double rand = rnd.NextDouble();
        newChromo.base_joint_amplitude = rand < 0.5f ? chromo.base_joint_amplitude : c2.base_joint_amplitude;

        rand = rnd.NextDouble();
        newChromo.base_joint_frequency = rand < 0.5f ? chromo.base_joint_frequency : c2.base_joint_frequency;

        rand = rnd.NextDouble();
        newChromo.base_joint_phase = rand < 0.5f ? chromo.base_joint_phase : c2.base_joint_phase;

        rand = rnd.NextDouble();
        newChromo.hunger_threshold = rand < 0.5f ? chromo.hunger_threshold : c2.hunger_threshold;
    }

	public static float similar_colour (Chromosome c1, Chromosome c2)
    {
		Color colour1 = c1.getColour();
		Color colour2 = c2.getColour();

        //return Mathf.Abs((colour1.r * colour2.r) - (colour1.g * colour2.g) - (colour1.b * colour2.b)); // this seems wrong
        return Mathf.Abs(Mathf.Abs(colour1.r - colour2.r) + Mathf.Abs(colour1.g - colour2.g) + Mathf.Abs(colour1.b - colour2.b));
    }
	
	private static float randomiseGene(float factor)
    {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}
