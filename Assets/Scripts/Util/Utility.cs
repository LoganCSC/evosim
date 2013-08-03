using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

 

public class Utility : MonoBehaviour {

	 // generate random float in the vicinity of n
	 public static float RandomApprox(float n, float r) {
		return Random.Range(n-r, n+r);
	}
	
	//return a random 'flat' vector within a given range
	public static Vector3 RandomFlatVec(float x, float y, float z) {
		return new Vector3( Random.Range(-x,x),
							y / 2,
							Random.Range(-z,z)
			              );
	}
	
	//return a random rotation on the y axis
	public static Vector3 RandomRotVec() {
		return new Vector3(0,Random.Range(0,360),0);
	}
	
	//return a random point inside a given cube's scale
	public static Vector3 RandomPointInsideCube(Vector3 bounds) {
		return new Vector3 ( Random.Range (-bounds.x, bounds.x),
							 Random.Range (-bounds.y, bounds.y),
							 Random.Range (-bounds.z, bounds.z)
						   );
	}
}
