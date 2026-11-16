// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Drawing.Imaging;
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
            double zoom = Screen.PrimaryScreen!.Bounds.Width / 1.5D;
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
            using (var src = BgraImage.FromBitmap(bmp))
            {
                return ToBitmap(src);
            }
        }

        /// <summary>
        /// 快速透视变换：源像素以交错 BGRA 整段布局采样（单缓存流），点到四条边的带符号距离用行内 DDA 增量计算
        /// </summary>
        internal unsafe Bitmap ToBitmap(BgraImage srcBmp)
        {
            var vertex = new PointF[4] { d, a, b, c };
            int srcW = srcBmp.Width, srcH = srcBmp.Height;

            #region 投影包围盒（并补上原先被跳过的第 0 行/列）

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
            int rx = (int)xmin, ry = (int)ymin;
            int rw = (int)(xmax - xmin) + 1, rh = (int)(ymax - ymin) + 1;
            if (rw <= 0 || rh <= 0 || srcW <= 0 || srcH <= 0) return new Bitmap(1, 1);

            #endregion

            #region 边距离函数（凸四边形内部四个带符号距离同号，其比值与绝对距离的比值一致）

            // 边 P1->P2（单位方向 ex, ey）：s(x, y) = ey * x - ex * y + (ex * P1.Y - ey * P1.X)
            EdgeOf(vertex[0], vertex[1], out double abDx, out double abDy, out double abC);
            EdgeOf(vertex[1], vertex[2], out double bcDx, out double bcDy, out double bcC);
            EdgeOf(vertex[2], vertex[3], out double cdDx, out double cdDy, out double cdC);
            EdgeOf(vertex[3], vertex[0], out double daDx, out double daDy, out double daC);

            #endregion

            var bmp = new Bitmap(rw, rh, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, rw, rh), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                int stride = bmpData.Stride;
                byte[] src = srcBmp.Pixels;
                fixed (byte* srcPtr = src)
                {
                    byte* dst = (byte*)bmpData.Scan0;
                    for (int y = 0; y < rh; y++)
                    {
                        double py = ry + y;
                        // 行起点距离，随后逐像素做 DDA 增量（每像素仅 4 次加法）
                        double sAB = abDx * rx + abDy * py + abC;
                        double sBC = bcDx * rx + bcDy * py + bcC;
                        double sCD = cdDx * rx + cdDy * py + cdC;
                        double sDA = daDx * rx + daDy * py + daC;
                        byte* row = dst + y * stride;
                        for (int x = 0; x < rw; x++)
                        {
                            bool pos = sAB > 0;
                            if (pos == (sBC > 0) && pos == (sCD > 0) && pos == (sDA > 0))
                            {
                                double uDen = sDA + sBC;
                                double vDen = sAB + sCD;
                                if (uDen != 0 && vDen != 0)
                                {
                                    float ppx = (float)(srcW * (sDA / uDen));
                                    float ppy = (float)(srcH * (sAB / vDen));
                                    int x1 = (int)ppx;
                                    if (x1 < 0) x1 = 0;
                                    else if (x1 > srcW - 1) x1 = srcW - 1;
                                    int x2 = x1 + 1;
                                    if (x2 > srcW - 1) x2 = srcW - 1;
                                    int y1 = (int)ppy;
                                    if (y1 < 0) y1 = 0;
                                    else if (y1 > srcH - 1) y1 = srcH - 1;
                                    int y2 = y1 + 1;
                                    if (y2 > srcH - 1) y2 = srcH - 1;
                                    float wx = ppx - x1;
                                    if (wx < 0) wx = 0;
                                    else if (wx > 1) wx = 1;
                                    float wy = ppy - y1;
                                    if (wy < 0) wy = 0;
                                    else if (wy > 1) wy = 1;
                                    float w1x = 1f - wx, w1y = 1f - wy;
                                    float w00 = w1x * w1y, w10 = wx * w1y, w01 = w1x * wy, w11 = wx * wy;
                                    byte* p00 = srcPtr + ((y1 * srcW + x1) << 2);
                                    byte* p10 = srcPtr + ((y1 * srcW + x2) << 2);
                                    byte* p01 = srcPtr + ((y2 * srcW + x1) << 2);
                                    byte* p11 = srcPtr + ((y2 * srcW + x2) << 2);
                                    row[0] = (byte)(p00[0] * w00 + p10[0] * w10 + p01[0] * w01 + p11[0] * w11);
                                    row[1] = (byte)(p00[1] * w00 + p10[1] * w10 + p01[1] * w01 + p11[1] * w11);
                                    row[2] = (byte)(p00[2] * w00 + p10[2] * w10 + p01[2] * w01 + p11[2] * w11);
                                    row[3] = (byte)(p00[3] * w00 + p10[3] * w10 + p01[3] * w01 + p11[3] * w11);
                                }
                            }
                            sAB += abDx; sBC += bcDx; sCD += cdDx; sDA += daDx;
                            row += 4;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }
            return bmp;
        }

        static void EdgeOf(PointF p1, PointF p2, out double dx, out double dy, out double c)
        {
            double ex = p2.X - p1.X, ey = p2.Y - p1.Y;
            double len = Math.Sqrt(ex * ex + ey * ey);
            if (len <= 0)
            {
                dx = 0; dy = 0; c = 0;
                return;
            }
            ex /= len;
            ey /= len;
            dx = ey;
            dy = -ex;
            c = ex * p1.Y - ey * p1.X;
        }
    }
}