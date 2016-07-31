using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 */
public class MultiConnectionTerm : GenotypeConnectionTerm
{
	private int minNumConnects;
	private int maxNumConnects;

	public MultiConnectionTerm(string txt, int minNumConnects, int maxNumConnects) : base(txt)
	{
		this.minNumConnects = minNumConnects;
		this.maxNumConnects = maxNumConnects;
	}
	
	public override void ConnectNodes(GenotypeFamilyNode firstNode, GenotypeFamilyNode secondNode)
	{
	}
	
}
