using System;
using System.Collections.Generic;
using System.Linq;
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
        double playerHealth = 100;
        int speed = 10;
        int ammoCount = 10;
        int score;
        int zombieSpeed = 3;

        private int enemyCounter = 3;

        // HitBox creates a box outside of the objects (bounds) which will be used to detect collision between two objects
        Rect playerHitBox, zombieHitBox, ammoHitBox, bulletHitBox;

        // Define images for the actors into stage
        ImageBrush playerTexture = new ImageBrush();
        ImageBrush zombieTexture = new ImageBrush();
        ImageBrush ammoTexture = new ImageBrush();

        Rectangle zombie; // Creates zombies 
        Rectangle ammo; // Creates ammo reloader into stage

        Random randNum = new Random();

        List<Rectangle> zombiesList = new List<Rectangle>();
        List<Rectangle> itemRemover = new List<Rectangle>();

        DispatcherTimer gameTimer = new DispatcherTimer();
        
        public MainWindow()
        {
            InitializeComponent();

            // The timer
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Start();

            Stage.Focus();

            // Define a background image for stage
            ImageBrush bg = new ImageBrush();

            playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/up.png"));
            player.Fill = playerTexture;
            //player.Stroke = Brushes.Red;
            //player.StrokeThickness = 2;

            RestartGame();
        }

        void GameTimerEvent(object sender, EventArgs e)
        {
            if (enemyCounter < 3)
            {
                MakeZombies(); 
                enemyCounter++;
            }

            if (playerHealth > 1)
                healthBar.Value = playerHealth;
            else
            {
                gameOver = true;
                playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dead.png"));
                player.Fill = playerTexture;
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

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            foreach (var item in Stage.Children.OfType<Rectangle>())
            {
                if ((string)item.Tag == "ammo")
                {
                    ammoHitBox = new Rect(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);
                    if (playerHitBox.IntersectsWith(ammoHitBox))
                    {
                        itemRemover.Add(item);
                        ammoCount += 5;
                    }
                }

                if ((string)item.Tag == "zombie")
                {
                    if (Canvas.GetLeft(item) > Canvas.GetLeft(player))
                    {
                        Canvas.SetLeft(item, Canvas.GetLeft(item) - zombieSpeed);
                        zombieTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/zleft.png"));
                        zombie.Fill = zombieTexture;
                    }
                    if (Canvas.GetLeft(item) < Canvas.GetLeft(player))
                    {
                        Canvas.SetLeft(item, Canvas.GetLeft(item) + zombieSpeed);
                        zombieTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/zright.png"));
                        zombie.Fill = zombieTexture;
                    }
                    if (Canvas.GetTop(item) > Canvas.GetTop(player))
                    {
                        Canvas.SetTop(item, Canvas.GetTop(item) - zombieSpeed);
                        zombieTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/zup.png"));
                        zombie.Fill = zombieTexture;
                    }
                    if (Canvas.GetTop(item) < Canvas.GetTop(player))
                    {
                        Canvas.SetTop(item, Canvas.GetTop(item) + zombieSpeed);
                        zombieTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/zdown.png"));
                        zombie.Fill = zombieTexture;
                    }
                    
                    zombieHitBox = new Rect(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);

                    if (zombieHitBox.IntersectsWith(playerHitBox))
                    {
                        playerHealth -= 0.2;

                        if (playerHealth < 75)
                            healthBar.Foreground = Brushes.DarkOrange;
                        if (playerHealth < 50)
                            healthBar.Foreground = Brushes.Red;
                        if (playerHealth < 25)
                            healthBar.Foreground = Brushes.DarkRed;
                    }
                }

                foreach (var shoot in Stage.Children.OfType<Rectangle>())
                {
                    if ((string)shoot.Tag == "bullet" && (string)item.Tag == "zombie")
                    {
                        bulletHitBox = new Rect(Canvas.GetLeft(shoot), Canvas.GetTop(shoot), shoot.Width, shoot.Height);

                        // Check collision between bullet and zombie objects
                        if (bulletHitBox.IntersectsWith(zombieHitBox))
                        {
                            score++;
                            itemRemover.Add(shoot);
                            itemRemover.Add(item);

                            enemyCounter --;                                                        
                        }

                        // Removes shooting if gets out of stage bounds
                        if (Canvas.GetLeft(shoot) < 0 || Canvas.GetLeft(shoot) > 940 || Canvas.GetTop(shoot) < 0 || Canvas.GetTop(shoot) > 700)
                            itemRemover.Add(shoot);
                    }
                }
            }

            foreach (Rectangle destroyedItem in itemRemover)
            {
                Stage.Children.Remove(destroyedItem);                
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver)
                return;
            
            if (e.Key == Key.Left)
            {
                goLeft = true;
                facing = "left";

                player.Width = 100;
                player.Height = 71;
                playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/left.png"));
                player.Fill = playerTexture;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
                facing = "right";

                player.Width = 100;
                player.Height = 71;
                playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/right.png"));
                player.Fill = playerTexture;
            }
            if (e.Key == Key.Up)
            {
                goUp = true;
                facing = "up";

                player.Width = 71;
                player.Height = 100;
                playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/up.png"));
                player.Fill = playerTexture;
            }
            if (e.Key == Key.Down)
            {
                goDown = true;
                facing = "down";

                player.Width = 71;
                player.Height = 100;
                playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/down.png"));
                player.Fill = playerTexture;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                goLeft = false;
            if (e.Key == Key.Right)
                goRight = false;
            if (e.Key == Key.Up)
                goUp = false;
            if (e.Key == Key.Down)
                goDown = false;
            if (e.Key == Key.Space && ammoCount > 0 && gameOver == false)
            {
                ammoCount--;
                ShootBullet(facing);

                if (ammoCount < 1)
                {
                    DropAmmo();
                }
            }

            if (e.Key==Key.Enter && gameOver)
            {
                RestartGame();
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
            zombie = new Rectangle();

            zombie.Tag = "zombie";
            zombieTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/zdown.png"));
            zombie.Fill = zombieTexture;
            zombie.Width = 71;
            zombie.Height = 71;
            //zombie.Stroke = Brushes.Yellow;
            //zombie.StrokeThickness = 2;            
            Canvas.SetLeft(zombie, randNum.Next(0, 900));
            Canvas.SetTop(zombie, randNum.Next(0, 700));
            zombiesList.Add(zombie);

            Stage.Children.Add(zombie);
        }

        private void DropAmmo()
        {
            ammo = new Rectangle();

            ammo.Tag = "ammo";
            ammoTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ammo-Image.png"));
            ammo.Fill = ammoTexture;
            ammo.Width = 50;
            ammo.Height = 50;
            //ammo.Stroke = Brushes.Blue;
            //ammo.StrokeThickness = 2;
            Canvas.SetLeft(ammo, randNum.Next(10, Convert.ToInt32(Stage.ActualWidth - ammo.Width)));
            Canvas.SetTop(ammo, randNum.Next(20, Convert.ToInt32(Stage.ActualHeight - ammo.Height)));

            Stage.Children.Add(ammo);
        }

        private void RestartGame()
        {
            playerTexture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/up.png"));
            player.Fill = playerTexture;

            foreach (Rectangle zombie in zombiesList)
            {
                Stage.Children.Remove(zombie);
            }

            zombiesList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeZombies();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammoCount = 10;

            gameTimer.Start();
        }
    }
}
