using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/* Silin Chen
 * Jian Zhou
 * Group C
 * Copyright 2019
 * */

namespace Setter
{
    public partial class Model
    {

        private string _hour;
        public string Hour
        {
            get { return _hour; }
            set
            {
                _hour = value;
                OnPropertyChanged("Hour");
            }
        }

        private string _minute;
        public string Minute
        {
            get { return _minute; }
            set
            {
                _minute = value;
                OnPropertyChanged("Minute");
            }
        }

        private string _second;
        public string Second
        {
            get { return _second; }
            set
            {
                _second = value;
                OnPropertyChanged("Second");
            }
        }


        private bool _is24hour = false;
        public bool Is24Hour
        {
            get { return _is24hour; }
            set
            {
                _is24hour = value;
                OnPropertyChanged("Is24Hour");
            }
        }

        private bool _isAlarm = false;
        public bool IsAlarm
        {
            get { return _isAlarm; }
            set
            {
                _isAlarm = value;
                OnPropertyChanged("IsAlarm");
            }
        }

        private Visibility _setAlarmButtonVisibility = Visibility.Hidden;
        public Visibility SetAlarmButtonVisibility
        {
            get { return _setAlarmButtonVisibility; }
            set
            {
                _setAlarmButtonVisibility = value;
                OnPropertyChanged("SetAlarmButtonVisibility");
            }
        }


    }

}
