using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * There are only a certain set of terms allowed in the genotype epxressions.
 * They must all be defined here. Some take parameters
 */
public abstract class GenotypeTerm
{
	protected string rawTxt;

	public GenotypeTerm(string txt)
	{
		rawTxt = txt;
	}

	/**
	 * @return a specific instance of the term 
	 * For example if the term is something like limbs:1:4
	 * then this might return limbs:3
	 */
	public virtual string GetInstanceText()
	{
		return rawTxt;
	}

	public override String ToString()
	{
		return rawTxt;
	}

	protected int GetSkewedRandom(int rndMin, int rndMax)
	{
		float max = (float)rndMax - rndMin + 1;
		float r = UnityEngine.Random.Range(0.0f, max);
		float rndVal = (r * r) / max;
		int rnd = rndMin + (int)rndVal;
		if (rnd > rndMax) throw new ArgumentException(rnd + " bigger than " + rndMax);
		return rnd;
	}
}
