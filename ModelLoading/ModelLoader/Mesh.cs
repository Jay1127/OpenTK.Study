using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Toolkit;

namespace ModelLoader
{
    class Mesh
    {
        private uint VAO;
        private uint VBO;
        private uint EBO;

        public Vertex[] Vertices { get; }
        public uint[] Indices { get; }
        public Texture[] Textures { get; }

        public Mesh(Vertex[] vertices, uint[] indices, Texture[] textures)
        {
            this.Vertices = vertices;
            this.Indices = indices;
            this.Textures = textures;
            InitMesh();
        }

        public void InitMesh()
        {
            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * Vertex.GetSize(), Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            // vertex positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.GetSize(), 0);

            // vertex normals
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.GetSize(), Vector3.SizeInBytes);

            // vertex texture coords
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.GetSize(), Vector3.SizeInBytes * 2);

            GL.BindVertexArray(0);
        }

        public void Draw(Shader shader)
        {
            shader.UseProgram();

            uint diffuseNr = 1;
            uint specularNr = 1;
            for (uint i = 0; i < Textures.Length; i++)
            {
                GL.ActiveTexture((TextureUnit)((uint)TextureUnit.Texture0 + i));

                uint number = 0;
                string name = Textures[i].Type;
                if (name == "texture_diffuse")
                    number = diffuseNr++;
                else if (name == "texture_specular")
                    number = specularNr++;

                shader.SetFloat(name + number, i);
                GL.BindTexture(TextureTarget.Texture2D, Textures[i].Id);
            }

            // draw mesh
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            // 원상태복구
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
        }
    }
}
