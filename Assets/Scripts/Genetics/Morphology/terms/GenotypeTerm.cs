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
	private string rawTxt;

	public GenotypeTerm(string txt)
	{
		rawTxt = txt;
	}

	public override String ToString()
	{
		return rawTxt;
	}
}
