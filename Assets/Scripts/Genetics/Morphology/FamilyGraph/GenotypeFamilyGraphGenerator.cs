using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Generate a graph representing a certain family of creatures.
 * The graph that is created can create the phonotypes for specific creatures.
 * @author Barry Becker
 */
public class GenotypeFamilyGraphGenerator : MonoBehaviour
{
	// context free grammar production rules
	private GenotypeFamilyGrammar grammar;

	private static char PROD_SEP = ';';
	private static string[] RULE_SEP = {"=>"};
	private static char OPTION_SEP = '|';
	private static char TERM_SEP = ',';


	/** 
	 * Constructor 
	 * @param grammar:  a grammar of this form
	 * "<LHS1>=><term1A1>,<term1A2>,...|<term1B1>,<term1B2>,...|...;<LHS2>=><term2A1>,<term2A2>,...|<term2B1>,<term2B2>,...|...;..."
	 * For example:
	 * "creature=>body,F,head;body=>torso:1:8|torso:1:5,C:1:4,limbs;limbs=>limb:1:3"
	 * Where F indicates a connection that needs to be instantiated first,
	 * L indicates a connection that needs to be instantiated last,
	 * And m:n after a term indicates the number of times that it can recur. Typically m is more likely than n.
	 */
	public GenotypeFamilyGraphGenerator(GenotypeFamilyGrammar gfGrammar)
	{
		grammar = gfGrammar;
	}

	/** 
	 * Create a random sentence using the genotype grammar. That sentence will become the graph.
	 * @return the root node of the genotype-family graph.
	 * This graph will produce the genotype-graph. The genotypy-graph is what will be persisted and mutated,
	 * and is what produces the phenotype (actual physical morphology).
	 */
	public GenotypeFamilyNode GenerateGenotypeFamilyGraph()
	{
		//
		return new GenotypeFamilyNode();
	}

	/** Serialize the productions dictionary. Useful for debugging */
	public override String ToString()
	{
		return "gfgg";
	}
}
