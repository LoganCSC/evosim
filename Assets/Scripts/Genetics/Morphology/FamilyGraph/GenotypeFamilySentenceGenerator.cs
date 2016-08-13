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
public class GenotypeFamilySentenceGenerator
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
	public GenotypeFamilySentenceGenerator(GenotypeFamilyGrammar gfGrammar)
	{
		grammar = gfGrammar;
	}

	/** 
	 * @return a sentence in the language defined by the grammar.
	 * This sentence will be turned into genotype-family graph.
	 * This graph will produce the genotype-graph. The genotypy-graph is what will be persisted and mutated,
	 * and is what produces the phenotype (actual physical morphology).
	 */
	public string createSentence()
	{
		// start with "creature" and make the sentence by applying production rules.
		//return e.g. "torso:2,Connect:2,limb:2,First,head";
		return getExpressionForNonTerminal(grammar.initialNonTerminal);
	}

	/**
	 * Recursively apply rules, until we have a sentence.
	 */
	private string getExpressionForNonTerminal(string nonTerminal)
	{

		string expression = "";
		List<List<GenotypeTerm>> rhs = grammar.GetRHS(nonTerminal);
		// randomly pick from the list of possible RHS's
		int rndIdx = UnityEngine.Random.Range(0, rhs.Count);
		//Debug.Log("getExp for " + nonTerminal + " rnd = " + rndIdx + "  count = "+ rhs.Count);
		List<GenotypeTerm> terms = rhs[rndIdx];
		foreach (GenotypeTerm term in terms)
		{
			if (term is VariableTerm)
			{
				expression += getExpressionForNonTerminal(term.ToString()) + ",";
			}
			else
			{
				expression += term.GetInstanceText() + ",";
			}
		}
		return expression.Substring(0, expression.Length - 1);
	}
}
