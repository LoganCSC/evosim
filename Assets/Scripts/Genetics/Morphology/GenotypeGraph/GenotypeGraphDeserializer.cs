using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LitJson;
using System.IO;

/**
 * Read a graph from its json representation.
 */
public class GenotypeGraphDeserializer
{

	public GenotypeNode readFromJson(string json)
	{
		Debug.Log("json = " + json);
		JsonData root = JsonMapper.ToObject(json);
		JsonData contents = root["genotype_graph"];

		return readNode(contents);
	}

	public GenotypeNode readNode(JsonData contents)
	{
		GenotypeNode node = new GenotypeNode();

		node.dimensions = new Vector3(
			float.Parse(contents["dimensions"]["x"].ToString()),
			float.Parse(contents["dimensions"]["y"].ToString()),
			float.Parse(contents["dimensions"]["z"].ToString()));

		node.translateFromParent = new Vector3(
			float.Parse(contents["translateFromParent"]["x"].ToString()),
			float.Parse(contents["translateFromParent"]["y"].ToString()),
			float.Parse(contents["translateFromParent"]["z"].ToString()));

		node.rotateFromParent = new Vector3(
			float.Parse(contents["rotateFromParent"]["x"].ToString()),
			float.Parse(contents["rotateFromParent"]["y"].ToString()),
			float.Parse(contents["rotateFromParent"]["z"].ToString()));

		if (contents.Keys.Contains("children"))
		{
			int ct = contents["children"].Count;
			for (int i = 0; i < ct; i++)
			{
				node.children.Add(readNode(contents["children"][i]));
			}
		}
		return node;
	}
}
