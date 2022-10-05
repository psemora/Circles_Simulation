using System;
using System.Windows;
using System.Windows.Media;

namespace HungerCircle
{
    class HungerCircle
    {
        private Point position;
        private Point direction;
        private int step = 0;
        private double size;
        private int hunger;
        private Color color;

        public Color Color
        {
            get { return color; }
        }
        public int Hunger
        {
            get { return hunger; }
        }
        public double Size
        {
            get
            {
                if (size < 0)
                {
                    return 0;
                }
                return size;
            }
            set { size = value; }
        }
        public Point Position
        {
            get { return position; }
        }


        public HungerCircle(Point position, double size, int hunger, Color color)
        {
            this.position = position;
            this.size = size;
            this.hunger = hunger;
            this.color = color;
        }

        public Point Step(double width, double height, Random r)
        {
            if (step == 0)
            {
                NewDirection(r);
            }
            if (position.X < 0 && direction.X < 0)
            {
                direction.X = -direction.X;
            }
            if (position.Y < 0 && direction.Y < 0)
            {
                direction.Y = -direction.Y;
            }
            if (position.X > width && direction.X > 0)
            {
                direction.X = -direction.X;
            }
            if (position.Y > height && direction.Y > 0)
            {
                direction.Y = -direction.Y;
            }
            position.X += direction.X;
            position.Y += direction.Y;
            step--;

            return position;

        }

        private void NewDirection(Random r)
        {
            step = r.Next(60);
            double moveX, moveY;
            do
            {
                moveX = ((double)(r.Next(250) - 125)) / 100.0;
                moveY = ((double)(r.Next(250) - 125)) / 100.0;
            }
            /*
            while (moveY == 0 && moveX == 0 && moveX + moveY < 0.3);
            direction = new Point(moveX, moveY);
            */
        }


        public int Colide(HungerCircle circle)
        {
            double a = this.Position.X - circle.Position.X;
            double b = this.Position.Y - circle.Position.Y;
            double distance = Math.Sqrt(a * a + b * b);
            if (distance < (this.Size + circle.Size) & this.color == circle.color)
            {
                if (this.Size < circle.Size)
                {
                    return -1;
                }
                return 1;
            }
            return 0;
        }

        public void IsEaten()
        {
            size = 0;
        }

        public void Eat(HungerCircle circle)
        {
            hunger += (int)circle.Size * 2;
            size += circle.Size / 8;
        }
    }
}
