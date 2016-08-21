using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Author:	Craig Lomax
 * Author:	Barry Becker
 */
public class Utility
{
	
	// generate random float in the vicinity of n
	public static float RandomApprox(float n, float r)
	{
		return Random.Range(n-r, n+r);
	}
	
	// return a random vector within a given range
	public static Vector3 RandomVec(float x, float y, float z)
{
		return new Vector3( Random.Range(-x, x),
							Random.Range(-y, y),
							Random.Range(-z, z));
	}
	
	// return a random rotation on the y axis
	public static Vector3 RandomRotVec()
	{
		return new Vector3(0.0F,Random.Range(0.0F, 360.0F), 0.0F);
	}
	
	// return a random vector with random eulerian angles in the range 0 - 360
	public static Vector3 RandomVector3()
	{
		return Utility.RandomVector3(new Vector3(360.0F, 360.0F, 360.0F));
	}

	// return a random vector with max values defined by vec.
	public static Vector3 RandomVector3(Vector3 vec)
	{
		return Utility.RandomVector3(new Vector3(0, 0, 0), vec);
	}

	// rturn a random vector within the bounds of the 2 specified vectors.
	public static Vector3 RandomVector3(Vector3 min, Vector3 max)
	{
		return new Vector3( Random.Range(min[0], max[0]),
							Random.Range(min[1], max[1]),
							Random.Range(min[2], max[2]));
	}

	public static Color RandomColor()
	{
		return new Color((float)Random.Range(0.0F, 1.0F),
						(float)Random.Range(0.0F, 1.0F),
						(float)Random.Range(0.0F, 1.0F));
	}

	// return a random point on the surface of a given cube's scale
	public static Vector3 RandomPointOnCubeSurface(Vector3 bounds)
	{
		float side = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
		int axis = Random.Range(0, 3);

		switch (axis)
		{
			case 0:
				return new Vector3(side * bounds.x / 2,
						Random.Range(-bounds.y, bounds.y) / 2,
						Random.Range(-bounds.z, bounds.z) / 2);
			case 1:
				return new Vector3(
						Random.Range(-bounds.x, bounds.x) / 2,
						side * bounds.y / 2,
						Random.Range(-bounds.z, bounds.z) / 2);
			case 2:
				return new Vector3(
						Random.Range(-bounds.x, bounds.x) / 2,
						Random.Range(-bounds.y, bounds.y) / 2,
						side * bounds.z / 2);
			default: throw new System.InvalidOperationException("invalid axis: " + axis);
		}
	}
	
	public static int UnixTimeNow()
	{
		System.TimeSpan t = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0));
		return (int) t.TotalSeconds;
	}
	
	/**
	 * http://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
	 */
	public static float ConvertRange(float old_value, float [] old_range, float [] new_range)
	{
		float new_value;
		float old_extent = (old_range[1] - old_range[0]);
		if (old_extent == 0)
		{
			new_value = new_range[0];
		}
		else
		{
			float new_extent = (new_range[1] - new_range[0]);
			new_value = (((old_value - old_range[0]) * new_extent) / old_extent) + new_range[0];
		}

		return new_value;
	}

	/**
	 * @return a random int in the range [rndMin, rndMax] 
	 * that is skewed toward the low end and only occasionally gives high numbers.
	 */
	public static int GetSkewedRandom(int rndMin, int rndMax)
	{
		float range = (float)rndMax - rndMin + 1;
		float r = UnityEngine.Random.Range(0.0f, range);
		float rndVal = (r * r) / range;
		int rnd = rndMin + (int)rndVal;
		if (rnd > rndMax) throw new System.ArgumentException(rnd + " bigger than " + rndMax);
		return rnd;
	}
}
