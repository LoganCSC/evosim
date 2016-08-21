using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Abstract base of genotype family components like nodes and connections.
 * @author Barry Becker
 */
public abstract class GenotypeFamilyComponent
{

	public abstract string ToString(string indent);
}
