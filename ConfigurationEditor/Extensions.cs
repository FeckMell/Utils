using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ConfigurationEditor
{
  /// <summary>
  /// Provides some extension methods for LINQ
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// Adds ForEach extension method. May throw if action parameter throws.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns> Returns same collection which is passed to allow method chaining </returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      if (source == null) { return source; }
      if (action == null) { return source; }
      foreach (T e in source) { action(e); }
      return source;
    }

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