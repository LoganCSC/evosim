using UnityEngine;
using System.Collections;

/**
 *		Author: 	Craig Lomax
 */
public class Foodbit : MonoBehaviour
{
	public static float foodbitHeight = 1.0F;
	
	Ether eth;
	MeshRenderer mr;

	public float energy;
	float decay_amount;
	float destroy_at;
	float decay_time;
	float decay_rate;

	 void Start ()
	 {
		name = "Foodbit";
		
		eth = Ether.getInstance();

		mr = GetComponent<MeshRenderer>();
		mr.sharedMaterial = (Material)Resources.Load("Materials/Foodbit");

		Collider co = GetComponent<SphereCollider>();
		co.isTrigger = true;
	 }

	public void destroy ()
	 {
		eth.removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}

}
