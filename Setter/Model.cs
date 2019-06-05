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

namespace Setter
{
    public partial class Model : INotifyPropertyChanged
    {

        UdpClient _dataSocket;
        private static UInt32 _localPort = 9001;
        private static String _localIPAddress = "127.0.0.1";
        private static UInt32 _remotePort = 9000;
        private static String _remoteIPAddress =  "127.0.0.1";

        private Thread _receiveDataThread;

        private readonly UInt32 _numLEDs = 6;

        public Model()
        {
           

        }


        public bool InitModel()
        {
            try
            {
                _dataSocket = new UdpClient((int)_localPort);
                ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
                _receiveDataThread = new Thread(threadFunction);
                _receiveDataThread.Start();
            }
            catch(Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }

            return true;
            
        }

        public void Send24()
        {
            TimeData.StructTimeData timeData = new TimeData.StructTimeData();
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            Byte[] sendBytes;

            if (Hour == null && Minute == null && Second == null)
            {
                try
                {
                    timeData.is24HourTime = Is24Hour;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());

                }
            }

            else
            {
                try
                {
                    timeData.hour = int.Parse(Hour);// serialize the gameData structure to a stream 
                    timeData.minute = int.Parse(Minute);
                    timeData.second = int.Parse(Second);
                    timeData.is24HourTime = Is24Hour;
                    timeData.isAlarmTime = IsAlarm;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }

            formatter.Serialize(stream, timeData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);

            _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
        }


        public void showCurrent()
        {


            TimeData.StructTimeData timeData = new TimeData.StructTimeData();
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            Byte[] sendBytes;

            if (Is24Hour)
            {
                timeData.hour = int.Parse(DateTime.Now.ToString("HH:mm:ss")[0].ToString() + DateTime.Now.ToString("HH:mm:ss")[1].ToString());
                timeData.minute = int.Parse(DateTime.Now.ToString("HH:mm:ss")[3].ToString() + DateTime.Now.ToString("HH:mm:ss")[4].ToString());
                timeData.second = int.Parse(DateTime.Now.ToString("HH:mm:ss")[6].ToString() + DateTime.Now.ToString("HH:mm:ss")[7].ToString());
                timeData.is24HourTime = Is24Hour;
                timeData.isAlarmTime = IsAlarm;
            }
            else
            {
                timeData.hour = int.Parse(DateTime.Now.ToString("hh:mm:ss")[0].ToString() + DateTime.Now.ToString("hh:mm:ss")[1].ToString());
                timeData.minute = int.Parse(DateTime.Now.ToString("hh:mm:ss")[3].ToString() + DateTime.Now.ToString("hh:mm:ss")[4].ToString());
                timeData.second = int.Parse(DateTime.Now.ToString("hh:mm:ss")[6].ToString() + DateTime.Now.ToString("hh:mm:ss")[7].ToString());
                timeData.is24HourTime = Is24Hour;
                timeData.isAlarmTime = IsAlarm;
            }


            formatter.Serialize(stream, timeData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);

            _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
        }

        public void SetAlarm()
        {
            
            if (CheckSet() == true)
            {
                TimeData.StructTimeData timeData = new TimeData.StructTimeData();
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                Byte[] sendBytes;

                try
                {
                    timeData.hour = int.Parse(Hour);// serialize the gameData structure to a stream 
                    timeData.minute = int.Parse(Minute);
                    timeData.second = int.Parse(Second);
                    timeData.is24HourTime = Is24Hour;
                    timeData.isAlarmTime = IsAlarm;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }

                formatter.Serialize(stream, timeData);

                // retrieve a Byte array from the stream
                sendBytes = stream.ToArray();

                // send the serialized data
                IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);

                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);

                SetAlarmButtonVisibility = Visibility.Visible;

            }
            else
            {
                Debug.Write("bad Input");
            }

            
        }




        public void SetTime()
        {
            if (CheckSet() == true)
            {
                TimeData.StructTimeData timeData = new TimeData.StructTimeData();
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                Byte[] sendBytes;

                try
                {
                    if (Is24Hour)
                    {
                        timeData.hour = int.Parse(Hour);
                        timeData.minute = int.Parse(Minute);
                        timeData.second = int.Parse(Second);
                        timeData.is24HourTime = Is24Hour;
                        timeData.isAlarmTime = IsAlarm;
                    }
                    else
                    {
                        if (int.Parse(Hour) >= 12) timeData.hour = int.Parse(Hour) - 12;
                        else timeData.hour = int.Parse(Hour);
                        timeData.minute = int.Parse(Minute);
                        timeData.second = int.Parse(Second);
                        timeData.is24HourTime = Is24Hour;
                        timeData.isAlarmTime = IsAlarm;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }

                formatter.Serialize(stream, timeData);

                // retrieve a Byte array from the stream
                sendBytes = stream.ToArray();

                // send the serialized data
                IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);

                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            else
            {
                Debug.Write("bad Input");
            }

        }

        public bool CheckSet()
        {
            try
            {
                if (int.Parse(Hour) == 23 && int.Parse(Minute) == 59 && int.Parse(Second) == 59)
                {
                    return true;
                }

                else if (int.Parse(Hour) >= 60 || int.Parse(Minute) >= 60 || int.Parse(Second) >= 60)
                {

                    return false;
                }

                else if (int.Parse(Hour) <= 23 && int.Parse(Minute) <= 59 && int.Parse(Second) <= 59)
                {

                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return false;
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


                    TimeData.StructTimeData timeData = new TimeData.StructTimeData();
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // deserialize data back into our GameData structure
                    stream = new System.IO.MemoryStream(receiveData);
                    timeData = (TimeData.StructTimeData)formatter.Deserialize(stream);

                    // update view data through our bound properties
                    timeData.hour = int.Parse(Hour);// serialize the gameData structure to a stream 
                    timeData.minute = int.Parse(Minute);
                    timeData.second = int.Parse(Second);
                    timeData.is24HourTime = Is24Hour;
                    timeData.isAlarmTime = IsAlarm;



                    // update status window
                    Console.WriteLine(DateTime.Now + ":" + " New message received.\n");
                    

                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    Console.WriteLine(ex.ToString() + "------------1");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString() + "------------2");
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
