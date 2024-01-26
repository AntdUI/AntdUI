// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;

namespace AntdUI
{
    public struct Vector
    {
        double _x, _y;

        public Vector(double x, double y)
        {
            _x = x; _y = y;
        }
        public Vector(PointF pt)
        {
            _x = pt.X;
            _y = pt.Y;
        }
        public Vector(PointF st, PointF end)
        {
            _x = end.X - st.X;
            _y = end.Y - st.Y;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.X, -v.Y);
        }

        public static Vector operator *(double c, Vector v)
        {
            return new Vector(c * v.X, c * v.Y);
        }

        public static Vector operator *(Vector v, double c)
        {
            return new Vector(c * v.X, c * v.Y);
        }

        public static Vector operator /(Vector v, double c)
        {
            return new Vector(v.X / c, v.Y / c);
        }

        public double CrossProduct(Vector v)
        {
            return _x * v.Y - v.X * _y;
        }

        public double DotProduct(Vector v)
        {
            return _x * v.X + _y * v.Y;
        }

        public static bool IsClockwise(PointF pt1, PointF pt2, PointF pt3)
        {
            Vector v1 = new Vector(pt2, pt1), v2 = new Vector(pt2, pt3);
            return v1.CrossProduct(v2) < 0;
        }

        public static bool IsCCW(PointF pt1, PointF pt2, PointF pt3)
        {
            Vector v1 = new Vector(pt2, pt1), v2 = new Vector(pt2, pt3);
            return v1.CrossProduct(v2) > 0;
        }

        public static double DistancePointLine(PointF pt, PointF lnA, PointF lnB)
        {
            Vector v1 = new Vector(lnA, lnB), v2 = new Vector(lnA, pt);
            v1 /= v1.Magnitude;
            return Math.Abs(v2.CrossProduct(v1));
        }

        public void Rotate(int degree)
        {
            double radian = degree * Math.PI / 180.0;
            double sin = Math.Sin(radian), cos = Math.Cos(radian);
            double nx = _x * cos - _y * sin, ny = _x * sin + _y * cos;
            _x = nx;
            _y = ny;
        }

        public PointF ToPointF()
        {
            return new PointF((float)_x, (float)_y);
        }
    }
}