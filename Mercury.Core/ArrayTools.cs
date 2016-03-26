using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
    public static class ArrayTools
    {
        public static bool AreEqual(int[] a, int[] b)
        {
            if (a[0] != b[0]) return false;
            if (a[1] != b[1]) return false;
            return true;
        }
    }
}
