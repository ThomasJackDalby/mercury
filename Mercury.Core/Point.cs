using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
    public static class Point
    {
        public static string GetString(int[] point)
        {
            return String.Format("[{0}, {1}]", point[0], point[1]);
        }
    }
}
