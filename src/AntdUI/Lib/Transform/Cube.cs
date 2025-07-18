// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class Cube
    {
        public int width = 0;
        public int height = 0;
        public int depth = 0;
        double xRotation = 0.0;
        double yRotation = 0.0;
        double zRotation = 0.0;
        Math3D.Camera camera1 = new Math3D.Camera();
        Math3D.Point3D cubeOrigin;
        public Point a, b, c, d;

        public double RotateX
        {
            get { return xRotation; }
            set { xRotation = value; }
        }

        public double RotateY
        {
            get { return yRotation; }
            set { yRotation = value; }
        }

        public double RotateZ
        {
            get { return zRotation; }
            set { zRotation = value; }
        }

        public Cube(int Width, int Height, int Depth)
        {
            width = Width;
            height = Height;
            depth = Depth;
            cubeOrigin = new Math3D.Point3D(width / 2, height / 2, depth / 2);
        }

        public static Rectangle getBounds(PointF[] points)
        {
            double left = points[0].X;
            double right = points[0].X;
            double top = points[0].Y;
            double bottom = points[0].Y;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].X < left)
                    left = points[i].X;
                if (points[i].X > right)
                    right = points[i].X;
                if (points[i].Y < top)
                    top = points[i].Y;
                if (points[i].Y > bottom)
                    bottom = points[i].Y;
            }
            return new Rectangle(0, 0, (int)Math.Round(right - left), (int)Math.Round(bottom - top));
        }

        public void calcCube(Point drawOrigin)
        {
            var point3D = new PointF[24];
            var tmpOrigin = new Point(0, 0);
            var point0 = new Math3D.Point3D(0, 0, 0);
            double zoom = Screen.PrimaryScreen.Bounds.Width / 1.5D;
            var cubePoints = fillCubeVertices(width, height, depth);
            var anchorPoint = cubePoints[4];
            double cameraZ = -(((anchorPoint.X - cubeOrigin.X) * zoom) / cubeOrigin.X) + anchorPoint.Z;
            camera1.Position = new Math3D.Point3D(cubeOrigin.X, cubeOrigin.Y, cameraZ);
            cubePoints = Math3D.Translate(cubePoints, cubeOrigin, point0);
            cubePoints = Math3D.RotateX(cubePoints, xRotation);
            cubePoints = Math3D.RotateY(cubePoints, yRotation);
            cubePoints = Math3D.RotateZ(cubePoints, zRotation);
            cubePoints = Math3D.Translate(cubePoints, point0, cubeOrigin);
            Math3D.Point3D vec;
            for (int i = 0; i < point3D.Length; i++)
            {
                vec = cubePoints[i];
                if (vec.Z - camera1.Position.Z >= 0)
                {
                    point3D[i].X = (int)((double)-(vec.X - camera1.Position.X) / (-0.1F) * zoom) + drawOrigin.X;
                    point3D[i].Y = (int)((double)(vec.Y - camera1.Position.Y) / (-0.1F) * zoom) + drawOrigin.Y;
                }
                else
                {
                    tmpOrigin.X = (int)((double)(cubeOrigin.X - camera1.Position.X) / (double)(cubeOrigin.Z - camera1.Position.Z) * zoom) + drawOrigin.X;
                    tmpOrigin.Y = (int)((double)-(cubeOrigin.Y - camera1.Position.Y) / (double)(cubeOrigin.Z - camera1.Position.Z) * zoom) + drawOrigin.Y;

                    point3D[i].X = (float)((vec.X - camera1.Position.X) / (vec.Z - camera1.Position.Z) * zoom + drawOrigin.X);
                    point3D[i].Y = (float)(-(vec.Y - camera1.Position.Y) / (vec.Z - camera1.Position.Z) * zoom + drawOrigin.Y);

                    point3D[i].X = (int)point3D[i].X;
                    point3D[i].Y = (int)point3D[i].Y;
                }
            }
            a = Point.Round(point3D[4]);
            b = Point.Round(point3D[5]);
            c = Point.Round(point3D[6]);
            d = Point.Round(point3D[7]);
        }

        public PointF Centre()
        {
            return new PointF(CX, CY);
        }
        public PointF CentreY()
        {
            return new PointF(d.X + width / 2F, CY);
        }
        public PointF CentreX()
        {
            return new PointF(CX, d.Y + height / 2F);
        }

        public float CY
        {
            get
            {
                if (a.Y < d.Y) return a.Y + height / 2F;
                else return d.Y + height / 2F;
            }
        }
        public float CX
        {
            get
            {
                if (c.X < d.X) return c.X + width / 2F;
                else return d.X + width / 2F;
            }
        }

        public static Math3D.Point3D[] fillCubeVertices(int width, int height, int depth)
        {
            return new Math3D.Point3D[]{
                //front face
                new Math3D.Point3D(0, 0, 0),
                new Math3D.Point3D(0, height, 0),
                new Math3D.Point3D(width, height, 0),
                new Math3D.Point3D(width, 0, 0),
                
                //back face
                new Math3D.Point3D(0, 0, depth),
                new Math3D.Point3D(0, height, depth),
                new Math3D.Point3D(width, height, depth),
                new Math3D.Point3D(width, 0, depth),

                //left face
                new Math3D.Point3D(0, 0, 0),
                new Math3D.Point3D(0, 0, depth),
                new Math3D.Point3D(0, height, depth),
                new Math3D.Point3D(0, height, 0),

                //right face
                new Math3D.Point3D(width, 0, 0),
                new Math3D.Point3D(width, 0, depth),
                new Math3D.Point3D(width, height, depth),
                new Math3D.Point3D(width, height, 0),

                //top face
                new Math3D.Point3D(0, height, 0),
                new Math3D.Point3D(0, height, depth),
                new Math3D.Point3D(width, height, depth),
                new Math3D.Point3D(width, height, 0),

                //bottom face
                new Math3D.Point3D(0, 0, 0),
                new Math3D.Point3D(0, 0, depth),
                new Math3D.Point3D(width, 0, depth),
                new Math3D.Point3D(width, 0, 0),
            };
        }

        public Bitmap ToBitmap(Bitmap bmp)
        {
            var vertex = new PointF[] { d, a, b, c };

            using (var srcCB = new ImageData())
            {
                srcCB.FromBitmap(bmp);

                int srcH = bmp.Height, srcW = bmp.Width;

                #region setVertex

                float xmin = float.MaxValue;
                float ymin = xmin;
                float xmax = float.MinValue;
                float ymax = xmax;
                for (int i = 0; i < 4; i++)
                {
                    xmax = Math.Max(xmax, vertex[i].X);
                    ymax = Math.Max(ymax, vertex[i].Y);
                    xmin = Math.Min(xmin, vertex[i].X);
                    ymin = Math.Min(ymin, vertex[i].Y);
                }
                var rect = new Rectangle((int)xmin, (int)ymin, (int)(xmax - xmin), (int)(ymax - ymin));

                Vector AB = new Vector(vertex[0], vertex[1]), BC = new Vector(vertex[1], vertex[2]), CD = new Vector(vertex[2], vertex[3]), DA = new Vector(vertex[3], vertex[0]);
                AB /= AB.Magnitude;
                BC /= BC.Magnitude;
                CD /= CD.Magnitude;
                DA /= DA.Magnitude;

                #endregion

                #region 返回

                using (var destCB = new ImageData())
                {
                    destCB.A = new byte[rect.Width, rect.Height];
                    destCB.B = new byte[rect.Width, rect.Height];
                    destCB.G = new byte[rect.Width, rect.Height];
                    destCB.R = new byte[rect.Width, rect.Height];
                    var ptInPlane = new PointF();
                    int x1, x2, y1, y2;
                    double dab, dbc, dcd, dda;
                    float dx1, dx2, dy1, dy2, dx1y1, dx1y2, dx2y1, dx2y2, nbyte;

                    int y = 0, x;
                    while (++y < rect.Height)
                    {
                        x = 0;
                        while (++x < rect.Width)
                        {
                            var srcPt = new Point(x, y);
                            srcPt.Offset(rect.X, rect.Y);

                            if ((!Vector.IsCCW(srcPt, vertex[0], vertex[1])) && (!Vector.IsCCW(srcPt, vertex[1], vertex[2])) && (!Vector.IsCCW(srcPt, vertex[2], vertex[3])) && (!Vector.IsCCW(srcPt, vertex[3], vertex[0])))
                            {
                                dab = Math.Abs((new Vector(vertex[0], srcPt)).CrossProduct(AB));
                                dbc = Math.Abs((new Vector(vertex[1], srcPt)).CrossProduct(BC));
                                dcd = Math.Abs((new Vector(vertex[2], srcPt)).CrossProduct(CD));
                                dda = Math.Abs((new Vector(vertex[3], srcPt)).CrossProduct(DA));
                                ptInPlane.X = (float)(srcW * (dda / (dda + dbc)));
                                ptInPlane.Y = (float)(srcH * (dab / (dab + dcd)));

                                x1 = (int)ptInPlane.X;
                                y1 = (int)ptInPlane.Y;

                                if (x1 >= 0 && x1 < srcW && y1 >= 0 && y1 < srcH)
                                {
                                    x2 = (x1 == srcW - 1) ? x1 : x1 + 1;
                                    y2 = (y1 == srcH - 1) ? y1 : y1 + 1;
                                    dx1 = ptInPlane.X - x1;
                                    if (dx1 < 0) dx1 = 0;
                                    dx1 = 1f - dx1;
                                    dx2 = 1f - dx1;
                                    dy1 = ptInPlane.Y - y1;
                                    if (dy1 < 0) dy1 = 0;
                                    dy1 = 1f - dy1;
                                    dy2 = 1f - dy1;
                                    dx1y1 = dx1 * dy1;
                                    dx1y2 = dx1 * dy2;
                                    dx2y1 = dx2 * dy1;
                                    dx2y2 = dx2 * dy2;
                                    nbyte = srcCB.A[x1, y1] * dx1y1 + srcCB.A[x2, y1] * dx2y1 + srcCB.A[x1, y2] * dx1y2 + srcCB.A[x2, y2] * dx2y2;
                                    destCB.A[x, y] = (byte)nbyte;
                                    nbyte = srcCB.B[x1, y1] * dx1y1 + srcCB.B[x2, y1] * dx2y1 + srcCB.B[x1, y2] * dx1y2 + srcCB.B[x2, y2] * dx2y2;
                                    destCB.B[x, y] = (byte)nbyte;
                                    nbyte = srcCB.G[x1, y1] * dx1y1 + srcCB.G[x2, y1] * dx2y1 + srcCB.G[x1, y2] * dx1y2 + srcCB.G[x2, y2] * dx2y2;
                                    destCB.G[x, y] = (byte)nbyte;
                                    nbyte = srcCB.R[x1, y1] * dx1y1 + srcCB.R[x2, y1] * dx2y1 + srcCB.R[x1, y2] * dx1y2 + srcCB.R[x2, y2] * dx2y2;
                                    destCB.R[x, y] = (byte)nbyte;
                                }
                            }
                        }
                    }
                    return destCB.ToBitmap();
                }
            }

            #endregion
        }
    }
}