using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Every creature has a torso, but it may consist of several "torso" segments.
 */
public class TorsoTerm : GenotypeNodeTerm
{
	private int minNum;
	private int maxNum;


	public TorsoTerm(string txt, int minNumSegments, int maxNumSegments) : base(txt)
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
