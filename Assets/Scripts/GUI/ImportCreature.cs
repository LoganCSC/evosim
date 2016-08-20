using UnityEngine;
using LitJson;
using System.Collections;
using System.IO;

public class ImportCreature : MonoBehaviour
{
	string creatures_folder;
	string[] creature_files;

	
	CreatureInfoContainer creature_info;
	ChromosomeLoader cLoader;
	private UIElement ui_element;
	public Transform button_ui_parent;
	GameObject s;

	void Start()
	{
		creature_info = CreatureInfoContainer.getInstance();
		creatures_folder = Application.dataPath + "/data/saved_creatures";
		cLoader = new ChromosomeLoader();
		ui_element = GetComponent<UIElement>();
	}

	public void OnVisible()
	{
		if (ui_element.visible)
		{
			LoadCreatures();
			GetComponentInChildren<CreatureList>().PopulateMenu(creature_info.creatures);
		}

		if (!ui_element.visible)
		{
			GetComponentInChildren<CreatureList>().DepopulateMenu();
		}
	}

	public void LoadCreatures()
	{
		creature_info.creatures.Clear();
		string[] fs;
		creature_files = Directory.GetDirectories(creatures_folder);
		foreach (var s in creature_files)
		{
			fs = Directory.GetFiles(s, "*.json");
			foreach (string file in fs)
			{
				Chromosome chromosome = cLoader.load(file);
				creature_info.Add(chromosome.name, chromosome);
			}
		}
	}
}
