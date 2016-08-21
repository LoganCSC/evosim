using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Traverses a family graph in order to produce a genotype graph to add to a chromosome.
 * @author Barry Becker
 */
public class GenotypeFamilyGraphTraverser
{
	/** 
	 * Constructor 
	 */
	public GenotypeFamilyGraphTraverser()
	{
	}

	/** 
	 * @param root the root of a family graph
	 * @return a family graph defined by the grammar.
	 * This graph will produce the genotype-graph. The genotypy-graph is what will be persisted and mutated,
	 * and is what produces the phenotype (actual physical morphology).
	 */
	public GenotypeNode TraverseFamilyGraph(GenotypeFamilyNode root)
	{
		GenotypeNode node = new GenotypeNode();
		node.dimensions = Utility.RandomVector3(root.minDimension, root.maxDimension);
		node.translateFromParent = Utility.RandomVector3(root.translateFromParent);
		node.rotateFromParent = Utility.RandomVector3();
		root.connections.Sort();

		return new GenotypeNode();
	}

	public string writeAsJson(GenotypeNode root, string indent)
	{
		return "";
	}
	
	public GenotypeNode readFromJson(string json)
	{
		return new GenotypeNode();
	}

	/*
	public void traverseFamilyGraph(GenotypeNode parent, GenotypeFamilyNode node)
	{
		root.connections.Sort();
		return new GenotypeNode();
	}*/
}
