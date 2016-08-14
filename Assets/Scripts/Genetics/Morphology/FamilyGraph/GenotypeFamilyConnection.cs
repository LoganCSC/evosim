using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Connects GenotypeFamilyNodes
 * @author Barry Becker
 */
public class GenotypeFamilyConnection
{
	// If true, this connction apples only at the recursive limit of the parent.
	public bool terminalOnly;

	// If grater than 1, then it represents a set of N symmetrical connections to child appendages.
	// For example, 2 means bilateral symmetry.
	public int symmetry;

	// transform from parent (position, orientation, scale)
	public Vector3 translateFromParent;
	public Vector3 rotateFromParent;  // eulerAngles
}

