using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using static System.Math;
using System.Windows.Interop;
using System.Windows;

namespace Laplace_WPF.Model
{
    public static class Images
    {
        public static Bitmap CreateImage(Data.Condition con, int dpi, double[,] data)
        {
            Bitmap image = new Bitmap(con.X_LMax * dpi, con.Y_LMax * dpi);

            for (int x = 0; x < con.X_LMax * dpi; x++)
            {
                for (int y = 0; y < con.Y_LMax * dpi; y++)
                {
                    var color = Color.FromArgb(GetColor(data[x, y]));
                    if (x >= con.X_Pos * dpi && y >= con.Y_Pos * dpi)
                        if (x <= (con.X_Pos + con.X_LRec) * dpi && y <= (con.Y_Pos + con.y_LRec) * dpi)
                        {
                            color = Color.White;
                        }
                    
                    image.SetPixel(x, y, color);
                }

            }
            return image;


        }
        public static BitmapSource GetBitmapSource(Bitmap bitmap)

        {

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap

            (

                bitmap.GetHbitmap(),

                IntPtr.Zero,

                Int32Rect.Empty,

                BitmapSizeOptions.FromEmptyOptions()

            );



            return bitmapSource;

        }

        public static int GetColor(double in_value)
        {
            // 0.0～1.0 の範囲の値をサーモグラフィみたいな色にする
            // 0.0                    1.0
            // 青    水    緑    黄    赤
            // 最小値以下 = 青
            // 最大値以上 = 赤
            int ret;
            int a = 255;    // alpha値
            int r, g, b;    // RGB値
            double value = in_value;
            double tmp_val = Cos(4 * PI * value);
            int col_val = (int)((-tmp_val / 2 + 0.5) * 255);
            if (value >= (4.0 / 4.0)) { r = 255; g = 0; b = 0; }   // 赤
            else if (value >= (3.0 / 4.0)) { r = 255; g = col_val; b = 0; }   // 黄～赤
            else if (value >= (2.0 / 4.0)) { r = col_val; g = 255; b = 0; }   // 緑～黄
            else if (value >= (1.0 / 4.0)) { r = 0; g = 255; b = col_val; }   // 水～緑
            else if (value >= (0.0 / 4.0)) { r = 0; g = col_val; b = 255; }   // 青～水
            else { r = 0; g = 0; b = 255; }   // 青
            ret = (a & 0x000000FF) << 24
                | (r & 0x000000FF) << 16
                | (g & 0x000000FF) << 8
                | (b & 0x000000FF);
            return ret;
        }

    }
}
