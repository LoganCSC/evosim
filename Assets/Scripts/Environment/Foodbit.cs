using UnityEngine;
using System.Collections;

/**
 *		Author: 	Craig Lomax
 */
public class Foodbit : MonoBehaviour
{
	public static float foodbitHeight = 1.0F;
	
	MeshRenderer mr;

	public float energy;

	 void Start ()
	 {
		name = "Foodbit";
		mr = GetComponent<MeshRenderer>();
		mr.sharedMaterial = (Material)Resources.Load("Materials/Foodbit");
	 }

	public void destroy ()
	 {
		Ether.getInstance().removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}

}
