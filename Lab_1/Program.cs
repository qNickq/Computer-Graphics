using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;

namespace Lab_1
{
    public enum eTypeOfPoint
    {
        None,
        Anomaly,
        Target,
        Earth
    }
    class Point
    {
        static public int count = 0;
        public Point()
        {
            count++;
            Number = count;
            Square = 0;
            Type = eTypeOfPoint.None;
        }

        
        public int Number { get; set; }

        public int Square { get; set; }

        public eTypeOfPoint Type { get; set; }

        public override string ToString() => $"#{Number} || Square: {Square} || Type: {Type}.\n";

    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = "map.txt";

            int fieldW = 64;
            int fieldH = 64;

            int[,] field = ParseMap(path, fieldW, fieldH);

            List<Point> points = SearchPoints(field);
            
            foreach (Point point in points)
            {
                Console.WriteLine(point);
            }
        }
        static int[,] ParseMap(string path, int width, int height)
        {
            int[,] field = new int[width, height];
       
            try
            {
                using (StreamReader br = new StreamReader(path))
                {
                    string temp;
                    int j = 0;
                    while ((temp = br.ReadLine()) != null)
                    {
                        for (int i = 0; i < temp.Length; i++)
                        {
                            field[j, i] = int.Parse(temp[i].ToString());
                        }
                        j++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            return field;

        }
        static public List<Point> SearchPoints(int[,] field)
        {

            List<Point> points = new List<Point>();

            Point p;

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] == 1)
                    {
                        p = new Point();

                        p.Square = SearchPath(ref field, i, j);

                        switch (p.Square)
                        {
                            case 0:
                                {
                                    p.Type = eTypeOfPoint.None;
                                    break;
                                }
                            case 1:
                                {
                                    p.Type = eTypeOfPoint.Anomaly;
                                    break;
                                }
                            case 2:
                            case 3:
                            case 4:
                                {
                                    p.Type = eTypeOfPoint.Target;
                                    break;
                                }
                            default:
                                {
                                    p.Type = eTypeOfPoint.Earth;
                                    break;
                                }
                        }
                        points.Add(p);
                    }
                }
            }
            return points;
        }
        static public int SearchPath(ref int[,] field, int x, int y)
        {
            int count = 0;

            if (x >= 0 && y >= 0 && x < field.GetLength(0) && y < field.GetLength(1) && field[x,y] == 1)
            {
                
                field[x, y] = 2;

                count += SearchPath(ref field, x + 1, y);
                count += SearchPath(ref field, x - 1, y);
                count += SearchPath(ref field, x, y + 1);
                count += SearchPath(ref field, x, y - 1);
                count++;
            }
            else
            {
                return 0;
            }
            return count;
        }
    }
}