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
		return this.rawTxt + Utility.GetSkewedRandom(minNum, maxNum);
	}

	public override GenotypeFamilyNode CreateGenotypeNode()
	{
		GenotypeFamilyNode limbNode = new GenotypeFamilyNode();
		limbNode.name = "limb";
		limbNode.minDimension = new Vector3(0.1f, 3, 0.1f);
		limbNode.maxDimension = new Vector3(0.5f, 6, 0.6f);

		limbNode.rotateFromParent = new Vector3(1, 1, 1);
		limbNode.translateFromParent = new Vector3(1, 1, 1);

		limbNode.selfRecursion = Utility.GetSkewedRandom(minNum, maxNum);

		return limbNode;
	}

}
