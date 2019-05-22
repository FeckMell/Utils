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
  /// Interaction logic for XMLFile.xaml
  /// </summary>
  public partial class XMLFile : UserControl
  {

    #region Fields

    /// <summary>
    /// Document
    /// </summary>
    public XmlDocument Document { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// XAML constructor
    /// </summary>
    /// <param name="e"></param>
    public XMLFile(XmlDocument document)
    {
      InitializeComponent();
      Initialise(document);
      UIExpander.IsExpanded = true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Filters values by name
    /// </summary>
    /// <param name="filter"></param>
    public void FilterByName(string filter)
    {
      bool isAllHidden = true;
      foreach (XMLFileNode e in UIChildren.Children)
      {
        isAllHidden &= e.FilterByName(filter);
      }
      Visibility = isAllHidden ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// Filters values by value
    /// </summary>
    /// <param name="filter"></param>
    public void FilterByValue(string filter)
    {
      bool isAllHidden = true;
      foreach (XMLFileNode e in UIChildren.Children)
      {
        isAllHidden &= e.FilterByValue(filter);
      }
      Visibility = isAllHidden ? Visibility.Collapsed : Visibility.Visible;
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

    #region Private methods

    /// <summary>
    /// Initialises instance
    /// </summary>
    /// <param name="document"></param>
    private void Initialise(XmlDocument document)
    {
      Document = document;
      UIFile.Content = System.IO.Path.GetFileName(Document.BaseURI);
      UIChildren.Children.Clear();
      foreach (XmlNode e in Document.DocumentElement.ChildNodes)
      {
        if (e.NodeType == XmlNodeType.Comment) { continue; }
        UIChildren.Children.Add(new XMLFileNode(e));
      }
    } 

    #endregion

    #region Handlers

    /// <summary>
    /// Handler for Save button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Save_Handler(object sender, RoutedEventArgs e)
    {
      try
      {
        bool isChanged = false;
        foreach (XMLFileNode x in UIChildren.Children)
        {
          isChanged = isChanged || x.Save();
        }
        if (isChanged)
        {
          Document.Save(Document.BaseURI.Remove(0, 8));
          Reload_Handler(null, null);
        }
      }
      catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    /// <summary>
    /// Handler for Open button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Open_Handler(object sender, RoutedEventArgs e)
    {
      try { System.Diagnostics.Process.Start(Document.BaseURI); }
      catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    /// <summary>
    /// Handler for Reload button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reload_Handler(object sender, RoutedEventArgs e)
    {
      try
      {
        string filepath = Document.BaseURI;
        XmlDocument doc = new XmlDocument();
        doc.Load(Document.BaseURI);

        Initialise(doc);
      }
      catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    /// <summary>
    /// Expander expanded handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UIExpander_Expanded(object sender, RoutedEventArgs e)
    {
      UIChildren.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Expander collapsed handler
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
