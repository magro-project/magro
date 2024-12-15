using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace editor
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("runtime.exe");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowVersion_Click(object sender, RoutedEventArgs e)
        {
            new VersionWindow(this).ShowDialog();
        }

        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("MouseDoubleClick");
        }
    }
}
