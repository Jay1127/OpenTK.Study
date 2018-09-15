using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit
{
    public static class MathUtil
    {
        public static float ToRadian(float angle)
            => (float)(angle * Math.PI / 180);
    }
}
