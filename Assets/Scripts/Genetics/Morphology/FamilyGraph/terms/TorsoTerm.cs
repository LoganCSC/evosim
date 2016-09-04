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
		return this.rawTxt + Utility.GetSkewedRandom(minNum, maxNum);
	}

	public override GenotypeFamilyNode CreateGenotypeNode()
	{
		GenotypeFamilyNode torsoNode = new GenotypeFamilyNode();
		torsoNode.name = "torso";
		torsoNode.minDimension = new Vector3(0.6f, 1.0f, 0.6f);
		torsoNode.maxDimension = new Vector3(1.2f, 2.0f, 1.1f);

		torsoNode.rotateFromParent = new Vector3(1, 1, 1);
		torsoNode.translateFromParent = new Vector3(1, 1, 1);

		torsoNode.selfRecursion = Utility.GetSkewedRandom(minNum, maxNum);

		return torsoNode;
	}

}
