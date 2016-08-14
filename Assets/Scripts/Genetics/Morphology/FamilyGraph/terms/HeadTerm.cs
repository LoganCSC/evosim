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
		GenotypeFamilyNode headNode = new GenotypeFamilyNode();
		headNode.name = "head";
		headNode.minDimension = new Vector3(3, 4, 4);
		headNode.maxDimension = new Vector3(6, 8, 8);

		headNode.rotateFromParent = new Vector3(1, 1, 1);
		headNode.translateFromParent = new Vector3(1, 1, 1);

		headNode.selfRecursion = 0;

		return headNode;
	}
	
}
