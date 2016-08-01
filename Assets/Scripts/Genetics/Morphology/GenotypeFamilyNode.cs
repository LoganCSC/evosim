using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * The genotype-family-graph is an abstract representation of a whole family of creatures.
 * When this graph is run it will produce specific genotype-graphs (a different one each time run).
 */
public class GenotypeFamilyNode
{
	public Vector3 minDimension;
	public Vector3 maxDimension;

	public Vector3 translateFromParent;
	public Vector3 rotateFromParent;  // eulerAngles

	// Configurable join parameters (there are a lot)
	// set target angle and use max force for slerp mode
	// set hanlge limits here.
	public 

}
