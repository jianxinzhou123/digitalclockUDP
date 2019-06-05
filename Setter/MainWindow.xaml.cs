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

/* Silin Chen
 * Jian Zhou
 * Group C
 * Copyright 2019
 * */

namespace Setter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Model _model;
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            _model = new Model();
   
            DataContext = _model;
            _model.InitModel();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _model.Is24Hour = true;
            _model.Send24();
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            _model.Is24Hour = false;
            _model.Send24();
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _model.IsAlarm = false;
            _model.showCurrent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _model.IsAlarm = false;
            _model.SetTime();
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _model.IsAlarm = true;
            _model.SetAlarm();
           
        }
    }
}
