using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Serialize to json
 */
public class GenotypeGraphSerializer
{
	private static string INDENT = "   ";

	public string writeAsJsonProperty(string propertyName, GenotypeNode root, string indent)
	{
		string graph_json = indent + "\"" + propertyName  + "\" : ";

		graph_json += writeAsJsonAux(root, INDENT + indent);
		return graph_json;
	}

	/**
	 * write the current node and all its children
	 */
	private string writeAsJsonAux(GenotypeNode root, string indent)
	{
		string node_json = @"" + indent + "{";  // open node

		string node_string = @"
{10}""type"" : {0},
{10}""dimensions"" : {{ ""x"": {1}, ""y"": {2}, ""z"": {3} }},
{10}""rotateFromParent"" : {{ ""x"": {4}, ""y"": {5}, ""z"": {6} }},
{10}""translateFromParent"" : {{ ""x"": {7}, ""y"": {8}, ""z"": {9} }}";

		string[] l_args = {
			'"' + root.type + '"',
			root.dimensions.x.ToString(), root.dimensions.y.ToString(), root.dimensions.z.ToString(),
			root.rotateFromParent.x.ToString(), root.rotateFromParent.y.ToString(), root.rotateFromParent.z.ToString(),
			root.translateFromParent.x.ToString(), root.translateFromParent.y.ToString(), root.translateFromParent.z.ToString(), indent + INDENT
		};
		node_json += String.Format(node_string, l_args);

		Boolean hasChildren = root.children.Count > 0;
		GenotypeNode lastChild = null;
		if (hasChildren)
		{
			node_json +=
		"," + Environment.NewLine + indent + INDENT + @"""children"": [" + indent + Environment.NewLine;
			lastChild = root.children[root.children.Count - 1];
		}
		// print the node attributes
		// print the child nodes
		// print "children" : ....
		foreach (GenotypeNode child in root.children)
		{
			node_json += writeAsJsonAux(child, indent + INDENT + INDENT);
			if (child != lastChild)
			{
				node_json += "," + Environment.NewLine;
			}
		}
		if (hasChildren)
		{
			node_json +=
			indent + @"" + Environment.NewLine + indent + INDENT + "]"; // close child list
		}
		node_json +=
		indent + @"" + Environment.NewLine + indent + "}"; // close node
		return node_json;
	}

}
