using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star {
    /// <summary>
    /// A星算法
    /// </summary>
    class A_Star {
        public const int OBLIQUE = 14; //对角移动的权值
        public const int STEP = 10; //上下左右移动的权值
        public int[,] mazeArr { get; private set; } //迷宫
        List<Point> closeList;//已经找过了的列表
        List<Point> waitList;//
        public A_Star (int[,] maze) {
            this.mazeArr = maze;
            waitList = new List<Point>(mazeArr.Length);
            closeList = new List<Point>(mazeArr.Length);
        }
        public Point FindPath (Point start , Point end , bool IsIgnoreCorner) {
            waitList.Add(start);
            while (waitList.Count != 0) {
                waitList = waitList.OrderBy(p => p.F).ToList();//找出F值最小的点
                var startPoint = waitList[0];
                waitList.RemoveAt(0);
                closeList.Add(startPoint); //已经找过了
                var roundPoints = FindRoundPoints(startPoint , IsIgnoreCorner); //找出它相邻的八个点
                foreach (Point roundPoint in roundPoints) {
                    if (waitList.Exists(roundPoint)) {
                        ComparePath(startPoint , roundPoint); //计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
                    } else {
                        AddNewPath(startPoint , end , roundPoint);//如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
                    }
                }
                if (waitList.Get(end) != null)
                    return waitList.Get(end);
            }
            return waitList.Get(end);
        }
        private void ComparePath (Point startPoint , Point roundPoint) {
            var G = CalcG(startPoint , roundPoint);
            if (G < roundPoint.G) {
                roundPoint.lastPoint = startPoint;
                roundPoint.G = G;
                roundPoint.CalcF();
            }
        }
        private void AddNewPath (Point start , Point end , Point roundPoint) {
            roundPoint.lastPoint = start;
            roundPoint.G = CalcG(start , roundPoint);
            roundPoint.H = CalcH(end , roundPoint);
            roundPoint.CalcF();
            waitList.Add(roundPoint);
        }
        private int CalcG (Point start , Point point) {
            int G = (Math.Abs(point.X - start.X) + Math.Abs(point.Y - start.Y)) == 2 ? STEP : OBLIQUE;
            int parentG = point.lastPoint != null ? point.lastPoint.G : 0;
            return G + parentG;
        }

        private int CalcH (Point end , Point point) {
            int step = Math.Abs(point.X - end.X) + Math.Abs(point.Y - end.Y);
            return step * STEP;
        }

        //获取某个点周围可以到达的点
        public List<Point> FindRoundPoints (Point centerPoint , bool IsIgnoreCorner) {
            var surroundPoints = new List<Point>(9);

            for (int x = centerPoint.X - 1 ; x <= centerPoint.X + 1 ; x++)
                for (int y = centerPoint.Y - 1 ; y <= centerPoint.Y + 1 ; y++) {
                    if (CanReach(centerPoint , x , y , IsIgnoreCorner))
                        surroundPoints.Add(x , y);
                }
            return surroundPoints;
        }

        //在二维数组对应的位置不为障碍物
        private bool CanReach (int x , int y) {
            return mazeArr[x , y] == 0;
        }

        public bool CanReach (Point start , int x , int y , bool IsIgnoreCorner) {
            if (!CanReach(x , y) || closeList.Exists(x , y))    //是否已经
                return false;
            else {
                if (Math.Abs(x - start.X) + Math.Abs(y - start.Y) == 1)
                    return true;
                //如果是斜方向移动, 判断是否 "拌脚"
                else {
                    if (CanReach(Math.Abs(x - 1) , y) && CanReach(x , Math.Abs(y - 1)))
                        return true;
                    else
                        return IsIgnoreCorner;
                }
            }
        }
    }

    //路径点类型 类型
    public class Point {
        public Point lastPoint { get; set; }
        public int F { get; set; } //F=G+H
        public int G { get; set; }
        public int H { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Point (int x , int y) {
            this.X = x;
            this.Y = y;
        }
        public void CalcF () {
            this.F = this.G + this.H;
        }
    }

    //对 List<PoinT> 的一些扩展方法，方便使用
    public static class ListHelper {
        public static bool Exists (this List<Point> points , Point point) {
            foreach (Point p in points)
                if ((p.X == point.X) && (p.Y == point.Y))
                    return true;
            return false;
        }

        public static bool Exists (this List<Point> points , int x , int y) {
            foreach (Point p in points)
                if ((p.X == x) && (p.Y == y))
                    return true;
            return false;
        }

        public static Point MinPoint (this List<Point> points) {
            points = points.OrderBy(p => p.F).ToList();
            return points[0];
        }
        public static void Add (this List<Point> points , int x , int y) {
            Point point = new Point(x , y);
            points.Add(point);
        }

        public static Point Get (this List<Point> points , Point point) {
            foreach (Point p in points)
                if ((p.X == point.X) && (p.Y == point.Y))
                    return p;
            return null;
        }
        public static void Remove (this List<Point> points , int x , int y) {
            foreach (Point point in points) {
                if (point.X == x && point.Y == y)
                    points.Remove(point);
            }
        }
    }
}
