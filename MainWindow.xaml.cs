using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Zombie_Shooter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "Up";
        int playerHealth = 100;
        int speed = 10;
        int ammoCount = 10;
        int score;
        int zombieSpeed = 3;
        Random randNum = new Random();
        Image ammo = new Image();
        Uri resourceUri;

        Rect playerHitBox;

        //List<Image> zombiesList = new List<Image>();
        List<Image> zombiesList = new List<Image>();

        DispatcherTimer gameTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            // The timer
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();

            Stage.Focus();
        }

        private void GameLoop(object sender, EventArgs e)
        {

        }

        void GameTimerEvent(object sender, EventArgs e)
        {
            //timer.Content = DateTime.Now.ToLongTimeString();

            if (playerHealth > 1)
                healthBar.Value = playerHealth;
            else
            {
                gameOver = true;
                player.Source = new BitmapImage(resourceUri = new Uri("/images/dead.png", UriKind.Relative));
                gameTimer.Stop();
            }

            txtAmmo.Content = "Ammo: " + ammoCount;
            txtScore.Content = "Kills: " + score;

            if (goLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - speed);
            }
            if (goRight && Canvas.GetLeft(player) + (player.Width) < Stage.ActualWidth)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + speed);
            }
            if (goUp && Canvas.GetTop(player) > 40)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - speed);
            }
            if (goDown && Canvas.GetTop(player) + (player.Height) < Stage.ActualHeight)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            }

            if (Canvas.GetLeft(player) + (player.Width) > Canvas.GetLeft(ammo) + (ammo.Width) &&
                        Canvas.GetLeft(player) + (player.Width) < Canvas.GetLeft(ammo) + (ammo.Width * 2) &&
                        Canvas.GetTop(player) + (player.Height) > Canvas.GetTop(ammo) + (ammo.Height) &&
                        Canvas.GetTop(player) + (player.Height) < Canvas.GetTop(ammo) + (ammo.Height * 2))
            {
                Stage.Children.Remove(ammo);

                if (ammoCount == 0)
                    ammoCount += 5;
            }         
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
                facing = "left";
                player.Source = new BitmapImage(resourceUri = new Uri("/images/left.png", UriKind.Relative));
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
                facing = "right";
                player.Source = new BitmapImage(resourceUri = new Uri("/images/right.png", UriKind.Relative));
            }
            if (e.Key == Key.Up)
            {
                goUp = true;
                facing = "up";
                player.Source = new BitmapImage(resourceUri = new Uri("/images/up.png", UriKind.Relative));
            }
            if (e.Key == Key.Down)
            {
                goDown = true;
                facing = "down";
                player.Source = new BitmapImage(resourceUri = new Uri("/images/down.png", UriKind.Relative));
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;
            }
            if (e.Key == Key.Up)
            {
                goUp = false;
            }
            if (e.Key == Key.Down)
            {
                goDown = false;
            }
            if (e.Key == Key.Space && ammoCount > 0)
            {
                ammoCount--;
                ShootBullet(facing);

                if (ammoCount < 1)
                {
                    DropAmmo();
                }
            }
        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();

            shootBullet.direction = direction;
            shootBullet.bulletLeft = Canvas.GetLeft(player) + (player.Width / 2);
            shootBullet.bulletTop = Canvas.GetTop(player) + (player.Height / 2);

            shootBullet.MakeBullet(Stage);
        }

        private void MakeZombies()
        {
            Image zombie = new Image();

            zombie.Name = "zombie";
            zombie.Source = new BitmapImage(resourceUri = new Uri("/images/zdown.png", UriKind.Relative));
            zombie.Width = 71;
            zombie.Height = 71;
            Canvas.SetLeft(zombie, randNum.Next(0, 900));
            Canvas.SetTop(zombie, randNum.Next(0, 700));
            zombiesList.Add(zombie);
            Stage.Children.Add(zombie);
            player.BringIntoView();
        }

        private void DropAmmo()
        {
            ammo.Name = "ammo";
            ammo.Source = new BitmapImage(resourceUri = new Uri("/images/ammo-Image.png", UriKind.Relative));
            ammo.Width = 50;
            ammo.Height = 50;
            Canvas.SetLeft(ammo, randNum.Next(10, Convert.ToInt32(Stage.ActualWidth - ammo.Width)));
            Canvas.SetTop(ammo, randNum.Next(20, Convert.ToInt32(Stage.ActualHeight - ammo.Height)));
            Stage.Children.Add(ammo);

            //Canvas.SetZIndex(player, 1);
        }

        private void RestartGame()
        {

        }
    }
}
