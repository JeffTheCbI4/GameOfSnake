using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace GameOfSnake
{
    //TODO Переписать рисование
    public partial class PlayingForm : Form
    {
        Form PreviousForm;
        ImageStorage GameImagesStorage;
        List<GameObjectImageInfo> SnakeBodyToDraw;
        List<GameObjectImageInfo> BonusesToDraw;
        List<GameObjectImageInfo> ActiveBonusesToDraw;
        GameObjectImageInfo FoodToDraw;
        List<Tuple<Point, Point>> LineGridToDraw;

        SnakeLogic SnakeLogic;
        bool isPlaying;

        int XBoundary = 30;
        int YBoundary = 20;

        MoveDirections CurrentDirection = MoveDirections.RIGHT;

        public PlayingForm(Form previousForm)
        {
            InitializeComponent();
            PreviousForm = previousForm;
            Init();

            int xPixels = pictureBox1.Width;
            int yPixels = pictureBox1.Height;
            for (int i = 1; i < XBoundary; i++)
            {
                Point start = new Point(i * (xPixels / XBoundary), 0);
                Point end = new Point(i * (xPixels / XBoundary), yPixels);
                LineGridToDraw.Add(new Tuple<Point, Point>(start, end));
            }
            for (int i = 1; i < YBoundary; i++)
            {
                Point start = new Point(0, i * (yPixels / YBoundary));
                Point end = new Point(xPixels, i * (yPixels / YBoundary));
                LineGridToDraw.Add(new Tuple<Point, Point>(start, end));
            }
            pictureBox1.Refresh();
        }

        public void Init()
        {
            GameImagesStorage = new ImageStorage();
            GameImagesStorage.LoadImages();
            SnakeBodyToDraw = new List<GameObjectImageInfo>();
            BonusesToDraw = new List<GameObjectImageInfo>();
            ActiveBonusesToDraw = new List<GameObjectImageInfo>();
            LineGridToDraw = new List<Tuple<Point, Point>>();
        }

        async private void PlayButton_Click(object sender, EventArgs e)
        {
            PlayButton.Enabled = false;
            GameOverLabel.Visible = false;
            SnakeLogic = new SnakeLogic(XBoundary, YBoundary);
            await PlaySnake(SnakeLogic);
            PlayButton.Enabled = true;
        }

        async private Task PlaySnake(SnakeLogic snakeLogic)
        {
            isPlaying = true;
            while (isPlaying && !snakeLogic.GameIsOver)
            {
                snakeLogic.PlayStep(CurrentDirection);
                UpdateFrame(snakeLogic);
                pictureBox1.Invalidate();
                activeBonusesPictureBox.Invalidate();
                await Task.Delay(100);
            }
            isPlaying = false;
            GameOverLabel.Visible = true;
            GameMediaPlayer.SoundsDict["GameOver"].Play();
        }

        private void UpdateFrame(SnakeLogic snakeLogic)
        {
            SnakeBodyToDraw.Clear();
            BonusesToDraw.Clear();
            ActiveBonusesToDraw.Clear();

            int xPixels = pictureBox1.Width;
            int yPixels = pictureBox1.Height;

            int recWidth = xPixels / XBoundary;
            int recHeight = yPixels / YBoundary;

            UpdateSnake(snakeLogic.PlayerSnake);
            OrganizeActiveBonuses(snakeLogic.ActiveBonuses);

            foreach(Item item in snakeLogic.ConsumableItems)
            {
                int x = item.X * recWidth;
                int y = item.Y * recHeight;
                if (item is SnakeLogic.Food)
                {
                    SnakeLogic.Food food = (SnakeLogic.Food)item;
                    int imageNumber = food.ImageNumber;
                    Bitmap image = GameImagesStorage.ImagesDict["Food" + imageNumber];
                    FoodToDraw = new GameObjectImageInfo(image, x, y);
                } 
                else if (item is Bonus)
                {
                    Bitmap image = GameImagesStorage.ImagesDict["Bonus2x"];
                    //Rectangle bonusRec = new Rectangle(x, y, recWidth, recHeight);
                    GameObjectImageInfo info = new GameObjectImageInfo(image, x, y);
                    BonusesToDraw.Add(info);
                }
            }
            PointsNumber.Text = snakeLogic.Points.ToString();
        }

        private void UpdateSnake(SnakeLogic.Snake snake)
        {
            Tuple<int, int> tail = snake.getTail();
            Bitmap tailImage = DetermineHeadTailRotationImage(tail, snake.getMidBodyFromTail(0), false);
            int x = (pictureBox1.Width / XBoundary) * tail.Item1;
            int y = (pictureBox1.Height / YBoundary) * tail.Item2;
            SnakeBodyToDraw.Add(new GameObjectImageInfo(tailImage, x, y));

            for (int i = 0; i < snake.SnakeBody.Count - 2; i++)
            {
                Tuple<int, int> prevPart = new Tuple<int, int>(0,0);
                Tuple<int, int> currentPart = new Tuple<int, int>(0, 0);
                Tuple<int, int> nextPart = new Tuple<int, int>(0, 0);
                if (i == 0) prevPart = snake.getTail();
                else prevPart = snake.getMidBodyFromTail(i - 1);
                currentPart = snake.getMidBodyFromTail(i);
                if (i == snake.SnakeBody.Count - 3) nextPart = snake.getHead();
                else nextPart = snake.getMidBodyFromTail(i + 1);
                Bitmap partImage = DetermineSnakeBodyPartImage(prevPart, currentPart, nextPart);

                x = (pictureBox1.Width / XBoundary) * currentPart.Item1;
                y = (pictureBox1.Height / YBoundary) * currentPart.Item2;
                SnakeBodyToDraw.Add(new GameObjectImageInfo(partImage, x, y));
            }

            Tuple<int, int> head = snake.getHead();
            Bitmap headImage = DetermineHeadTailRotationImage(head, snake.getMidBodyFromHead(0), true);
            x = (pictureBox1.Width / XBoundary) * head.Item1;
            y = (pictureBox1.Height / YBoundary) * head.Item2;
            SnakeBodyToDraw.Add(new GameObjectImageInfo(headImage, x, y));
        }

        private Bitmap DetermineSnakeBodyPartImage(Tuple<int, int> prevPart,
                                                  Tuple<int, int> currentPart,
                                                  Tuple<int, int> nextPart)
        {
            Bitmap image = null;
            //Bitmap straight = (Bitmap)GameImagesStorage.ImagesDict["SnakeBody"].Clone();
            //Bitmap LShape = (Bitmap)GameImagesStorage.ImagesDict["SnakeBodyLShape1"].Clone();

            int dxNext = nextPart.Item1 - currentPart.Item1;
            int dxPrev = currentPart.Item1 - prevPart.Item1;
            int dyNext = nextPart.Item2 - currentPart.Item2;
            int dyPrev = currentPart.Item2 - prevPart.Item2;

            if (dyNext == 0 && dyPrev == 0)
            {
                image = GameImagesStorage.ImagesDict["SnakeBodyHorizontal"];
            }
            else if (dxNext == 0 && dxPrev == 0)
            {
                image = GameImagesStorage.ImagesDict["SnakeBodyVertical"];
            }
            else if ((dxNext == 1 && dyPrev == -1) ||
                     (dxPrev == -1 && dyNext == 1))
            {
                image = GameImagesStorage.ImagesDict["SnakeBodyLShape0"];
            }
            else if ((dxNext == 1 && dyPrev == 1) ||
                     (dxPrev == -1 && dyNext == -1))
            {
                image = GameImagesStorage.ImagesDict["SnakeBodyLShape270"];
                //image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            else if ((dxPrev == 1 && dyNext == -1) ||
                     (dxNext == -1 && dyPrev == 1))
            {
                image = GameImagesStorage.ImagesDict["SnakeBodyLShape180"];
                //image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if ((dxPrev == 1 && dyNext == 1) ||
                     (dxNext == -1 && dyPrev == -1))
            {
                image = GameImagesStorage.ImagesDict["SnakeBodyLShape90"];
                //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            return image;
        }

        //true - head
        //false - tail
        private Bitmap DetermineHeadTailRotationImage(Tuple<int, int> currentPart,
                                                  Tuple<int, int> neighBoorPart, bool headOrTail)
        {
            string name = headOrTail ? "SnakeHead" : "SnakeTail";
            Bitmap image = GameImagesStorage.ImagesDict[name + 0];
            int dx = currentPart.Item1 - neighBoorPart.Item1;
            int dy = currentPart.Item2 - neighBoorPart.Item2;
            if (dx == -1)
            {
                image = GameImagesStorage.ImagesDict[name + 180];
                //image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            } else if (dy == 1)
            {
                image = GameImagesStorage.ImagesDict[name + 90];
                //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            } else if (dy == -1)
            {
                image = GameImagesStorage.ImagesDict[name + 270];
                //image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }

            return image;
        }

        private void OrganizeActiveBonuses(List<Bonus> bonuses)
        {
            int imageHeight = 48;
            int imageWidth = 48;
            int padding = 10;

            int picHeight = activeBonusesPictureBox.Height;
            int picWidth = activeBonusesPictureBox.Width;

            int currentColumn = 0;
            int currentRow = 0;
            for (int i = 0; i < bonuses.Count; i++)
            {
                int x = padding * (currentColumn + 1) + imageWidth * currentColumn;
                int y = padding * (currentRow + 1) + imageHeight * currentRow;
                Bitmap image = GameImagesStorage.ImagesDict["ActiveBonus2x"];
                GameObjectImageInfo info = new GameObjectImageInfo(image, x, y);
                ActiveBonusesToDraw.Add(info);
                currentColumn++;
                if (x + (imageWidth * 2) + padding > picWidth)
                {
                    currentRow++;
                    currentColumn = 0;
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen generalPen = new Pen(Color.GreenYellow);

            for (int i = 0; i < LineGridToDraw.Count; i++)
            {
                Tuple<Point, Point> line = LineGridToDraw[i];
                g.DrawLine(generalPen, line.Item1, line.Item2);
            }

            /*if (SnakeBodyToDraw != null && SnakeBodyToDraw.Count > 0)
            {
                for (int i = 0; i < SnakeBodyToDraw.Count - 1; i++)
                {
                    g.FillRectangle(generalBrush, SnakeBodyToDraw[i]);
                }
                g.FillRectangle(headBrush, SnakeBodyToDraw.Last());
            }*/

            if (SnakeBodyToDraw != null && SnakeBodyToDraw.Count > 0)
            {
                for (int i = 0; i < SnakeBodyToDraw.Count; i++)
                {
                    Bitmap image = SnakeBodyToDraw[i].ObjectImage;
                    int x = SnakeBodyToDraw[i].X;
                    int y = SnakeBodyToDraw[i].Y;
                    g.DrawImage(image, x, y);
                }
            }

            if (BonusesToDraw != null)
            {
                for (int i = 0; i < BonusesToDraw.Count; i++)
                {
                    Bitmap image = BonusesToDraw[i].ObjectImage;
                    int x = BonusesToDraw[i].X;
                    int y = BonusesToDraw[i].Y;
                    g.DrawImage(image, x, y);
                }
            }

            if (FoodToDraw != null)
            {
                int x = FoodToDraw.X;
                int y = FoodToDraw.Y;
                Bitmap image = FoodToDraw.ObjectImage;
                g.DrawImage(image, x, y);
            }
        }

        private void PlayingForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W:
                    CurrentDirection = MoveDirections.UP;
                    break;
                case Keys.S:
                    CurrentDirection = MoveDirections.DOWN;
                    break;
                case Keys.A:
                    CurrentDirection = MoveDirections.LEFT;
                    break;
                case Keys.D:
                    CurrentDirection = MoveDirections.RIGHT;
                    break;
                default:
                    break;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            PreviousForm.Show();
            this.Hide();
        }

        private void activeBonusesPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (ActiveBonusesToDraw != null)
            {
                for (int i = 0; i < ActiveBonusesToDraw.Count; i++)
                {
                    Bitmap image = ActiveBonusesToDraw[i].ObjectImage;
                    int x = ActiveBonusesToDraw[i].X;
                    int y = ActiveBonusesToDraw[i].Y;
                    g.DrawImage(image, x, y);
                }
            }
        }

        private void PlayingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PreviousForm.Close();
        }

        private class GameObjectImageInfo
        {
            public Bitmap ObjectImage { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public GameObjectImageInfo(Bitmap objectImage, int x, int y)
            {
                ObjectImage = objectImage;
                X = x;
                Y = y;
            }
        }

        private class ImageStorage
        {
            public Dictionary<string, Bitmap> ImagesDict { get; set; }

            public ImageStorage()
            {
                ImagesDict = new Dictionary<string, Bitmap>();
            }

            public void LoadImages()
            {
                string[] filesNames = Directory.GetFiles(@"Images\");
                foreach (string path in filesNames)
                {
                    Bitmap image = new Bitmap(path);
                    string name = path.Replace("Images\\", "").Replace(".png", "");
                    ImagesDict.Add(name, image);
                }
                PrepareSnakeImages();
            }

            private void PrepareSnakeImages()
            {
                Bitmap headImage = (Bitmap)ImagesDict["SnakeHead0"].Clone();
                int rotateDegree = 90;
                for (int i = 0; i < 3; i++)
                {
                    headImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    Bitmap image = (Bitmap)headImage.Clone();
                    ImagesDict.Add("SnakeHead" + rotateDegree, image);
                    rotateDegree += 90;
                }

                Bitmap tailImage = (Bitmap)ImagesDict["SnakeTail0"].Clone();
                rotateDegree = 90;
                for (int i = 0; i < 3; i++)
                {
                    tailImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    Bitmap image = (Bitmap)tailImage.Clone();
                    ImagesDict.Add("SnakeTail" + rotateDegree, image);
                    rotateDegree += 90;
                }

                Bitmap LShapeImage = (Bitmap)ImagesDict["SnakeBodyLShape0"].Clone();
                rotateDegree = 90;
                for (int i = 0; i < 3; i++)
                {
                    LShapeImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    Bitmap image = (Bitmap)LShapeImage.Clone();
                    ImagesDict.Add("SnakeBodyLShape" + rotateDegree, image);
                    rotateDegree += 90;
                }

                Bitmap horizontaImage = (Bitmap)ImagesDict["SnakeBodyHorizontal"].Clone();
                horizontaImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                ImagesDict.Add("SnakeBodyVertical", horizontaImage);
            }
        }
    }
}
