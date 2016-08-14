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

	public override string GetInstanceText()
	{
		return this.rawTxt + GetSkewedRandom(minNumConnects, maxNumConnects);
	}


	public override void ConnectNodes(GenotypeFamilyNode firstNode, GenotypeFamilyNode secondNode)
	{
		GenotypeFamilyConnection connection = new GenotypeFamilyConnection();

		connection.isFirst = false;
		connection.rotateFromParent = new Vector3(0, 0, 0);
		connection.translateFromParent = new Vector3(0.5f, 1.0f, 0.5f);
		connection.terminalOnly = false;
		connection.symmetry = GetSkewedRandom(minNumConnects, maxNumConnects);

		firstNode.AddConnection(connection);
	}

}
