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
	
	//return a random vector within a given range
	public static Vector3 RandomVec(float x, float y, float z)
{
		return new Vector3( Random.Range(-x, x),
							Random.Range(-y, y),
							Random.Range(-z, z)
							);
	}
	
	//return a random rotation on the y axis
	public static Vector3 RandomRotVec()
	{
		return new Vector3(0.0F,Random.Range(0.0F, 360.0F), 0.0F);
	}
	
	public static Vector3 RandomVector3()
	{
		return new Vector3 ( Random.Range(0.0F, 360.0F),
							 Random.Range(0.0F, 360.0F),
							 Random.Range(0.0F, 360.0F)
							);
	}
	
	public static Color RandomColor()
	{
		return new Color((float)Random.Range(0.0F, 1.0F),
						(float)Random.Range(0.0F, 1.0F),
						(float)Random.Range(0.0F, 1.0F)
					);
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
	
	//http://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
	public static float ConvertRange(float old_value, float old_min, float old_max, float new_min, float new_max)
	{
		float new_value;
		float old_range = (old_max - old_min);
		if (old_range == 0)
		{
			new_value = new_min;
		}
		else
		{
			float new_range = (new_max - new_min);
			new_value = (((old_value - old_min) * new_range) / old_range) + new_min;
		}

		return (new_value);
	}
}
