using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Every creature has a head.
 * It has an eye and mouth.
 */
public class HeadTerm : GenotypeNodeTerm
{

	public HeadTerm(string txt) : base(txt)
	{
	}
	
	public override GenotypeFamilyNode CreateGenotypeNode()
	{
		return new global::GenotypeFamilyNode();
	}
	
}
