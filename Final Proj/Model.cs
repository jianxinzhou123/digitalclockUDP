using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

// Sockets
using System.Net.Sockets;
using System.Net;

// debug
using System.Diagnostics;

// threading
using System.Threading;

using System.Windows.Threading;

// byte data serialization
using System.Runtime.Serialization.Formatters.Binary;

// memory streams
using System.IO;
using TimeDataDLL;

/* Silin Chen
 * Jian Zhou
 * Group C
 * Copyright 2019
 * */

namespace Final_Proj
{
    public partial class Model : INotifyPropertyChanged
    {
        private int second1;
        private int second2;
        private int minute1;
        private int minute2;
        private int hour1;
        private int hour2;

        private int alarm_hour = 0;
        private int alarm_minute = 0;
        private int alarm_second = 0;

        private int real_hour = 0;
        private int real_minute = 0;
        private int real_second = 0;

        private bool isAlarmTime = false;
        private bool is24HourTime = false;


        private string _AMorPM;
        public string AMorPM
        {
            get { return _AMorPM; }
            set
            {
                _AMorPM = value;
                OnPropertyChanged("AMorPM");
            }
        }

        private Visibility _amvisibility = Visibility.Hidden;
        public Visibility AMVisibility
        {
            get { return _amvisibility; }
            set
            {
                _amvisibility = value;
                OnPropertyChanged("AMVisibility");
            }
        }

        private Visibility _alarm = Visibility.Hidden;
        public Visibility Alarm
        {
            get { return _alarm; }
            set
            {
                _alarm = value;
                OnPropertyChanged("Alarm");
            }
        }

        private Visibility _pmvisibility = Visibility.Hidden;
        public Visibility PMVisibility
        {
            get { return _pmvisibility; }
            set
            {
                _pmvisibility = value;
                OnPropertyChanged("PMVisibility");
            }
        }

        private bool _isPM = (DateTime.Now.Hour > 12);
        public bool ISPM
        {
            get { return _isPM; }
            set
            {
                _isPM = value;
                OnPropertyChanged("ISPM");
            }
        }




        [Serializable]
        public struct StructTimeData
        {
            public int hour, minute, second;
            public bool isAlarmTime;
            public bool is24HourTime;

            public StructTimeData(int h = 12, int m = 0, int s = 0, bool a = false, bool t = true)
            {
                hour = h;
                minute = m;
                second = s;
                isAlarmTime = a;
                is24HourTime = t;
            }
        }


        UdpClient _dataSocket;
        private static UInt32 _localPort = 9000;
        private static string _localIPAddress = "127.0.0.1";



        private Thread _receiveDataThread;
        public ObservableCollection<LED> LEDCollection;

        private readonly UInt32 _numLEDs = 6;

        public Model()
        {
            LEDCollection = new ObservableCollection<LED>();
            for (int i = 0; i < _numLEDs; i++)
            {
                LEDCollection.Add(new LED() { });
            }

        }


        public bool InitModel()
        {
            try
            {
                _dataSocket = new UdpClient((int)_localPort);
                ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
                _receiveDataThread = new Thread(threadFunction);
                _receiveDataThread.Start();

                if (ISPM)
                {
                    AMVisibility = Visibility.Hidden;
                    PMVisibility = Visibility.Visible;

                }
                else
                {
                    PMVisibility = Visibility.Hidden;
                    AMVisibility = Visibility.Visible;
                }

            }
            catch(Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }

            return true;
            

        }

        DispatcherTimer timer;

