using System;
using UnityEngine;
/**
 * These connections are always applied first. If there are more than one attached to a node. 
 * The order in which the first connections are applied is not defined, 
 * but they will always be before all other connections.
 * @athor Barry Becker
 */
public class FirstConnectionTerm : GenotypeConnectionTerm
{

	public FirstConnectionTerm(string txt) : base(txt)
	{
	}
	
	public override void ConnectNodes(GenotypeFamilyNode firstNode, GenotypeFamilyNode secondNode)
	{
		GenotypeFamilyConnection connection = new GenotypeFamilyConnection(secondNode);

		connection.isFirst = true;
		connection.rotateFromParent = new Vector3(0, 0, 0);
		connection.translateFromParent = new Vector3(0.5f, 0.5f, 0.5f);
		connection.terminalOnly = false;

		firstNode.AddConnection(connection);
	}
	
}
