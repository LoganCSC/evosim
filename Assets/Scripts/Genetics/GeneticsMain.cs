using UnityEngine;
using System.Collections;

/**
 *		Author: Craig Lomax
 *		Author: Barry Becker
 */
public class GeneticsMain : MonoBehaviour
{
	public static GameObject container;
	public static GeneticsMain instance;
	
	void Start () {
		Settings settings = Settings.getInstance();
		Ether eth = Ether.getInstance();

		// For each new creature, generate random genes and spawn the bugger
		for (int i=0; i<settings.starting_creatures; i++) {
			Chromosome chromosome = new Chromosome();
			
			// random colours
			Color col = new Color( (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F)
								 );
			chromosome.setColour(new Color(col.r, col.g, col.b));
			chromosome.setLimbColour(new Color(col.r, col.g, col.b));

			chromosome.hunger_threshold = settings.hunger_threshold;

			Vector3 rootScale = settings.getRandomRootScale();
			chromosome.setRootScale(rootScale);
			
			// random initial limbs
			int bs = Random.Range (1, settings.branch_limit + 1);
			chromosome.setNumBranches(bs);
			ArrayList branches = new ArrayList();

			for (int j=0; j<bs; j++) {
				ArrayList limbs = new ArrayList();

				int recurrences = Random.Range(0, settings.recurrence_limit);
				chromosome.num_recurrences[j] = recurrences;
				for (int k=0; k<=recurrences; k++) {

					Vector3 position = Utility.RandomPointOnCubeSurface(rootScale);

					ArrayList limb = new ArrayList();
					limb.Add  (position);
					limb.Add  (settings.getRandomLimbScale());
					limbs.Add (limb);
				}
				branches.Add(limbs);
			}

			chromosome.setBaseFequency (Random.Range (3,20));
			chromosome.setBaseAmplitude (Random.Range (3,6));
			chromosome.setBasePhase (Random.Range (0,360));
			chromosome.setBranches(branches);

			float init_energy = settings.creature_init_energy;
			float spread = settings.creature_spread;
			if (eth.enoughEnergy(init_energy)) {
				eth.spawner.spawn(Utility.RandomVec(-spread, spread, spread), Utility.RandomRotVec(), init_energy, chromosome);
				eth.subtractEnergy(init_energy);
			}
		}
	}
	
	public static GeneticsMain getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "GeneticsMain";
			instance = container.AddComponent<GeneticsMain>();
		}
		return instance;
	}
}
