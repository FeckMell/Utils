using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ConfigurationEditor
{
  /// <summary>
  /// Interaction logic for XMLFileNode.xaml
  /// </summary>
  public partial class XMLFileNode : UserControl
  {

    #region Fields

    /// <summary>
    /// Reference to XMLDocument node in <see cref="XMLFileModel"/>
    /// </summary>
    public XmlNode Node { get; set; }

    /// <summary>
    /// Name assessor 
    /// </summary>
    public string NodeName
    {
      get => (UIName?.Content ?? "") as string;
      set { if (UIName != null) { UIName.Content = value ?? ""; } }
    }

    /// <summary>
    /// Value assessor
    /// </summary>
    public string Value
    {
      get => UIValue?.Text ?? "";
      set { if (UIValue != null) { UIValue.Text = value ?? ""; } }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node"></param>
    public XMLFileNode(XmlNode node)
    {
      InitializeComponent();
      Node = node;
      NodeName = Node.Name;
      if (Node.IsLeaf())
      {
        UIValue.Visibility = Visibility.Visible;
        Value = Node.InnerText;
      }
      else
      {
        UIValue.Visibility = Visibility.Collapsed;
        foreach (XmlNode e in Node.ChildNodes)
        {
          if (e.NodeType == XmlNodeType.Comment) { continue; }
          UIChildren.Children.Add(new XMLFileNode(e));
        }
      }
      UIExpander.Visibility = (UIChildren.Children.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
      UIExpander.IsExpanded = true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Save method
    /// </summary>
    /// <returns></returns>
    public bool Save()
    {
      bool result = false;
      if (Node.IsLeaf()) { result = !Value.Equals(Node.InnerText); Node.InnerText = Value; }
      foreach (XMLFileNode e in UIChildren.Children)
      {
        result |= e.Save();
      }
      return result;
    }

    /// <summary>
    /// Filters instance by Name
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public bool FilterByName(string filter)
    {
      bool isAllHidden = true;
      if (NodeName.ToUpper().Contains(filter.ToUpper())) { isAllHidden = false; }
      foreach (XMLFileNode e in UIChildren.Children)
      {
        isAllHidden &= e.FilterByName(filter);
      }
      Visibility = isAllHidden ? Visibility.Collapsed : Visibility.Visible;
      return isAllHidden;
    }

    /// <summary>
    /// Filters instance by Value
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public bool FilterByValue(string filter)
    {
      bool isAllHidden = true;
      if (Value.ToUpper().Contains(filter.ToUpper())) { isAllHidden = false; }
      foreach (XMLFileNode e in UIChildren.Children)
      {
        isAllHidden &= e.FilterByValue(filter);
      }
      Visibility = isAllHidden ? Visibility.Collapsed : Visibility.Visible;
      return isAllHidden;
    }

    /// <summary>
    /// Cancels filter collapsing
    /// </summary>
    public void CancelFilter()
    {
      Visibility = Visibility.Visible;
      foreach (XMLFileNode e in UIChildren.Children) { e.CancelFilter(); }
    }

    #endregion

    #region Handlers

    /// <summary>
    /// Expander expanded
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UIExpander_Expanded(object sender, RoutedEventArgs e)
    {
      UIChildren.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Expander collapsed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UIExpander_Collapsed(object sender, RoutedEventArgs e)
    {
      UIChildren.Visibility = Visibility.Collapsed;
    } 

    #endregion
  }
}
