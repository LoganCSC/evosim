using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreatureList : MonoBehaviour
{
	public GameObject button_prefab;
	public List<GameObject> selections;

	void Start () {}
	
	/*
	// I did not think that this was needed, but wiuthout it there is a SavedCreature npe.
	void OnEnable ()
	{
		SveCreature.CreatureSved += OnUpdate;
	}

	void OnDisable ()
	{
		SveCreature.CreatureSved -= OnUpdate;
	}*/
	
	public void PopulateMenu (SortedList<string, Chromosome> creatures)
	{
		DepopulateMenu();  // first remove all items
	
		for (int i = 0; i < creatures.Count; ++i)
		{
			string name = creatures.Keys[i];
			Chromosome chromosome = creatures.Values[i];
			GameObject button = Instantiate(button_prefab, transform.position, transform.rotation) as GameObject;
			button.transform.SetParent(transform);
			LayoutElement le = button.AddComponent<LayoutElement>();
			le.minHeight = 30;

			LoadChromosomeButton lc = button.GetComponent<LoadChromosomeButton>();
			lc.parent = GetComponentInParent<UIElement>();
			lc.c = chromosome;

			Text t = button.GetComponentInChildren<Text>();
			t.text = name;

			Image[] colours = GameObject.FindObjectsOfType<Image>();
			colours[0].color = chromosome.getBodyColour();
			colours[1].color = chromosome.getLimbColour();

			button.AddComponent<Image>();

			RectTransform rt = button.GetComponent<RectTransform>();
			rt.localScale = new Vector3(1, 1, 1);

			selections.Add(button);
		}
	}

	public void DepopulateMenu ()
	{
		foreach (var s in selections)
		{
			Destroy(s);
		}
		selections.Clear();
	}

	
	void OnUpdate ()
	{
		DepopulateMenu();
		GetComponentInParent<ImportCreature>().LoadCreatures();
		//PopulateMenu(creature_info.creatures);
	}
}
