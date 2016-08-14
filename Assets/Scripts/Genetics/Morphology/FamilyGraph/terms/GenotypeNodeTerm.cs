using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * There are only a certain set of terms allowed in the genotype expressions.
 * They must all be defined here. Some take parameters
 */
public abstract class GenotypeNodeTerm : GenotypeTerm
{

	public GenotypeNodeTerm(string txt) : base(txt)
	{
	}

	public abstract GenotypeFamilyNode CreateGenotypeNode();
}
