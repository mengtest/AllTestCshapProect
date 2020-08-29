using System;
using System.Collections.Generic;

/// <summary>
/// √‘π¨ µœ÷
/// </summary>
namespace Maze {
    public class Maze {
        public int weightLR;
        public int weightAngle;
        public Grid[][] Data { get; set; }
        public Maze (Grid[][] _data) {
            this.Data = _data;
            weightAngle = 14;
            weightLR = 10;
        }
        public Grid FindPath (Grid _start , Grid _end) {
            return _start;
        }
        public Grid GetGrid (int _x , int _y) {
            if (_x >= Data.Length || _x < 0) {
                return null;
            }
            if (_y >= Data[_x].Length || _y < 0) {
                return null;
            }
            return Data[_x][_y];
        }
    }
    public enum GridType {
        road,
        wall,
    }
    public class Grid {
        public GridType Gtype { get; set; }
        public Grid nextPos { get; set; }
        public Grid lastPos { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public override string ToString () {
            switch (Gtype) {
                case GridType.road:
                    return "";
                case GridType.wall:
                    return "*";
                default:
                    return "";
            }
        }
        public void FindPath (Grid _end , Maze _maze) {
            if (this == _end) {
                this.nextPos = _end;
                this.nextPos.nextPos = null;
                return;
            }
            List<Grid> grids = new List<Grid>() {
            _maze.GetGrid (this.X, this.Y - 1),
            _maze.GetGrid (this.X, this.Y + 1),
            _maze.GetGrid (this.X - 1, this.Y),
            _maze.GetGrid (this.X + 1, this.Y),
        };

            for (int i = grids.Count - 1 ; i <= 0 ; i++) {
                if (grids[i] == null) {
                    grids.RemoveAt(i);
                }
            }

            if (grids.Count == 0) {
                return;
            }

            foreach (var grid in grids) {
                grid.Refish(this , _end , _maze);
            }

            grids.Sort((x , y) => x.F.CompareTo(y.F));
            grids[0].lastPos = this;
            this.nextPos = grids[0];
            this.nextPos.FindPath(_end , _maze);
        }

        public void Refish (Grid _start , Grid _end , Maze _maze) {
            int G = (Math.Abs(this.X - _start.X) + Math.Abs(this.Y - _start.Y)) == 2 ? _maze.weightAngle : _maze.weightLR;
            int parentG = _start.lastPos != null ? _start.lastPos.G : 0;
            this.G = G + parentG;

            int step = Math.Abs(this.X - _end.X) + Math.Abs(this.Y - _end.Y);
            this.H = step * _maze.weightLR;

            this.F = this.G + this.H;
        }
    }

    public class Test {
        public void Start () {
            int width = 20;
            int heigh = 12;
            Grid[][] grids = new Grid[heigh][];

            int[,] array = { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1 },
            { 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        };
            for (int i = 0 ; i < heigh ; i++) {
                grids[i] = new Grid[width];
                for (int j = 0 ; j < width ; j++) {
                    Grid grid = new Grid();
                    grid.X = i;
                    grid.Y = j;
                    grid.Gtype = (GridType)array[i , j];
                    grids[i][j] = grid;
                }
            }

            Maze ma = new Maze(grids);

            for (int i = 0 ; i < grids.Length ; i++) {
                for (int j = 0 ; j < grids[i].Length ; j++) {
                    GotoXY(i , j);
                    System.Console.WriteLine(grids[i][j]);
                }
            }

            Grid start = new Grid() { X = 1 , Y = 10 };
            Grid end = new Grid() { X = 4 , Y = 11 };

            GotoXY(start.X , start.Y);
            System.Console.WriteLine("-");
            GotoXY(end.X , end.Y);
            System.Console.WriteLine("-");
            start.FindPath(end , ma);
            while (start.nextPos != null) {
                start = start.nextPos;
                GotoXY(start.X , start.Y);
                System.Console.WriteLine("-");
                System.Threading.Thread.Sleep(500);
            }

            GotoXY(13 , 21);
            System.Console.WriteLine("");
        }
        public static void GotoXY (int x , int y) {
            Console.SetCursorPosition(y * 2 , x);
        }
    }
}