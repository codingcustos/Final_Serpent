using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using serpent_experiment;
using System.Numerics;

namespace Final_Serpent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            PictureBoxPos = new Point(canvas.Location.X, canvas.Location.Y);
            PictureBoxSizeX = canvas.Size.Width;
            PictureBoxSizeY = canvas.Size.Height;
            DavidMaxWidth = canvas.Size.Width / LogicalCellSize;
            DavidMaxHeight = canvas.Size.Height / LogicalCellSize;
            LogicalCellAmountX = PictureBoxSizeX / LogicalCellSize;
            LogicalCellAmountY = PictureBoxSizeY / LogicalCellSize;

            FoodBrush = Brushes.Red;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        List<Player> PlayerList = new List<Player>();
        List<Food> FoodList = new List<Food>();
        private Point PictureBoxPos;
        private int PictureBoxSizeX;
        private int PictureBoxSizeY;
        private int DavidMaxWidth;
        private int DavidMaxHeight;
        private int LogicalCellAmountX;
        private int LogicalCellAmountY;
        private const int LogicalCellSize = 20;
        private int TickCounter = 0;
        private int MoveCounter = 0;

        private Brush FoodBrush;
        private Brush LightFieldBrush;
        private Brush DarkFieldBrush;

        private int SelectedTickSpeed;
        private string SelectedTheme;

        private System.Timers.Timer gameTimerButFaster;

        private void OnGameTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                TickCounter++;
                foreach (Player player in PlayerList.Where(x => x.IsAlive))
                {
                    if (player is AIPlayer aiPlayer)
                    {
                        aiPlayer.UpdateDirection(FoodList, LogicalCellSize, PlayerList);
                    }

                    if (IsAlignedWithGrid(player.Positions[0]) && CheckIfDirectionIsNotOppositeToTempDirection(player.Direction, player.TempDirection))
                    {
                        PermitDirectionChange(player);
                    }
                    player.Move(LogicalCellSize / 4);
                    MoveCounter++;
                }
                canvas.Invalidate();
                foreach (Player player in PlayerList.Where(x => x.IsAlive))
                {
                    if (CheckCollision(player)) RemovePlayerFromTheLiving(player);
                }
                CheckFoodCollision();
            }));
        }

        private bool CheckIfDirectionIsNotOppositeToTempDirection(Directions direction, Directions tempDirection)
        {
            if (direction == Directions.Up && tempDirection == Directions.Down) return false;
            if (direction == Directions.Down && tempDirection == Directions.Up) return false;
            if (direction == Directions.Left && tempDirection == Directions.Right) return false;
            if (direction == Directions.Right && tempDirection == Directions.Left) return false;
            return true;
        }

        private void PermitDirectionChange(Player player)
        {
            player.Direction = player.TempDirection;
        }

        private bool IsAlignedWithGrid(PointF position)
        {
            return (position.X * LogicalCellSize) % LogicalCellSize == 0.0 && (position.Y * LogicalCellSize) % LogicalCellSize == 0.0;
        }

        private void InitializeTheme()
        {
            if (SelectedTheme == "Dark")
            {
                this.BackColor = Color.FromArgb(30, 30, 60);
                LightFieldBrush = new SolidBrush(Color.FromArgb(30, 30, 60));
                DarkFieldBrush = new SolidBrush(Color.FromArgb(15, 15, 45));
            }
            else
            {
                this.BackColor = Color.White;
                LightFieldBrush = new SolidBrush(Color.FromArgb(245, 245, 245));
                DarkFieldBrush = Brushes.White;
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int x = 0; x < LogicalCellAmountX; x++)
            {
                for (int y = 0; y < LogicalCellAmountY; y++)
                {
                    Brush brush = (x + y) % 2 == 0 ? LightFieldBrush : DarkFieldBrush;
                    g.FillRectangle(brush, x * LogicalCellSize, y * LogicalCellSize, LogicalCellSize, LogicalCellSize);
                }
            }

            foreach (Player player in PlayerList.Where(x => x.IsAlive))
            {
                foreach (PointF pos in player.Positions)
                {
                    g.FillRectangle(player.SnakeBrush, LogicalCellSize * pos.X, LogicalCellSize * pos.Y, LogicalCellSize, LogicalCellSize);
                }
            }

            foreach (Food food in FoodList)
            {
                g.FillRectangle(FoodBrush, LogicalCellSize * food.Position.X, LogicalCellSize * food.Position.Y, LogicalCellSize, LogicalCellSize);
            }
        }

        private void PlaceGenericFood()
        {
            Random rand = new Random();
            Point foodPoint = new Point(rand.Next(PictureBoxSizeX / LogicalCellSize), rand.Next(PictureBoxSizeY / LogicalCellSize));
            foreach (Player playerCheckFood in PlayerList)
            {
                foreach (PointF playerPointCheckFood in playerCheckFood.Positions)
                {
                    foreach (Food foodCheckOtherFoodColl in FoodList)
                    {
                        while ((PointF)foodPoint == playerPointCheckFood || foodPoint == foodCheckOtherFoodColl.Position)
                        {
                            foodPoint = new Point(rand.Next(PictureBoxSizeX), rand.Next(PictureBoxSizeY));
                        }
                    }
                }
            }
            Food newFood = new Food(foodPoint);
            FoodList.Add(newFood);
        }

        private bool CheckFoodCollision()
        {
            foreach (Player playerCheckFood in PlayerList)
            {
                foreach (Food foodCheckColl in FoodList)
                {
                    if ((PointF)foodCheckColl.Position == playerCheckFood.Positions[0])
                    {
                        SnakeEatFood(playerCheckFood);
                        FoodList.Remove(foodCheckColl);
                        PlaceGenericFood();
                        return true;
                    }
                }
            }
            return false;
        }

        public void SnakeEatFood(Player player)
        {
            player.LogicalSerpentLength++;
            for (int i = 0; i < (1.0f / player.StepSize); i++)
            {
                PointF head = player.Positions[0];
                PointF newHead = new PointF(head.X, head.Y);
                switch (player.Direction)
                {
                    case Directions.Left:
                        newHead.X -= player.StepSize;
                        break;
                    case Directions.Right:
                        newHead.X += player.StepSize;
                        break;
                    case Directions.Up:
                        newHead.Y -= player.StepSize;
                        break;
                    case Directions.Down:
                        newHead.Y += player.StepSize;
                        break;
                    default:
                        throw new Exception("Invalid direction given");
                }
                newHead = RoundPointF(newHead, 2);
                player.Positions.Insert(0, newHead);
                player.Positions.RemoveAt(player.Positions.Count - 1);

                head = newHead;
            }
        }

        private PointF RoundPointF(PointF point, int decimalPlaces)
        {
            float x = (float)Math.Round(point.X, decimalPlaces);
            float y = (float)Math.Round(point.Y, decimalPlaces);
            return new PointF(x, y);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Brush snakeBrush1;
            Brush snakeBrush2;
            float stepSizePl = 1.0f / (LogicalCellSize / 4);
            Food food1 = new Food(new Point(14, 6));
            Food food2 = new Food(new Point(32, 5));
            Food food3 = new Food(new Point(3, 13));
            Food food4 = new Food(new Point(27, 20));
            FoodList.Add(food1);
            FoodList.Add(food2);
            FoodList.Add(food3);
            FoodList.Add(food4);
            Player newPlayer = new Player(new PointF(5, 5), 15, stepSizePl, Directions.Right, snakeBrush1 = Brushes.Green);
            Player newPlayer2 = new Player(new PointF(10, 10), 15, stepSizePl, Directions.Right, snakeBrush2 = Brushes.LightGreen);
            AIPlayer aiPlayer = new AIPlayer(new PointF(15, 15), 15, stepSizePl, Directions.Right, Brushes.Blue);
            SettingsForm settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                SelectedTheme = settingsForm.SelectedTheme;
                InitializeTheme();
                SelectedTickSpeed = settingsForm.SelectedTickSpeed;

                gameTimerButFaster = new System.Timers.Timer(0.5);
                gameTimerButFaster.Elapsed += OnGameTimerElapsed;
                gameTimerButFaster.AutoReset = true;
                PlayerList.Add(newPlayer);
                PlayerList.Add(newPlayer2);
                PlayerList.Add(aiPlayer);
                gameTimerButFaster.Start();
            }
        }

        Directions playerDirection = Directions.Right;
        Directions player2Direction = Directions.Right;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            playerDirection = e.KeyCode switch
            {
                Keys.D when playerDirection != Directions.Left => Directions.Right,
                Keys.A when playerDirection != Directions.Right => Directions.Left,
                Keys.W when playerDirection != Directions.Down => Directions.Up,
                Keys.S when playerDirection != Directions.Up => Directions.Down,
                _ => playerDirection
            };

            player2Direction = e.KeyCode switch
            {
                Keys.Right when player2Direction != Directions.Left => Directions.Right,
                Keys.Left when player2Direction != Directions.Right => Directions.Left,
                Keys.Up when player2Direction != Directions.Down => Directions.Up,
                Keys.Down when player2Direction != Directions.Up => Directions.Down,
                _ => player2Direction
            };

            PlayerList[0].TempDirection = playerDirection;
            PlayerList[1].TempDirection = player2Direction;
        }

        public bool CheckCollision(Player player)
        {
            if (player.Positions[0].X < 0 || player.Positions[0].X >= DavidMaxWidth || player.Positions[0].Y < 0 || player.Positions[0].Y >= DavidMaxHeight)
            {
                return true;
            }

            if (MoveCounter > 1.5f * (1.0f / player.StepSize))
            {
                for (int i = 1; i < player.Positions.Count; i++)
                {
                    if (player.Positions[0] == player.Positions[i])
                    {
                        return true;
                    }
                }
            }

            foreach (Player pVPCollison in PlayerList)
            {
                if (player == pVPCollison) continue;
                if (MoveCounter > 1.5f * (1.0f / player.StepSize) && pVPCollison.IsAlive)
                {
                    for (int i = 1; i < pVPCollison.Positions.Count; i++)
                    {
                        if (player.Positions[0] == pVPCollison.Positions[i])
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void RemovePlayerFromTheLiving(Player player)
        {
            player.IsAlive = false;
            MakeDeadPlayerTurnToFood(player);
        }

        public void MakeDeadPlayerTurnToFood(Player player)
        {
            foreach (PointF posF in player.Positions)
            {
                bool tempBool = false;
                Point pos = new Point((int)posF.X, (int)posF.Y);
                Food newFood = new Food(pos);
                foreach (Food foodLoop in FoodList)
                {
                    if (foodLoop.Position == newFood.Position) tempBool = true;
                }
                if (tempBool) continue;
                FoodList.Add(newFood);
            }
        }
    }
}