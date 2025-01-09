using Magro.Compiler;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Magro
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
            Task.Run(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    ExecButton.IsEnabled = false;
                }));

                var info = Directory.CreateDirectory(".magro");
                var dir = info.FullName;

                var utf8 = new UTF8Encoding(false);

                using (var writer = new StreamWriter(dir + "\\go.mod", false, utf8))
                {
                    var source = Properties.Resources.go_mod;
                    writer.Write(source);
                }

                using (var writer = new StreamWriter(dir + "\\build.bat", false, utf8))
                {
                    var source = Properties.Resources.build_bat;
                    writer.Write(source);
                }

                using (var writer = new StreamWriter(dir + "\\main.go", false, utf8))
                {
                    var source = Properties.Resources.main_go;
                    writer.Write(source);
                }

                using (var reader = new StreamReader(".\\script.ss", utf8))
                using (var writer = new StreamWriter(dir + "\\script.go", false, utf8))
                {
                    var compiler = new SyakeCompiler();
                    compiler.Compile(reader, writer);
                }

                var build = Process.Start(new ProcessStartInfo(dir + "\\build.bat")
                {
                    WorkingDirectory = dir,
                    WindowStyle = ProcessWindowStyle.Hidden,
                });
                build.WaitForExit();

                if (build.ExitCode != 0)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBox.Show("ビルドに失敗しました。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        ExecButton.IsEnabled = true;
                    }));
                    return;
                }

                Process.Start("game.exe");

                Dispatcher.Invoke(new Action(() =>
                {
                    ExecButton.IsEnabled = true;
                }));
            });
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
