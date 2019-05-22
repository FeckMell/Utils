using System.Xml;
using System.Windows.Forms;
using System;
using System.Windows;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ConfigurationEditor
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
    } 

    #endregion

    #region Private methods

    /// <summary>
    /// Processes folder
    /// </summary>
    /// <param name="folder"></param>
    private void ProcessFolder(string folder)
    {
      var files = Directory.GetFiles(folder).Where(x => Path.GetExtension(x).Equals(".xml", StringComparison.OrdinalIgnoreCase)).ToList();
      var documents = new List<XmlDocument>();
      foreach (var e in files)
      {
        try
        {
          var xml = new XmlDocument();
          xml.Load(e);
          documents.Add(xml);
        }
        catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"File: {Path.GetFileName(e)}.\nException:{ex.Message}"); }
      }
      ProcessDocuments(documents);
    }

    /// <summary>
    /// Processing documents
    /// </summary>
    /// <param name="xmls"></param>
    private void ProcessDocuments(List<XmlDocument> documents)
    {
      foreach (var e in documents)
      {
        try { UIFiles.Children.Add(new XMLFile(e)); }
        catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"{ex.Message}"); }
      }
    } 

    #endregion

    /// <summary>
    /// Opens folder
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenFolder_Handler(object sender, RoutedEventArgs e)
    {
      try
      {
        UIFiles.Children.Clear();
        using (OpenFileDialog dialog = new OpenFileDialog() { ValidateNames = false, CheckFileExists = false, CheckPathExists = true, FileName = "Folder Selection." })
        {
          if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
          {
            FolderPath.Text = Path.GetDirectoryName(dialog.FileName);
          }
        }
        ProcessFolder(FolderPath.Text);
      }
      catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
    }

    /// <summary>
    /// Filters nodes by name
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchFile_Handler(object sender, RoutedEventArgs e)
    {
      foreach (XMLFile x in UIFiles.Children)
      {
        x.CancelFilter();
        x.Visibility = (x.UIFile.Content as string).ToUpper().Contains(UIFilter.Text.ToUpper()) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    /// <summary>
    /// Filters nodes by name
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchByName_Handler(object sender, RoutedEventArgs e)
    {
      foreach (XMLFile x in UIFiles.Children)
      {
        x.CancelFilter();
        x.FilterByName(UIFilter.Text);
      }
    }

    /// <summary>
    /// Filters nodes by value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchByValue_Handler(object sender, RoutedEventArgs e)
    {
      foreach (XMLFile x in UIFiles.Children)
      {
        x.CancelFilter();
        x.FilterByValue(UIFilter.Text);
      }
    }

    /// <summary>
    /// Collapses all files
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CollapseAll_Handler(object sender, RoutedEventArgs e)
    {
      foreach(XMLFile x in UIFiles.Children)
      {
        x.UIExpander.IsExpanded = false;
      }
    }

    /// <summary>
    /// Expands all files
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExpandAll_Handler(object sender, RoutedEventArgs e)
    {
      foreach (XMLFile x in UIFiles.Children)
      {
        x.UIExpander.IsExpanded = true;
      }
    }
  }
}
