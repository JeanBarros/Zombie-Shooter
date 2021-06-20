using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zombie_Shooter
{
    class Bullet
    {
        public string direction;
        public double bulletLeft;
        public double bulletTop;

        private double speed = 25;
        // Create a Ellipse.
        private Ellipse bullet = new Ellipse();
        private DispatcherTimer bulletTimer = new DispatcherTimer();

        public void MakeBullet(Canvas stage)
        {
            // Create a SolidColorBrush with a color to fill the ellipse
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 255);
            
            bullet.Name = "bullet";
            bullet.Width = 2;
            bullet.Height = 2;
            bullet.Fill = mySolidColorBrush;
            Canvas.SetLeft(bullet, bulletLeft);
            Canvas.SetTop(bullet, bulletTop);

            // Add the Ellipse to the canvas.
            stage.Children.Add(bullet);
            //bullet.Visibility = Visibility.Hidden;

            bulletTimer.Interval = TimeSpan.FromMilliseconds(speed);
            bulletTimer.Tick += new EventHandler(BulletTimerEvent);
            bulletTimer.Start();
        }

        private void BulletTimerEvent(object sender, EventArgs e)
        {
            switch (direction)
            {
                case "left":
                    Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) - speed);
                    break;
                case "right":
                    Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + speed);
                    break;
                case "up":
                    Canvas.SetTop(bullet, Canvas.GetTop(bullet) - speed);
                    break;
                case "down":
                    Canvas.SetTop(bullet, Canvas.GetTop(bullet) + speed);
                    break;
                default:
                    //bullet.Visibility = Visibility.Visible;
                    break;
            }

            if (Canvas.GetLeft(bullet) < 0 || Canvas.GetLeft(bullet) > 940 || Canvas.GetTop(bullet) < 0 || Canvas.GetTop(bullet) > 700)
            {
                bulletTimer.Stop();
                bulletTimer = null;                
                bullet = null;
                
            }
        }
    }
}
