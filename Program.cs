using System;

namespace LabReport03
{
    abstract class Shape
    {
        public abstract double Area();

        public void DisplayShape()
        {
            Console.WriteLine("This shape is a: " + this.GetType().Name);
        }
    }

    class Circle : Shape
    {
        private double radius;

        public Circle(double radius)
        {
            this.radius = radius;
        }

        public override double Area()
        {
            return Math.PI * radius * radius;
        }
    }

    class Rectangle : Shape
    {
        private double length;
        private double width;

        public Rectangle(double length, double width)
        {
            this.length = length;
            this.width = width;
        }

        public override double Area()
        {
            return length * width;
        }
    }

    class Program
    {
        static void Main()
        {
            Circle circle = new Circle(5);
            circle.DisplayShape();
            Console.WriteLine("Area: " + circle.Area().ToString("F2"));

            Console.WriteLine();

            Rectangle rectangle = new Rectangle(4, 6);
            rectangle.DisplayShape();
            Console.WriteLine("Area: " + rectangle.Area().ToString("F2"));
        }
    }
}