        public void startTimer()
        {
            int timerCount = 0;

            if (is24HourTime)
            {
                second1 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[6].ToString());
                second2 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[7].ToString());
                minute1 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[3].ToString());
                minute2 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[4].ToString());
                hour1 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[0].ToString());
                hour2 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[1].ToString());
            }

            else
            {
                second1 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[6].ToString());
                second2 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[7].ToString());
                minute1 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[3].ToString());
                minute2 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[4].ToString());
                hour1 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[0].ToString());
                hour2 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[1].ToString());
            }

            timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Tick += timer_Tick;
                timer.Start();

                void timer_Tick(object sender, EventArgs e)
                {
                    timerCount++;

                    if (ISPM)
                    {
                        AMVisibility = Visibility.Hidden;
                        PMVisibility = Visibility.Visible;

                    }
                    else
                    {
                        PMVisibility = Visibility.Hidden;
                        AMVisibility = Visibility.Visible;
                    }

                    if (is24HourTime)
                    {
                        PMVisibility = Visibility.Hidden;
                        AMVisibility = Visibility.Hidden;
                        if (second2 != 9) second2++;
                        else
                        {
                            second2 = 0;
                            if (second1 != 5) second1++;
                            else
                            {
                                second1 = 0;
                                if (minute2 != 9) minute2++;
                                else
                                {
                                    minute2 = 0;
                                    if (minute1 != 5) minute1++;
                                    else
                                    {
                                        minute1 = 0;
                                        if (hour2 != 9 && hour1 != 2) hour2++;
                                        else if (hour1 != 2 && hour2 == 9)
                                        {
                                            hour1++; hour2 = 0;
                                        }
                                        else if (hour2 != 3 && hour1 == 2) hour2++;
                                        else if (hour1 == 2 && hour2 == 3)
                                        {
                                            hour1 = 0; hour2 = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (second2 != 9) second2++;
                        else
                        {
                            second2 = 0;
                            if (second1 != 5) second1++;
                            else
                            {
                                second1 = 0;
                                if (minute2 != 9) minute2++;
                                else
                                {
                                    minute2 = 0;
                                    if (minute1 != 5) minute1++;
                                    else
                                    {
                                        minute1 = 0;
                                        if (hour2 != 9 && hour1 != 1) hour2++;
                                        else if (hour1 != 1 && hour2 == 9)
                                        {
                                            hour1++; hour2 = 0;
                                        }
                                        else if (hour2 != 1 && hour1 == 1) hour2++;
                                        else if (hour1 == 1 && hour2 == 1)
                                        {
                                            hour1 = 0; hour2 = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }

                translate(hour1, hour2, minute1, minute2, second1, second2);

                if (alarm_hour / 10 == hour1 && alarm_hour % 10 == hour2 && alarm_minute / 10 == minute1 && alarm_minute % 10 == minute2 && alarm_second / 10 == second1 && alarm_second % 10 == second2)
                    Alarm = Visibility.Visible;
                if(timerCount == 12)
                {
                    Alarm = Visibility.Hidden;
                    timerCount = 0;
                }


            }
        }

        private void translate(int hr1, int hr2, int min1, int min2, int sec1, int sec2)
        {
            switch (sec2)
            {
                case 0:
                    LEDCollection[5].LEDValue = 0; break;
                case 1:
                    LEDCollection[5].LEDValue = 1; break;
                case 2:
                    LEDCollection[5].LEDValue = 2; break;
                case 3:
                    LEDCollection[5].LEDValue = 3; break;
                case 4:
                    LEDCollection[5].LEDValue = 4; break;
                case 5:
                    LEDCollection[5].LEDValue = 5; break;
                case 6:
                    LEDCollection[5].LEDValue = 6; break;
                case 7:
                    LEDCollection[5].LEDValue = 7; break;
                case 8:
                    LEDCollection[5].LEDValue = 8; break;
                case 9:
                    LEDCollection[5].LEDValue = 9; break;
            }

            switch (sec1)
            {
                case 0:
                    LEDCollection[4].LEDValue = 0; break;
                case 1:
                    LEDCollection[4].LEDValue = 1; break;
                case 2:
                    LEDCollection[4].LEDValue = 2; break;
                case 3:
                    LEDCollection[4].LEDValue = 3; break;
                case 4:
                    LEDCollection[4].LEDValue = 4; break;
                case 5:
                    LEDCollection[4].LEDValue = 5; break;
            }

            switch (min2)
            {
                case 0:
                    LEDCollection[3].LEDValue = 0; break;
                case 1:
                    LEDCollection[3].LEDValue = 1; break;
                case 2:
                    LEDCollection[3].LEDValue = 2; break;
                case 3:
                    LEDCollection[3].LEDValue = 3; break;
                case 4:
                    LEDCollection[3].LEDValue = 4; break;
                case 5:
                    LEDCollection[3].LEDValue = 5; break;
                case 6:
                    LEDCollection[3].LEDValue = 6; break;
                case 7:
                    LEDCollection[3].LEDValue = 7; break;
                case 8:
                    LEDCollection[3].LEDValue = 8; break;
                case 9:
                    LEDCollection[3].LEDValue = 9; break;
            }

            switch (min1)
            {
                case 0:
                    LEDCollection[2].LEDValue = 0; break;
                case 1:
                    LEDCollection[2].LEDValue = 1; break;
                case 2:
                    LEDCollection[2].LEDValue = 2; break;
                case 3:
                    LEDCollection[2].LEDValue = 3; break;
                case 4:
                    LEDCollection[2].LEDValue = 4; break;
                case 5:
                    LEDCollection[2].LEDValue = 5; break;
            }

            switch (hr2)
            {
                case 0:
                    LEDCollection[1].LEDValue = 0; break;
                case 1:
                    LEDCollection[1].LEDValue = 1; break;
                case 2:
                    LEDCollection[1].LEDValue = 2; break;
                case 3:
                    LEDCollection[1].LEDValue = 3; break;
                case 4:
                    LEDCollection[1].LEDValue = 4; break;
                case 5:
                    LEDCollection[1].LEDValue = 5; break;
                case 6:
                    LEDCollection[1].LEDValue = 6; break;
                case 7:
                    LEDCollection[1].LEDValue = 7; break;
                case 8:
                    LEDCollection[1].LEDValue = 8; break;
                case 9:
                    LEDCollection[1].LEDValue = 9; break;
            }

            switch (hr1)
            {
                case 0:
                    LEDCollection[0].LEDValue = 0; break;
                case 1:
                    LEDCollection[0].LEDValue = 1; break;
                case 2:
                    LEDCollection[0].LEDValue = 2; break;
            }
        }

        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    // wait for data
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // check to see if this is synchronization data 
                    // ignore it. we should not recieve any sychronization
                    // data here, because synchronization data should have 
                    // been consumed by the SynchWithOtherPlayer thread. but, 
                    // it is possible to get 1 last synchronization byte, which we
                    // want to ignore
                    if (receiveData.Length < 2)
                        continue;


                    // process and display data


                    TimeData.StructTimeData timeData;
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // deserialize data back into our GameData structure
                    stream = new System.IO.MemoryStream(receiveData);
                    timeData = (TimeData.StructTimeData)formatter.Deserialize(stream);

                    // update view data through our bound properties
                    if (timeData.hour == 0 && timeData.minute == 0 && timeData.second == 0)
                    {
                        is24HourTime = timeData.is24HourTime;
                        if (is24HourTime)
                        {
                            AMVisibility = Visibility.Hidden;
                            PMVisibility = Visibility.Hidden;
                            hour1 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[0].ToString());
                            hour2 = int.Parse(DateTime.Now.ToString("HH:mm:ss")[1].ToString());
                        }
                        else
                        {
                            
                           
                            hour1 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[0].ToString());
                            hour2 = int.Parse(DateTime.Now.ToString("hh:mm:ss")[1].ToString());

                            if (ISPM)
                            {
                                AMVisibility = Visibility.Hidden;
                                PMVisibility = Visibility.Visible;

                            }
                            else
                            {
                                PMVisibility = Visibility.Hidden;
                                AMVisibility = Visibility.Visible;
                            }
                        }
                    }
                    else
                    {

                        if (!timeData.isAlarmTime) {
                            real_hour = timeData.hour;
                            real_minute = timeData.minute;
                            real_second = timeData.second;
                            isAlarmTime = timeData.isAlarmTime;
                            is24HourTime = timeData.is24HourTime;

                            if (!is24HourTime && real_hour >= 12)
                            {
                                real_hour -= 12;
                            }

                            second1 = real_second / 10;
                            second2 = real_second % 10;
                            minute1 = real_minute / 10;
                            minute2 = real_minute % 10;
                            hour1 = real_hour / 10;
                            hour2 = real_hour % 10;
                        }

                        else
                        {
                            alarm_hour = timeData.hour;
                            alarm_minute = timeData.minute;
                            alarm_second = timeData.second;
                        }
                       
                    }
                 

                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    Console.WriteLine(ex.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
