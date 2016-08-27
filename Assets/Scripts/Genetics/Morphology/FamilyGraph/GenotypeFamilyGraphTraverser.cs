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
		GenotypeNode node = CreateSingleNode(root);
		//root.connections.Sort();

		foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => c.isFirst)) {
			// process isFirst nodes, adding them to the first node
			addChildrenFromConnection(node, conn);
		}
		
		foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => !c.isFirst && !c.terminalOnly))
		{
			addChildrenFromConnection(node, conn);
		}
		// apply selfRecursion (does nothing if selfRecusion is 0)
		//GenotypeNode previousNode = node;
		GenotypeNode currentNode = node;
		for (int i = 0; i < root.selfRecursion; i++)
		{
			currentNode = CreateSingleNode(root);
			foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => !c.isFirst && !c.terminalOnly))
			{
				addChildrenFromConnection(currentNode, conn);
			}
			//previousNode = currentNode;
		}
		foreach (GenotypeFamilyConnection conn in root.connections.FindAll(c => c.terminalOnly))
		{
			// process terminaOnly nodes on the lastNode
			addChildrenFromConnection(currentNode, conn);
		}
		return node;
	}

	private void addChildrenFromConnection(GenotypeNode node, GenotypeFamilyConnection conn)
	{
		GenotypeNode child = TraverseFamilyGraph(conn.GetChild());

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
		node.dimensions = Utility.RandomVector3(fNode.minDimension, fNode.maxDimension);
		node.translateFromParent = Utility.RandomVector3(fNode.translateFromParent);
		node.rotateFromParent = Utility.RandomVector3();
		return node;
	}
}
