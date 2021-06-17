﻿using System;
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
using System.Windows.Threading;

namespace Game_of_Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            Random würfel = new Random();

            drawing_area.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            drawing_area.Arrange(new Rect(0.0, 0.0, drawing_area.DesiredSize.Width, drawing_area.DesiredSize.Height));


            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = drawing_area.ActualWidth / anzahlZellenBreit - 2.0;
                    r.Height = drawing_area.ActualHeight / anzahlZellenHoch - 2.0;
                    r.Fill = (würfel.Next(0, 2) == 1) ? Brushes.Black : Brushes.Red;
                    drawing_area.Children.Add(r);
                    Canvas.SetLeft(r, j * drawing_area.ActualWidth / anzahlZellenBreit);
                    Canvas.SetTop(r, i * drawing_area.ActualHeight / anzahlZellenHoch);
                    r.MouseDown += R_MouseDown;

                    felder[i, j] = r;
                }
            }

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            

        }

            const int anzahlZellenBreit = 30;
            const int anzahlZellenHoch = 30;
            Rectangle[,] felder = new Rectangle[anzahlZellenBreit, anzahlZellenHoch];
            DispatcherTimer timer = new DispatcherTimer();

        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.Black) ? Brushes.Red : Brushes.Black;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int[,] anzahlNachbarn = new int[anzahlZellenHoch, anzahlZellenBreit];

            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    int nachbarn = 0;
                    int iDarüber = i - 1;

                    if (iDarüber < 0)
                    { iDarüber = anzahlZellenHoch - 1; }

                    int iDarunter = i + 1;
                    if (iDarunter >= anzahlZellenHoch)
                    { iDarunter = 0; }

                    int jLinks = j - 1;
                    if (jLinks < 0)
                    { jLinks = anzahlZellenBreit - 1; }

                    int jRechts = j + 1;
                    if (jRechts >= anzahlZellenBreit)
                    { jRechts = 0; }

                    if (felder[iDarüber, jLinks].Fill == Brushes.Red)
                    { nachbarn++; }
                    if (felder[iDarüber, j].Fill == Brushes.Red)
                    { nachbarn++; }
                    if (felder[iDarüber, jRechts].Fill == Brushes.Red)
                    { nachbarn++; }

                    if (felder[i, jLinks].Fill == Brushes.Red)
                    { nachbarn++; }
                    if (felder[i, jRechts].Fill == Brushes.Red)
                    { nachbarn++; }

                    if (felder[iDarunter, jLinks].Fill == Brushes.Red)
                    { nachbarn++; }
                    if (felder[iDarunter, j].Fill == Brushes.Red)
                    { nachbarn++; }
                    if (felder[iDarunter, jRechts].Fill == Brushes.Red)
                    { nachbarn++; }

                    anzahlNachbarn[i, j] = nachbarn;
                }

            }

            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    if (anzahlNachbarn[i, j] < 2 || anzahlNachbarn[i, j] > 3)
                    {
                        felder[i, j].Fill = Brushes.Black;
                    }
                    else if (anzahlNachbarn[i, j] == 3)
                    {
                        felder[i, j].Fill = Brushes.Red;
                    }
                }
            }
        }


        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                buttonStartStop.Content = "Starte Animation!" ;
            }
            else 
            {
                timer.Start();
                buttonStartStop.Content = "Stoppe Animation!";
            }
        }
    }
}
