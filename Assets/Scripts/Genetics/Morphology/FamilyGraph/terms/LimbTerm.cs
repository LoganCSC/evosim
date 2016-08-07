using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Many creatures have limbs (arms, legs, etc)
 */
public class LimbTerm : GenotypeNodeTerm
{
	private int minNum;
	private int maxNum;


	public LimbTerm(string txt, int minNumSegments, int maxNumSegments) : base(txt)
	{
		minNum = minNumSegments;
		maxNum = maxNumSegments;
	}

	public override string GetInstanceText()
	{
		return this.rawTxt + GetSkewedRandom(minNum, maxNum);
	}
	
	public override GenotypeFamilyNode CreateGenotypeNode()
	{
		return new global::GenotypeFamilyNode();
	}
	
}
