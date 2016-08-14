using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 */
public class LastConnectionTerm : GenotypeConnectionTerm
{

	public LastConnectionTerm(string txt) : base(txt)
	{
	}

	public override void ConnectNodes(GenotypeFamilyNode firstNode, GenotypeFamilyNode secondNode)
	{
		GenotypeFamilyConnection connection = new GenotypeFamilyConnection();

		connection.isFirst = false;
		connection.rotateFromParent = new Vector3(0.5f, 0, 0);
		connection.translateFromParent = new Vector3(0.5f, 0.5f, 0.5f);
		connection.terminalOnly = true;

		firstNode.AddConnection(connection);
	}

}
