using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TotalEnergy : MonoBehaviour
{
	Text text;

	void Start()
	{
		text = GetComponent<Text>();
	}

	void OnEnable()
	{
		Ether.EnergyInitialised += OnStarted;
	}

	void OnDisable()
	{
		Ether.EnergyInitialised -= OnStarted;
	}

	void OnStarted(float n)
	{
		text.text = "Total energy: " + n.ToString("0");
	}
}
