using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
                InstListBox.Items.Add(label);
            }
        }

        // メニューバー項目

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("開くボタンが押されました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("保存ボタンが押されました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("オプションボタンが押されました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowVersion_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VersionWindow(this);
            dialog.ShowDialog();
        }

        // ツールバー項目

        private void ExecButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("runtime.exe"))
            {
                MessageBox.Show("実行に失敗しました。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Process.Start("runtime.exe");
        }

        // その他

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var index = InstListBox.Items.IndexOf(sender);
            if (index == -1) return;

            MessageBox.Show("MouseDoubleClick index=" + index, "情報", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
