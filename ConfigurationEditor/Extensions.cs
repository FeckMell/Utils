using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ConfigurationEditor
{
  /// <summary>
  /// Provides some extension methods
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// Determines is this node a leaf or not
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsLeaf(this XmlNode node)
    {
      if (node == null) { return true; }
      if (node.ChildNodes == null) { return true; }
      if (!node.HasChildNodes) { return true; }
      return node.ChildNodes.Count <= 1 && !node.ChildNodes[0].HasChildNodes;
    }
  }
}