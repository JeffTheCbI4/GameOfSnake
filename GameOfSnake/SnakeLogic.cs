using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.Threading;

namespace GameOfSnake
{
    public enum MoveDirections { UP, DOWN, LEFT, RIGHT }
    abstract class Item
    {
        public String Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int PointWorth { get; set; }

        public abstract void Consumed(SnakeLogic logic);
    }

    abstract class Bonus : Item
    {
        public int MapTime { get; set; }
        public int ActiveTime { get; set; }

        public abstract bool ReduceMapTime();
        public abstract bool ReduceActiveTime();
        public abstract void StopBonus(SnakeLogic logic);

    }

    class SnakeLogic
    {
        int XBoundary { get; set; }
        int YBoundary { get; set; }
        //Лист со всеми клетками, содержащими тело змеи.
        //Последний элемент - голова.
        //Первый элемент - хвост.
        public Snake PlayerSnake { get; set; }
        public List<Item> ConsumableItems { get; set; }
        public List<Bonus> ActiveBonuses { get; set; }
        private int _points;
        public int Points { get { return _points; } set { _points = value; } }
        public int PointsModifier { get; set; }
        public bool GameIsOver { get; set; }

        public SnakeLogic(int xBoundary, int yBoundary)
        {
            ConsumableItems = new List<Item>();
            ActiveBonuses = new List<Bonus>();
            XBoundary = xBoundary - 1;
            YBoundary = yBoundary - 1;
            PointsModifier = 1;
            Points = 0;
            GameIsOver = false;

            Random rand = new Random();
            int foodX = rand.Next(0, XBoundary);
            int foodY = rand.Next(0, YBoundary);
            ConsumableItems.Add(new Food(foodX, foodY));

            int startX = 3;
            int startY = YBoundary / 2;
            Tuple<int, int> startCoords = new Tuple<int, int>(startX, startY);
            PlayerSnake = new Snake(3, startCoords, MoveDirections.RIGHT);
        }

        public void PlayStep(MoveDirections direction)
        {
            if (GameIsOver) return;
            PlayerSnake.Move(direction);
            //Проверяем, съела ли змея что-нибудь
            checkConsumptionAndRemove(this);
            GenerateBonus();
            TimeOutBonuses();
            GameIsOver = DetermineIfGameOver(this);
        }

        private static void checkConsumptionAndRemove(SnakeLogic logic)
        {
            List<int> indicesToRemove = new List<int>();
            Tuple<int, int> head = logic.PlayerSnake.getHead();
            for (int i = 0; i < logic.ConsumableItems.Count; i++)
            {
                Item item = logic.ConsumableItems[i];
                if (head.Item1 == item.X && head.Item2 == item.Y)
                {
                    item.Consumed(logic);
                    if (item is Bonus) indicesToRemove.Add(i);
                }
            }
            foreach (int i in indicesToRemove)
            {
                logic.ConsumableItems.RemoveAt(i);
            }
        }

        private static bool DetermineIfGameOver(SnakeLogic logic)
        {
            Tuple<int, int> head = logic.PlayerSnake.getHead();
            if (head.Item1 < 0 || head.Item1 > logic.XBoundary ||
                head.Item2 < 0 || head.Item2 > logic.YBoundary) return true;

            for (int i = 0; i < logic.PlayerSnake.SnakeBody.Count - 1; i++)
            {
                Tuple<int, int> bodyPart = logic.PlayerSnake.SnakeBody[i];
                if (head.Item1 == bodyPart.Item1 && head.Item2 == bodyPart.Item2) return true;
            }
            return false;
        }

        private void GenerateBonus()
        {
            Random rand = new Random();
            int probability = rand.Next(1, 100);
            if (probability <= 5 )
            {
                int x = rand.Next(0, XBoundary);
                int y = rand.Next(0, YBoundary);
                Point2xBonus bonus = new Point2xBonus(x, y);
                ConsumableItems.Add(bonus);
            }
        }

        public void TimeOutBonuses()
        {
            List<int> bonusIndicesToRemove = new List<int>();
            for (int i = 0; i < ConsumableItems.Count; i++)
            {
                Item item = ConsumableItems[i];
                if (item is Bonus)
                {
                    Bonus bonus = (Bonus)item;
                    if (bonus.ReduceMapTime())
                    {
                        bonusIndicesToRemove.Add(i);
                    }
                }
            }
            bonusIndicesToRemove.ForEach(n => ConsumableItems.RemoveAt(n));

            bonusIndicesToRemove = new List<int>();
            for (int i = 0; i < ActiveBonuses.Count; i++)
            {
                if (ActiveBonuses[i].ReduceActiveTime())
                {
                    bonusIndicesToRemove.Add(i);
                    ActiveBonuses[i].StopBonus(this);
                }
            }
            bonusIndicesToRemove.ForEach(n => ActiveBonuses.RemoveAt(n));
        }

        private void AddPoints(int value)
        {
            Points += value * PointsModifier;
        }

        public class Snake
        {
            public List<Tuple<int, int>> SnakeBody { get; set; }
            MoveDirections LastMoveDirection { get; set; }

