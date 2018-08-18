using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.IO;

namespace DrawRectangleByEBO
{
    class Game : GameWindow
    {
        float[] vertices = new float[]
        {
            0.5f, 0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            -0.5f, 0.5f, 0.0f
        };

        uint[] indices = new uint[]
        {
            0, 1, 3,
            1, 2, 3
        };

        uint VBO;
        uint VAO;
        int vertexShader;
        int fragShader;
        int success;
        int shaderProgram;

        uint EBO;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);

            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out EBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            vertexShader = GL.CreateShader(ShaderType.VertexShader);

            using (StreamReader sr = new StreamReader(@"vertexShader.c"))
            {
                GL.ShaderSource(vertexShader, sr.ReadToEnd());
            }

            GL.CompileShader(vertexShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
            if (success == -1)
            {
                GL.GetShaderInfoLog(vertexShader, out string infoLog);
                Debug.Print($"Vertex shader compliation failed : {infoLog}");
            }

            fragShader = GL.CreateShader(ShaderType.FragmentShader);

            using (StreamReader sr = new StreamReader(@"fragShader.c"))
            {
                GL.ShaderSource(fragShader, sr.ReadToEnd());
            }

            GL.CompileShader(fragShader);

            GL.GetShader(fragShader, ShaderParameter.CompileStatus, out success);
            if (success == -1)
            {
                GL.GetShaderInfoLog(fragShader, out string infoLog);
                Debug.Print($"fragment shader compliation failed : {infoLog}");
            }

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragShader);
            GL.LinkProgram(shaderProgram);

            GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out success);
            if (success == -1)
            {
                GL.GetProgramInfoLog(shaderProgram, out string infoLog);
                Debug.Print($"shader program is not linked : {infoLog}");
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragShader);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.UseProgram(shaderProgram);

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);

            SwapBuffers();
        }
    }
}
