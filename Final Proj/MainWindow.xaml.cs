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

namespace Final_Proj
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
            SevenSegmentLED.ItemsSource = _model.LEDCollection;
            _model.startTimer();
        }
    }
}