            public Snake(int length, Tuple<int, int> startingCoords, MoveDirections startingDirection)
            {
                LastMoveDirection = startingDirection;

                SnakeBody = new List<Tuple<int, int>>();
                int x = startingCoords.Item1;
                int y = startingCoords.Item2;
                int dx = 0;
                int dy = 0;
                switch(startingDirection)
                {
                    case MoveDirections.UP:
                        dy = -1;
                        break;
                    case MoveDirections.DOWN:
                        dy = +1;
                        break;
                    case MoveDirections.LEFT:
                        dx = -1;
                        break;
                    case MoveDirections.RIGHT:
                        dx = +1;
                        break;
                    default:
                        throw new Exception("Snake direction was not set");
                }

                for (int i = 0; i < length; i++)
                {
                    Tuple<int, int> bodyPart = new Tuple<int, int>(x + dx, y + dy);
                    SnakeBody.Add(bodyPart);
                    x += dx;
                    y += dy;
                }
            }

            public void Move(MoveDirections direction)
            {
                Tuple<int, int> head = SnakeBody.Last();
                if ((direction == MoveDirections.UP && LastMoveDirection == MoveDirections.DOWN) ||
                    (direction == MoveDirections.DOWN && LastMoveDirection == MoveDirections.UP) ||
                    (direction == MoveDirections.RIGHT && LastMoveDirection == MoveDirections.LEFT) ||
                    (direction == MoveDirections.LEFT && LastMoveDirection == MoveDirections.RIGHT))
                {
                    direction = LastMoveDirection;
                }
                switch (direction)
                {
                    case MoveDirections.UP:
                        SnakeBody.Add(new Tuple<int, int>(head.Item1, head.Item2 - 1));
                        break;
                    case MoveDirections.DOWN:
                        SnakeBody.Add(new Tuple<int, int>(head.Item1, head.Item2 + 1));
                        break;
                    case MoveDirections.RIGHT:
                        SnakeBody.Add(new Tuple<int, int>(head.Item1 + 1, head.Item2));
                        break;
                    case MoveDirections.LEFT:
                        SnakeBody.Add(new Tuple<int, int>(head.Item1 - 1, head.Item2));
                        break;
                    default:
                        break;
                }
                LastMoveDirection = direction;
                RemoveTailPart();
            }

            public void AddTailPart()
            {
                Tuple<int, int> tail = SnakeBody[0];
                Tuple<int, int> tail2 = SnakeBody[1];
                int dx = tail2.Item1 - tail.Item1;
                int dy = tail2.Item2 - tail.Item2;
                Tuple<int, int> newTail = new Tuple<int, int>(tail.Item1 - dx, tail.Item2 - dy);
                SnakeBody = SnakeBody.Prepend(newTail).ToList();
            }

            public void RemoveTailPart()
            {
                SnakeBody.RemoveAt(0);
            }

            public Tuple<int,int> getHead()
            {
                return SnakeBody.Last();
            }

            public Tuple<int, int> getTail()
            {
                return SnakeBody[0];
            }

            //i - номер части тела от головы, начиная с 0
            public Tuple<int, int> getMidBodyFromHead(int i)
            {
                return SnakeBody[SnakeBody.Count - 2 - i];
            }
            public Tuple<int, int> getMidBodyFromTail(int i)
            {
                return SnakeBody[i + 1];
            }
        }

        public class Food : Item
        {
            public int ImageNumber { get; set; }
            public Food(int x, int y)
            {
                X = x;
                Y = y;
                Name = "Food";
                //выбираем случайное изображение еды по номеру
                ImageNumber = new Random().Next(1, 4);
                PointWorth = 1000;
            }
            override public void Consumed(SnakeLogic logic)
            {
                logic.AddPoints(PointWorth);
                logic.PlayerSnake.AddTailPart();

                Random rand = new Random();
                X = rand.Next(0, logic.XBoundary);
                Y = rand.Next(0, logic.YBoundary);
                //выбираем случайное изображение еды по номеру
                ImageNumber = new Random().Next(1, 4);
                GameMediaPlayer.SoundsDict["Eat"].Play();
            }
        }

        class Point2xBonus : Bonus
        {
            public Point2xBonus(int x, int y)
            {
                X = x;
                Y = y;
                Name = "Point2xBonus";
                PointWorth = 100;
                MapTime = 20;
                ActiveTime = 100;
            }
            override public void Consumed(SnakeLogic logic)
            {
                logic.AddPoints(PointWorth);
                logic.PointsModifier *= 2;
                logic.ActiveBonuses.Add(this);

                GameMediaPlayer.SoundsDict["Bonus"].Play();
            }

            override public bool ReduceMapTime()
            {
                MapTime -= 1;
                return MapTime <= 0;
            }

            override public bool ReduceActiveTime()
            {
                ActiveTime -= 1;
                return ActiveTime <= 0;
            }

            override public void StopBonus(SnakeLogic logic)
            {
                logic.PointsModifier /= 2;
            }
        }
    }
}
