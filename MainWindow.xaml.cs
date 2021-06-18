using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Zombie_Shooter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool goLeft, goRight, goUp, goDown;
        string facing = "Up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        Random randNum = new Random();
        Uri resourceUri;

        Rect playerHitBox;
        
        //List<Image> zombiesList = new List<Image>();
        List<Rectangle> zombiesList = new List<Rectangle>();

        
        public MainWindow()
        {
            InitializeComponent();

            // The timer
            gameTimer.Interval = TimeSpan.FromTicks(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            Stage.Focus();

            ImageBrush playerTexture = new ImageBrush();
            playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/up.png"));
            player.Fill = playerTexture;


            resourceUri = new Uri("/images/dead.png", UriKind.Relative);
            
        }

        private void GameLoop(object sender, EventArgs e)
        {
            
        }

        void MainTimerEvent(object sender, EventArgs e)
        {
            timer.Content = DateTime.Now.ToLongTimeString();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
                facing = "left";
                player1.Source = new BitmapImage(resourceUri);
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {

        }

        private void ShootBullet()
        {

        }

        private void MakeZombies()
        {

        }

        private void RestartGame()
        {

        }
    }
}
