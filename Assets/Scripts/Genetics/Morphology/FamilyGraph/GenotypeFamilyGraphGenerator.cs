﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Generate a graph representing a certain family of creatures.
 * The graph that is created can create the phonotypes for specific creatures.
 * @author Barry Becker
 */
public class GenotypeFamilyGraphGenerator
{
	// context free grammar production rules
	private GenotypeFamilyGrammar grammar;


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
	 * @return a family graph defined by the grammar.
	 * This graph will produce the genotype-graph. The genotypy-graph is what will be persisted and mutated,
	 * and is what produces the phenotype (actual physical morphology).
	 */
	public GenotypeFamilyNode CreateGenotypeFamilyGraph()
	{
		// start with "creature" and make the sentence by applying production rules.
		//return e.g. "torso:2,Connect:2,limb:2,First,head";
		return GetGraphForNonTerminal(grammar.initialNonTerminal);
	}

	/**
	 * Recursively apply rules, until we have a sentence.
	 * RHS sides are either a node, or a node connection node (where node may be generated by a non-terminal).
	 */
	private GenotypeFamilyNode GetGraphForNonTerminal(string nonTerminal)
	{
		List<List<GenotypeTerm>> rhs = grammar.GetRHS(nonTerminal);
		// randomly pick from the list of possible RHS's
		int rndIdx = UnityEngine.Random.Range(0, rhs.Count);
		//Debug.Log("getExp for " + nonTerminal + " rnd = " + rndIdx + "  count = "+ rhs.Count);
		List<GenotypeTerm> terms = rhs[rndIdx];
		int numTerms = terms.Count;
		GenotypeFamilyNode node;


		if (numTerms == 1)
		{
			node = ((GenotypeNodeTerm)terms[0]).CreateGenotypeNode();
		}
		else if (numTerms == 3)
		{
			node = GetNode(terms[0]);
			GenotypeFamilyNode otherNode = GetNode(terms[2]);
			GenotypeConnectionTerm connection = (GenotypeConnectionTerm)terms[1];
			connection.ConnectNodes(node, otherNode);
		}
		else
		{
			throw new ArgumentException("Unexpected number of terms in RHS rule: " + numTerms);
		}
		return node;
	}

	private GenotypeFamilyNode GetNode(GenotypeTerm term)
	{
		if (term is VariableTerm)
		{
			return GetGraphForNonTerminal(term.ToString());
		}
		else
		{
			return ((GenotypeNodeTerm)term).CreateGenotypeNode();
		}
	}

}
