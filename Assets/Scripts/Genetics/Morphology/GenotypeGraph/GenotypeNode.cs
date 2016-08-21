using System;
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

	public List<GenotypeNode> children = new List<GenotypeNode>();
}
