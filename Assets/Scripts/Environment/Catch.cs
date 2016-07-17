using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Catch : MonoBehaviour {
	
	Creature crt;
	
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.name == "root") {
			crt = col.transform.parent.gameObject.GetComponent<Creature>();
			crt.kill();
		}
	}



}
