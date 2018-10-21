using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLoader
{
    struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TexCoords { get; set; }

        public static int GetSize()
        {
            return Vector3.SizeInBytes * 2 + Vector2.SizeInBytes;
        }
    }
}
