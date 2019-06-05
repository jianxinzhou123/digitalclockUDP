using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Shapes;

/* Silin Chen
 * Jian Zhou
 * Group C
 * Copyright 2019
 * */

namespace Final_Proj
{
    public class LED : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private double _LEDCanvasLeft;
        public double LEDCanvasLeft
        {
            get { return _LEDCanvasLeft; }
            set
            {
                _LEDCanvasLeft = value;
                OnPropertyChanged("TileCanvasLeft");
            }
        }

        private System.Windows.Visibility _topHorizontal_Visible;
        public System.Windows.Visibility TopHorizontal_Visible
        {
            get { return _topHorizontal_Visible; }
            set
            {
                _topHorizontal_Visible = value;
                OnPropertyChanged("TopHorizontal_Visible");
            }
        }

        private System.Windows.Visibility _middleHorizontal_Visible;
        public System.Windows.Visibility MiddleHorizontal_Visible
        {
            get { return _middleHorizontal_Visible; }
            set
            {
                _middleHorizontal_Visible = value;
                OnPropertyChanged("MiddleHorizontal_Visible");
            }
        }

        private System.Windows.Visibility _bottomHorizontal_Visible;
        public System.Windows.Visibility BottomHorizontal_Visible
        {
            get { return _bottomHorizontal_Visible; }
            set
            {
                _bottomHorizontal_Visible = value;
                OnPropertyChanged("BottomHorizontal_Visible");
            }
        }

        private System.Windows.Visibility _topLeftVertical_Visible;
        public System.Windows.Visibility TopLeftVertical_Visible
        {
            get { return _topLeftVertical_Visible; }
            set
            {
                _topLeftVertical_Visible = value;
                OnPropertyChanged("TopLeftVertical_Visible");
            }
        }

        private System.Windows.Visibility _bottomLeftVertical_Visible;
        public System.Windows.Visibility BottomLeftVertical_Visible
        {
            get { return _bottomLeftVertical_Visible; }
            set
            {
                _bottomLeftVertical_Visible = value;
                OnPropertyChanged("BottomLeftVertical_Visible");
            }
        }

        private System.Windows.Visibility _topRightVertical_Visible;
        public System.Windows.Visibility TopRightVertical_Visible
        {
            get { return _topRightVertical_Visible; }
            set
            {
                _topRightVertical_Visible = value;
                OnPropertyChanged("TopRightVertical_Visible");
            }
        }

        private System.Windows.Visibility _bottomRightVertical_Visible;
        public System.Windows.Visibility BottomRightVertical_Visible
        {
            get { return _bottomRightVertical_Visible; }
            set
            {
                _bottomRightVertical_Visible = value;
                OnPropertyChanged("BottomRightVertical_Visible");
            }
        }

        private double _ledTop;
        public double LEDTop
        {
            get { return _ledTop; }
            set
            {
                _ledTop = value;
                OnPropertyChanged("LEDTop");
            }
        }

        private double _ledLeft;
        public double LEDLeft
        {
            get { return _ledLeft; }
            set
            {
                _ledLeft = value;
                OnPropertyChanged("LEDLeft");
            }
        }


        private UInt32 _ledValue;
        public UInt32 LEDValue
        {
            get { return _ledValue; }
            set
            {
                makeAllVisible();
                switch (value)
                {

                    case 0:
                        MiddleHorizontal_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 1:
                        TopHorizontal_Visible = System.Windows.Visibility.Hidden;
                        MiddleHorizontal_Visible = System.Windows.Visibility.Hidden;
                        BottomHorizontal_Visible = System.Windows.Visibility.Hidden;
                        TopLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        BottomLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 2:
                        TopLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        BottomRightVertical_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 3:
                        TopLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        BottomLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 4:
                        TopHorizontal_Visible = System.Windows.Visibility.Hidden;
                        BottomLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        BottomHorizontal_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 5:
                        TopRightVertical_Visible = System.Windows.Visibility.Hidden;
                        BottomLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 6:
                        TopRightVertical_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 7:
                        TopLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        BottomLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        MiddleHorizontal_Visible = System.Windows.Visibility.Hidden;
                        BottomHorizontal_Visible = System.Windows.Visibility.Hidden;
                        break;
                    case 8:
                        break;
                    case 9:
                        BottomLeftVertical_Visible = System.Windows.Visibility.Hidden;
                        break;
                }

                _ledValue = value;
            }
        }

        private void makeAllVisible()
        {
            TopHorizontal_Visible = System.Windows.Visibility.Visible;
            MiddleHorizontal_Visible = System.Windows.Visibility.Visible;
            BottomHorizontal_Visible = System.Windows.Visibility.Visible;
            TopLeftVertical_Visible = System.Windows.Visibility.Visible;
            TopRightVertical_Visible = System.Windows.Visibility.Visible;
            BottomLeftVertical_Visible = System.Windows.Visibility.Visible;
            BottomRightVertical_Visible = System.Windows.Visibility.Visible;
        }
    }
}
