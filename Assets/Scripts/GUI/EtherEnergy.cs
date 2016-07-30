using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Author: Craig Lomax
 */
 
public class EtherEnergy : MonoBehaviour
{
	Text text;

	void Start ()
	{
		text = GetComponent<Text>();
	}

	void OnEnable ()
	{
		Ether.EnergyUpdated += OnUpdated;
	}

	void OnDisable()
	{
		Ether.EnergyUpdated -= OnUpdated;
	}

	void OnUpdated (float n)
	{
		text.text = "Ether energy: " + n.ToString("0.0");
	}
}
