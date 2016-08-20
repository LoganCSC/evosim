using UnityEngine;
using System.Collections;
using System.IO;

/**
 * Write out the creature state as json.
 * The file name will be the id of the creature.
 */
public class SaveCreature : MonoBehaviour
{
	public delegate void Save();
	public CreaturePane cp;
	private ChromosomePersister persister;

	void Start()
	{
		persister = new ChromosomePersister();
	}

	public void save()
	{
		Chromosome chromosome = cp.crt.chromosome;
		int crt_id = Mathf.Abs(cp.crt.gameObject.GetInstanceID());
		string path = Application.dataPath + "/data/saved_creatures/" + crt_id;
		string name = "creature-" + crt_id;

		persister.save(path, name, chromosome);
	}
}
