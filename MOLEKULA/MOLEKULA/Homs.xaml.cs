using MOLEKULA.MyData;
using MOLEKULA.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace MOLEKULA
{
    /// <summary>
    /// Логика взаимодействия для Homs.xaml
    /// </summary>
    public partial class Homs : Window
    {
        Build build;
        Functionals f = Functionals.DB();
        public Homs()
        {
            InitializeComponent();
            Properties.Settings.Default["Connection"] = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\Molekules.mdf;Integrated Security=True;Connect Timeout=30";
            f.nameDB = Properties.Settings.Default.Connection;
            CONTENTS.Content = new HomeMain();
            build = new Build();
            f.GetSmaile();
        }
        private void MoveForm(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (build.myConst[9] == 0)
                    if (Mouse.GetPosition(Header).Y < 75)
                    {
                        DragMove();
                    }
            }
            catch (Exception a) { }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BorderHome.Visibility = Visibility.Visible;
            StartBuild.Visibility = Visibility.Hidden;
            CONTENTS.Content = build;
        }

        private void CONTENTS_ContentRendered(object sender, EventArgs e)
        {
            CONTENTS.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BorderHome.Visibility = Visibility.Hidden;
            StartBuild.Visibility = Visibility.Visible;
            CONTENTS.Content = new HomeMain();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            BorderHome.Visibility = Visibility.Hidden;
            StartBuild.Visibility = Visibility.Hidden;
            CONTENTS.Content = new Product();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            BorderHome.Visibility = Visibility.Visible;
            StartBuild.Visibility = Visibility.Hidden;
            CONTENTS.Content = build;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            BorderHome.Visibility = Visibility.Hidden;
            StartBuild.Visibility = Visibility.Hidden;
            CONTENTS.Content = new Help();
        }

        private void Open_Table(object sender, RoutedEventArgs e)
        {
            BorderHome.Visibility = Visibility.Hidden;
            StartBuild.Visibility = Visibility.Hidden;
            CONTENTS.Content = new Mendeleev();
        }

        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
