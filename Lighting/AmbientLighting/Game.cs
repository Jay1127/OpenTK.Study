using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit;

namespace AmbientLigthing
{
    class Game : GameWindow
    {
        float[] vertices = new float[]
        {
              -0.5f, -0.5f, -0.5f,
               0.5f, -0.5f, -0.5f,
               0.5f,  0.5f, -0.5f,
               0.5f,  0.5f, -0.5f,
              -0.5f,  0.5f, -0.5f,
              -0.5f, -0.5f, -0.5f,

              -0.5f, -0.5f,  0.5f,
               0.5f, -0.5f,  0.5f,
               0.5f,  0.5f,  0.5f,
               0.5f,  0.5f,  0.5f,
              -0.5f,  0.5f,  0.5f,
              -0.5f, -0.5f,  0.5f,

              -0.5f,  0.5f,  0.5f,
              -0.5f,  0.5f, -0.5f,
              -0.5f, -0.5f, -0.5f,
              -0.5f, -0.5f, -0.5f,
              -0.5f, -0.5f,  0.5f,
              -0.5f,  0.5f,  0.5f,

               0.5f,  0.5f,  0.5f,
               0.5f,  0.5f, -0.5f,
               0.5f, -0.5f, -0.5f,
               0.5f, -0.5f, -0.5f,
               0.5f, -0.5f,  0.5f,
               0.5f,  0.5f,  0.5f,

              -0.5f, -0.5f, -0.5f,
               0.5f, -0.5f, -0.5f,
               0.5f, -0.5f,  0.5f,
               0.5f, -0.5f,  0.5f,
              -0.5f, -0.5f,  0.5f,
              -0.5f, -0.5f, -0.5f,

              -0.5f,  0.5f, -0.5f,
               0.5f,  0.5f, -0.5f,
               0.5f,  0.5f,  0.5f,
               0.5f,  0.5f,  0.5f,
              -0.5f,  0.5f,  0.5f,
              -0.5f,  0.5f, -0.5f,
        };

        uint VBO;
        uint cubeVAO;
        uint lightVAO;
        Shader shader;
        Shader lightShader;

        Matrix4 model;
        Matrix4 view;
        Matrix4 projection;
        float timeValue;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lightShader = new Shader(@"vertexShader.vs", @"lightShader.fs");
            lightShader.Create();

            shader = new Shader(@"vertexShader.vs", @"fragShader.fs");
            shader.Create();

            GL.GenVertexArrays(1, out cubeVAO);
            GL.BindVertexArray(cubeVAO);

            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.GenVertexArrays(1, out lightVAO);
            GL.BindVertexArray(lightVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            lightShader.UseProgram();
            lightShader.SetVec3("objectColor", 1.0f, 0.5f, 0.31f);
            lightShader.SetVec3("lightColor", 1.0f, 1.0f, 1.0f);

            model = Matrix4.CreateFromAxisAngle(new Vector3(0.5f, 1.0f, 0.0f), (float)(50.0f * Math.PI / 180));
            view = Matrix4.CreateTranslation(0.0f, 0.0f, -5f);

            float fov = (float)(45.0f * Math.PI / 180);
            float aspectRatio = Width / Height;
            projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100.0f);

            GL.UniformMatrix4(GL.GetUniformLocation(lightShader.Id, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(lightShader.Id, "view"), false, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(lightShader.Id, "projection"), false, ref projection);

            GL.BindVertexArray(lightVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            shader.UseProgram();
            model *= Matrix4.CreateTranslation(5, 0, 5);
            model *= Matrix4.CreateScale(0.2f);
            GL.UniformMatrix4(GL.GetUniformLocation(shader.Id, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(shader.Id, "view"), false, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(shader.Id, "projection"), false, ref projection);

            GL.BindVertexArray(cubeVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }
    }
}
