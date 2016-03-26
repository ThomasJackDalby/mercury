using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
    public static class Tools
    {
        public static List<double> HSVtoRGB(List<double> hsv)
        {
            int i;
            double f, p, q, t;
            double red, green, blue;
            double hue = hsv[0];
            double saturation = hsv[1];
            double value = hsv[2];
            if (saturation == 0) // Grey
            {

                red = green = blue = value;
                return new List<double>() { red, green, blue };
            }

            hue

            /= 60;
            i

            = (int)Math.Floor(hue);
            f

            = hue - i;
            p

            = value * (1 - saturation);
            q

            = value * (1 - saturation * f);
            t

            = value * (1 - saturation * (1 - f));
            switch (i)
            {

                case 0:
                    red = value;
                    green = t;
                    blue = p;
                    break;
                case 1:
                    red = q;
                    green = value;
                    blue = p;
                    break;
                case 2:
                    red = p;
                    green = value;
                    blue = t;
                    break;
                case 3:
                    red = p;
                    green = q;
                    blue = value;
                    break;
                case 4:
                    red = t;
                    green = p;
                    blue = value;
                    break;
                default:
                    red = value;
                    green = p;
                    blue = q;
                    break;
            }

            return new List<double>() { red, green, blue };
        }
        public static int Max(params int[] nums)
        {

            int i = 0;
            for (int j = 0; j < nums.Length; j++) if (nums[j] > nums[i]) i = j;
            return i;
        }
        public static int Min(params int[] nums)
        {

            int i = 0;
            for (int j = 0; j < nums.Length; j++) if (nums[j] < nums[i]) i = j;
            return i;
        }
    }
}
