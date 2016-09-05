﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * The genotype graph is the genetic representation of the creature's phenotype (actual morphology).
 * The genotype graph is produced by executing the genotype-family-graph.
 * Different genotype graphs will be produced each time.
 * This genotype graph will be placed in the chromosome, saved/restored, mutated, 
 * and responsible for generating the creature morphology.
 */
public class GenotypeNode
{
	// Things like dimensions, color, position, rotation, joint defintion to parent stored here
	public Vector3 dimensions;
	public Vector3 translateFromParent;
	public Vector3 rotateFromParent;  // eulerAngles
	// the node type is based on the family node name
	public string type;

	public List<GenotypeNode> children = new List<GenotypeNode>();

	public GenotypeNode Copy()
	{
		GenotypeNode newNode = new GenotypeNode();
		newNode.type = this.type;
		newNode.dimensions = Utility.Copy(this.dimensions);
		newNode.translateFromParent = Utility.Copy(this.translateFromParent);
		newNode.rotateFromParent = Utility.Copy(this.rotateFromParent);

		foreach (GenotypeNode child in this.children)
		{
			newNode.children.Add(child.Copy());
		}
		return newNode;
	}
}
