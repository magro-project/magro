using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace editor
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Label> lineLabels = new List<Label>();

        public MainWindow()
        {
            InitializeComponent();

            // add lines
            for (var i = 0; i < 20; i++)
            {
                var label = new Label()
                {
                    Name = "lineLabel" + i,
                    Foreground = Brushes.White,
                    FontSize = 14,
                    Padding = new Thickness { },
                    Content = "Line " + i,
                };
                label.MouseDoubleClick += Label_MouseDoubleClick;
                instList.Items.Add(label);
                lineLabels.Add(label);
            }
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
            if (sender is Label label)
            {
                var index = lineLabels.FindIndex(x => x == label);
                MessageBox.Show("MouseDoubleClick index=" + index);
            }
        }
    }
}
