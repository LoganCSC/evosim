using UnityEngine;
using System.Collections;

public class LoadChromosome : MonoBehaviour
{
	public Chromosome c;
	public UIElement parent;

	Settings s;

	void Start ()
	{
		s = Settings.getInstance();
	}

	public void OnClick ()
	{
		Ether eth = Ether.getInstance();
		eth.spawner.spawn(
			Camera.main.transform.position + new Vector3(0, 0, 10),
			Utility.RandomRotVec(),
			s.init_energy,
			c
		);
		Ether.getInstance().subtractEnergy(s.init_energy);
		parent.make_invisible();
		GetComponentInParent<CreatureList>().DepopulateMenu();
	}
}
