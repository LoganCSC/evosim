using System;
using System.Collections.Generic;

/**
 * 
 */
public abstract class GenotypeConnectionTerm : GenotypeTerm
{

	public GenotypeConnectionTerm(string txt): base(txt)
	{
	}

	public abstract void ConnectNodes(GenotypeFamilyNode firstNode, GenotypeFamilyNode secondNode);

}
