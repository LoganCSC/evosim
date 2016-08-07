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
public class GenotypeFamilyGrammar
{
	// context free grammar production rules
	private Dictionary<string, List<List<GenotypeTerm>>> productions;

	private static char PROD_SEP = ';';
	private static string[] RULE_SEP = {"=>"};
	private static char OPTION_SEP = '|';
	private static char TERM_SEP = ',';

	private string initialNonTerminal;
	


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
	public GenotypeFamilyGrammar(string grammar)
	{
		productions = new Dictionary<string, List<List<GenotypeTerm>>>();
		GenotypeTermParser parser = new GenotypeTermParser();

		string[] rules = grammar.Split(PROD_SEP);
		initialNonTerminal = rules[0].Split(RULE_SEP, StringSplitOptions.None)[0];

		foreach (string rule in rules)
		{
			string[] lhsRhs = rule.Split(RULE_SEP, StringSplitOptions.None);
			List<string> optionsList = lhsRhs[1].Split(OPTION_SEP).ToList();
			List<List<GenotypeTerm>> options = new List<List<GenotypeTerm>>();
			foreach (string opt in optionsList)
			{
				string[] termList = opt.Split(TERM_SEP);
				List<GenotypeTerm> terms = new List<GenotypeTerm>();
				foreach (string termText in termList)
				{
					terms.Add(parser.ParseTerm(termText));
				}
				options.Add(terms);
			}
			productions.Add(lhsRhs[0], options);
		}

		MonoBehaviour.print("grammar dictionary = \n" + this.ToString());
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
		return getExpressionForNonTerminal(initialNonTerminal);
	}

	/**
	 * Recursively apply rules, until we have a sentence.
	 */
	private string getExpressionForNonTerminal(string nonTerminal)
	{

		if (!productions.ContainsKey(nonTerminal))
			throw new ArgumentException("Unexpected nonTerminal: " + nonTerminal);

		string expression = "";
		List<List<GenotypeTerm>> rhs = productions[nonTerminal];
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
		return expression.Substring(0, expression.Length -1);
	}

	/** Serialize the productions dictionary. Useful for debugging */
	public override string ToString()
	{
		StringBuilder bldr = new StringBuilder();
		foreach (KeyValuePair<string, List<List<GenotypeTerm>>> rule in productions)
		{
			bldr.Append(rule.Key + " => ");
			List<string> optionTerms = new List<string>();

			foreach (List<GenotypeTerm> terms in rule.Value)
			{
				List<string> termsList = new List<string>();
				MonoBehaviour.print("terms len " + terms.Count());
				foreach (GenotypeTerm term in terms)
				{
					termsList.Add(term.ToString());
					//MonoBehaviour.print("term " + term.ToString());
				}
				optionTerms.Add(string.Join(", ", termsList.ToArray()));
			}
			bldr.Append(string.Join(" | ", optionTerms.ToArray()));
			bldr.Append("   ");
		}
		return bldr.ToString();
	}
}
