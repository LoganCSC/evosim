using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * There are only a certain set of terms allowed in the genotype epxressions.
 * They must all be defined here. Some take parameters
 */
public class VariableTerm : GenotypeTerm
{
	private Dictionary<string, List<string>> productions;

	public VariableTerm(string txt) : base(txt)
	{
	}
	
}
