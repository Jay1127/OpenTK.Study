using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLoader
{
    class Texture
    {
        public uint Id { get; set; }

        /// <summary>
        /// diffuse texture or specular texture
        /// </summary>
        public string Type { get; set; }

        public string Path { get; set; }
    }
}
