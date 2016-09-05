using System;
using UnityEngine;

/**
 * Traverses a family graph in order to produce a genotype graph to add to a chromosome.
 * @author Barry Becker
 */
public class GenotypeFamilyGraphTraverser
{
	/**  Constructor  */
	public GenotypeFamilyGraphTraverser() {}

	/** 
	 * @param root the root of a family graph
	 * @return a genotype graph defined by the grammar.
	 * The genotypy-graph is what will be persisted and mutated, and is what produces the phenotype (actual physical morphology).
	 */
	public GenotypeNode TraverseFamilyGraph(GenotypeFamilyNode root, int recurse)
	{
		GenotypeNode node = CreateSingleNode(root);

		foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => c.isFirst)) {
			// process isFirst nodes, adding them to the first node
			addChildrenFromConnection(node, conn);
		}
		
		foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => !c.isFirst && !c.terminalOnly))
		{
			addChildrenFromConnection(node, conn);
		}
		if (recurse > 0)
		{
			node.children.Add(TraverseFamilyGraph(root, recurse - 1));
		}
		foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => c.terminalOnly))
		{
			// process terminalOnly nodes on the lastNode
			addChildrenFromConnection(node, conn);
		}
		return node;
	}

	private void addChildrenFromConnection(GenotypeNode node, GenotypeFamilyConnection conn)
	{
		GenotypeFamilyNode familyNode = conn.GetChild();
		GenotypeNode child = TraverseFamilyGraph(familyNode, familyNode.selfRecursion);

		Vector3[] rotation = Utility.GetSymmetricalRotations(child.rotateFromParent, conn.symmetry);
		Vector3[] translate = Utility.GetSymmetricalTranslations(child.translateFromParent, conn.symmetry);
		for (int j = 0; j < conn.symmetry; j++)
		{
			GenotypeNode newChild = child.Copy();
			newChild.rotateFromParent = rotation[j];
			newChild.translateFromParent = translate[j];
			node.children.Add(newChild);
		}
	}

	private GenotypeNode CreateSingleNode(GenotypeFamilyNode fNode)
	{
		GenotypeNode node = new GenotypeNode();
		node.type = fNode.name;
		node.dimensions = Utility.RandomVector3(fNode.minDimension, fNode.maxDimension);
		node.translateFromParent = Utility.RandomVector3(fNode.translateFromParent);
		node.rotateFromParent = Utility.RandomVector3();
		return node;
	}
}
