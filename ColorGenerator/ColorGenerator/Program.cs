using System.Drawing;
using Console = Colorful.Console;

namespace ColorGenerator
{
    public class ColorConvert
    {
        public static readonly string RESET = "\u001b[0m";

        public static Tuple<int, int, int> HexToRgb(string hexStr)
        {
            int r = Convert.ToInt32(hexStr.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexStr.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexStr.Substring(4, 2), 16);
            return new Tuple<int, int, int>(r, g, b);
        }

        public static string RgbToHex(Tuple<int, int, int> rgb)
        {
            return $"{rgb.Item1:x2}{rgb.Item2:x2}{rgb.Item3:x2}";
        }

        public static string GetColorEscape(Tuple<int, int, int> dec, bool background = false)
        {
            int r = dec.Item1;
            int g = dec.Item2;
            int b = dec.Item3;
            return $"\u001b[{(background ? 48 : 38)};2;{r};{g};{b}m";
        }

        public static Tuple<double, double, double> RgbToHsv(Tuple<int, int, int> rgb)
        {
            int red = rgb.Item1;
            int green = rgb.Item2;
            int blue = rgb.Item3;

            int maxColor = Math.Max(Math.Max(red, green), blue);
            int minColor = Math.Min(Math.Min(red, green), blue);

            double hue, saturation, value;

            if (red == green && green == blue)
            {
                hue = 0;
            }
            else if (maxColor == red)
            {
                hue = (green - blue) / (double)(maxColor - minColor);
            }
            else if (maxColor == green)
            {
                hue = 2.0 + (blue - red) / (double)(maxColor - minColor);
            }
            else
            {
                hue = 4.0 + (red - green) / (double)(maxColor - minColor);
            }

            hue *= 60;

            if (hue < 0)
            {
                hue += 360;
            }

            hue = Math.Round(hue, 2);
            value = Math.Round(maxColor / 255.0, 2);

            if (maxColor > 0)
            {
                saturation = Math.Round((1 - minColor / (double)maxColor), 2);
            }
            else
            {
                saturation = 0;
            }

            return new Tuple<double, double, double>(hue, saturation, value);
        }

        public static Tuple<int, int, int> HsvToRgb(Tuple<double, double, double> hsv)
        {
            double h = hsv.Item1;
            double s = hsv.Item2;
            double v = hsv.Item3;

            int M = (int)Math.Round(255 * v);
            int m = (int)Math.Round(M * (1 - s));

            double z = (M - m) * (1 - Math.Abs((h / 60) % 2 - 1));
            int r, g, b;

            if (0 <= h && h < 60)
            {
                r = M;
                g = (int)Math.Round(z + m);
                b = m;
            }
            else if (60 <= h && h < 120)
            {
                r = (int)Math.Round(z + m);
                g = M;
                b = m;
            }
            else if (120 <= h && h < 180)
            {
                r = m;
                g = M;
                b = (int)Math.Round(z + m);
            }
            else if (180 <= h && h < 240)
            {
                r = m;
                g = (int)Math.Round(z + m);
                b = M;
            }
            else if (240 <= h && h < 300)
            {
                r = (int)Math.Round(z + m);
                g = m;
                b = M;
            }
            else // 300 <= h < 360
            {
                r = M;
                g = m;
                b = (int)Math.Round(z + m);
            }

            return new Tuple<int, int, int>(r, g, b);
        }

        public static (Tuple<double, double, double>[], Tuple<int, int, int>[]) GetAccents(Tuple<double, double, double> baseHsv)
        {
            var hsvColors = new Tuple<double, double, double>[9];

            hsvColors[4] = baseHsv;

            for (int i = 0; i < 9; i++)
            {
                if (i < 5)
                {
                    if (hsvColors[i] == null)
                    {
                        var sat = 0.2 + (Math.Abs(baseHsv.Item2 - 0.2) / 5) * (i + 1);
                        var val = 1 - (Math.Abs(baseHsv.Item3 - 1) / 5) * (i + 1);
                        var rgb = HsvToRgb(new Tuple<double, double, double>(baseHsv.Item1, sat, val));

                        hsvColors[i] = new (baseHsv.Item1, sat, val);
                    }
                }
                else
                {
                    if (hsvColors[^(i - 4)] == null)
                    {
                        hsvColors[^(i - 4)] = new (baseHsv.Item1, 1 - (Math.Abs(baseHsv.Item2 - 1) / 4) * Math.Abs(i - 5), 0.4 + (Math.Abs(baseHsv.Item3 - 0.4) / 4) * (i - 5));
                    }
                }
            }

            var rgbColors = new List<Tuple<int, int, int>>();

            foreach (var hsvT in hsvColors)
            {
                if (hsvT != null)
                {
                    var rgbT = HsvToRgb(new (hsvT.Item1, hsvT.Item2, hsvT.Item3));
                    rgbColors.Add(rgbT);
                }
            }

            return (hsvColors, rgbColors.ToArray());
        }
        public static (Tuple<double, double, double>[], Tuple<int, int, int>[]) ModifyAngle(List<double> baseColor, double angle)
        {
            var newHsv = new Tuple<double, double, double>(angle, baseColor[1], baseColor[2]);
            return GetAccents(newHsv);
        }

        public static void PrintColors(Tuple<int, int, int>[] rgbArray)
        {
            //var rgbList = new Tuple<int, int, int>[hsvList.Count];
            //for (int i = 0; i < hsvList.Count; i++)
            //{
            //    rgbList[i] = HsvToRgb(new (hsvList[i].Item1, hsvList[i].Item2, hsvList[i].Item3));
            //}

            for (int index = 0; index < rgbArray.Length; index++)
            {
                var color = rgbArray[index];
                //string hexString = $"{color.Item1:X2}{color.Item2:X2}{color.Item3:X2}";
                //string colorEscape = GetColorEscape(color, true);
                Console.WriteLine("████████", Color.FromArgb(color.Item1, color.Item2, color.Item3));
                //Console.WriteLine($"{colorEscape}           {RESET}({hsvList[index].Item1}, {hsvList[index].Item2}, {hsvList[index].Item3}) => ({hexString})");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var color = "d13bac";
            var rgb = ColorConvert.HexToRgb(color);
            var accent = ColorConvert.GetAccents(ColorConvert.RgbToHsv(rgb));
            ColorConvert.PrintColors(accent.Item2);
            Console.WriteLine($"RGB: {rgb.Item1}, {rgb.Item2}, {rgb.Item3}", Color.FromArgb(rgb.Item1, rgb.Item2, rgb.Item3));
        }
    }
}