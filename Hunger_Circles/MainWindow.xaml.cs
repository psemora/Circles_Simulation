using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace HungerCircle
{
    public partial class MainWindow : Window
    {
        Dictionary<HungerCircle, Ellipse> circles = new Dictionary<HungerCircle, Ellipse>();
        Random r = new Random();
        DispatcherTimer t = new DispatcherTimer();
        DispatcherTimer t2 = new DispatcherTimer();
        private int count1 = 50, count2 = 50, count3 = 50;
        private int i = 0;
        public bool flagStopContinue = true;
        public bool flag_reset = true;
        private int intervalseconds =1;
        private int intervalmilliseconds = 5;
        
        public MainWindow()
        {
            InitializeComponent();
            btn_stop.IsEnabled = false;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Step();
            lblcount1.Content = count1.ToString();
            lblcount2.Content = count2.ToString();
            lblcount3.Content = count3.ToString();
        }

        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            i++;
            lblTime.Content = i.ToString();
        }


        private void Start()
        {
            btn_stop.IsEnabled = true;
            count1 = 50;
            count2 = 50;
            count3 = 50;
            for (int i = 0; i < 50; i++)
            {
                AddCircle_red();
                AddCircle_green();
                AddCircle_blue();
            }
            
           
            if (intervalmilliseconds == 5)
            {
                t.Tick += new EventHandler(dispatcherTimer_Tick);
                t.Interval = new TimeSpan(0, 0, 0, 0, 5);
                intervalmilliseconds = 6;
                t.Start();
            }
            else
            {
                t.Start();
            }

            if (intervalseconds == 1)
                {
                t2.Interval = new TimeSpan(0, 0, 0, 1);
                t2.Tick += new EventHandler(dispatcherTimer2_Tick);
                intervalseconds = 2;
                t2.Start();
            }
            else
            {
                t2.Start();
            }    
        }

        private void Step()
        {
            Remove();
            
            foreach (KeyValuePair<HungerCircle, Ellipse> tup in circles)
            {
                Point p = tup.Key.Step(screen.Width, screen.Height, r);
                tup.Value.Width = tup.Key.Size * 2;
                tup.Value.Height = tup.Key.Size * 2;
                Canvas.SetTop(tup.Value, p.Y - tup.Value.Height / 2);
                Canvas.SetLeft(tup.Value, p.X - tup.Value.Width / 2);
            }
            
            Eating();
            
            if (count1 == 1 | count2 == 1 | count3 == 1)
            {
                t2.Stop();
                MessageBox.Show("Celkový čas je: " + i.ToString() + " sekund");
                MessageBox.Show("Zbylo: " + count1.ToString() + " červených, " + count2.ToString() + " zelených a " + count3.ToString() + " modrých kuliček");
                this.Close();
            }
        }
        
        private bool Intersect(HungerCircle c)
        {
            foreach (HungerCircle checkCircle in circles.Keys)
            {
                if (c.Colide(checkCircle) != 0)
                {
                    return true;
                }
            }
            return false;
        }
        
        private void Remove()
        {
            List<HungerCircle> delete = new List<HungerCircle>(circles.Keys);
            delete = delete.FindAll(x => (x.Size == 0));
            foreach (HungerCircle c in delete)
            {
                screen.Children.Remove(circles[c]);
                circles.Remove(c);
            }
        }

        private void Eating()
        {      
           List<HungerCircle> searchList = new List<HungerCircle>(circles.Keys);
           List<HungerCircle> newList = new List<HungerCircle>();
            while (searchList.Count > 0)
            {
                HungerCircle c = searchList[0];
                 newList.Add(c);
                 searchList.Remove(c);

                foreach (HungerCircle circle in searchList)
                {

                    int colideResult = c.Colide(circle);

                    if (colideResult == 1)
                    {
                        c.Eat(circle);
                        circle.IsEaten();

                        if (circle.Color == Color.FromRgb((255), (0), (0)))
                        {
                            count1--;
                        }
                        if (circle.Color == Color.FromRgb((0), (255), (0)))
                        {
                            count2--;
                        }
                        if (circle.Color == Color.FromRgb((0), (0), (255)))
                        {
                            count3--;
                        }
                    }

                    else if (colideResult == -1)
                    {
                        circle.Eat(c);
                        c.IsEaten();

                        if (circle.Color == Color.FromRgb((255), (0), (0)))
                        {
                            count1--;
                        }
                        if (circle.Color == Color.FromRgb((0), (255), (0)))
                        {
                            count2--;
                        }
                        if (circle.Color == Color.FromRgb((0), (0), (255)))
                        {
                            count3--;
                        }
                    }
                }
            }
        }

        private void AddCircle_red()
        {
            HungerCircle c;
            Ellipse el;
            do
            {
                c = new HungerCircle(new Point((r.Next((int)screen.Width)), (r.Next((int)screen.Height))), Math.Round(r.NextDouble() * 25 + 5), 2, Color.FromRgb(255, 0, 0));
                el = new Ellipse();
                el.Width = c.Size * 2;
                el.Height = c.Size * 2;
                el.Fill = new SolidColorBrush(c.Color);
            } while (Intersect(c));
            screen.Children.Add(el);
            Canvas.SetTop(el, c.Position.Y - el.Height / 2);
            Canvas.SetLeft(el, c.Position.X - el.Width / 2);
            circles.Add(c, el);
        }
        private void AddCircle_green()
        {
            HungerCircle c;
            Ellipse el;
            do
            {
                c = new HungerCircle(new Point((r.Next((int)screen.Width)), (r.Next((int)screen.Height))), Math.Round(r.NextDouble() * 25 + 5), 2, Color.FromRgb(0, 255, 0));
                el = new Ellipse();
                el.Width = c.Size * 2;
                el.Height = c.Size * 2;
                el.Fill = new SolidColorBrush(c.Color);
            } while (Intersect(c));
            screen.Children.Add(el);
            Canvas.SetTop(el, c.Position.Y - el.Height / 2);
            Canvas.SetLeft(el, c.Position.X - el.Width / 2);
            circles.Add(c, el);
        }
        private void AddCircle_blue()
        {
            HungerCircle c;
            Ellipse el;
            do
            {
                c = new HungerCircle(new Point((r.Next((int)screen.Width)), (r.Next((int)screen.Height))), Math.Round(r.NextDouble() * 25 + 5), 2, Color.FromRgb(0, 0, 255));
                el = new Ellipse();
                el.Width = c.Size * 2;
                el.Height = c.Size * 2;
                el.Fill = new SolidColorBrush(c.Color);
            } while (Intersect(c));
            screen.Children.Add(el);
            Canvas.SetTop(el, c.Position.Y - el.Height / 2);
            Canvas.SetLeft(el, c.Position.X - el.Width / 2);
            circles.Add(c, el);
        }
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (flag_reset == true)
            {
                flag_reset = false;
                btn_start.Content = "Reset";
                Start(); 
            }
            else
            {
                btn_start.Content= "Start";
                flag_reset = true;
                Reset();
            }
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            if (flagStopContinue == true)
            {
                btn_stop.Content = "Continue";
                btn_stop.Background = Brushes.LightBlue;
                flagStopContinue = false;
                t.Stop();
                t2.Stop();
            }
            else
            {
                btn_stop.Content = "Pause";
                btn_stop.Background = Brushes.LightBlue;
                flagStopContinue = true;
                t.Start();
                t2.Start();
            }
        }

        private void Reset()
        {
            List<HungerCircle> delete = new List<HungerCircle>(circles.Keys);
            delete = delete.FindAll(x => (x.Size >= 0));
            foreach (HungerCircle c in delete)
            {
                screen.Children.Remove(circles[c]);
                circles.Remove(c);
            }
            btn_stop.IsEnabled = false;
            btn_stop.Content = "Pause";
            lblcount1.Content = "";
            lblcount2.Content = "";
            lblcount3.Content = "";
            i=0;
            lblTime.Content = "";
            t.Stop();
            t2.Stop();
        }
    }
}
