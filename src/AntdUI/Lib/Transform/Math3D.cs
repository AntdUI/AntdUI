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

namespace AntdUI
{
    public class Math3D
    {
        public class Point3D
        {
            public double X;
            public double Y;
            public double Z;

            public Point3D(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = 1; //no Depth
            }

            public Point3D(float x, float y, float z)
            {
                X = (double)x;
                Y = (double)y;
                Z = (double)z;
            }

            public Point3D(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public Point3D()
            {
            }

            public override string ToString()
            {
                return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
            }
        }

        public class Camera
        {
            public Point3D Position = new Point3D();
        }

        public static Point3D RotateX(Point3D point3D, double degrees)
        {
            double cDegrees = (Math.PI * degrees) / 180.0f;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);
            double y = (point3D.Y * cosDegrees) + (point3D.Z * sinDegrees);
            double z = (point3D.Y * -sinDegrees) + (point3D.Z * cosDegrees);
            return new Point3D(point3D.X, y, z);
        }

        public static Point3D RotateY(Point3D point3D, double degrees)
        {
            double cDegrees = (Math.PI * degrees) / 180.0;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);
            double x = (point3D.X * cosDegrees) + (point3D.Z * sinDegrees);
            double z = (point3D.X * -sinDegrees) + (point3D.Z * cosDegrees);
            return new Point3D(x, point3D.Y, z);
        }

        public static Point3D RotateZ(Point3D point3D, double degrees)
        {
            double cDegrees = (Math.PI * degrees) / 180.0;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);
            double x = (point3D.X * cosDegrees) + (point3D.Y * sinDegrees);
            double y = (point3D.X * -sinDegrees) + (point3D.Y * cosDegrees);
            return new Point3D(x, y, point3D.Z);
        }

        public static Point3D Translate(Point3D points3D, Point3D oldOrigin, Point3D newOrigin)
        {
            Point3D difference = new Point3D(newOrigin.X - oldOrigin.X, newOrigin.Y - oldOrigin.Y, newOrigin.Z - oldOrigin.Z);
            points3D.X += difference.X;
            points3D.Y += difference.Y;
            points3D.Z += difference.Z;
            return points3D;
        }

        public static Point3D[] RotateX(Point3D[] points3D, double degrees)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = RotateX(points3D[i], degrees);
            }
            return points3D;
        }

        public static Point3D[] RotateY(Point3D[] points3D, double degrees)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = RotateY(points3D[i], degrees);
            }
            return points3D;
        }

        public static Point3D[] RotateZ(Point3D[] points3D, double degrees)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = RotateZ(points3D[i], degrees);
            }
            return points3D;
        }

        public static Point3D[] Translate(Point3D[] points3D, Point3D oldOrigin, Point3D newOrigin)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = Translate(points3D[i], oldOrigin, newOrigin);
            }
            return points3D;
        }
    }
}