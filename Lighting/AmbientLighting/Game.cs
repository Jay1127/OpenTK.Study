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
        uint lampVAO;
        uint modelVAO;

        Shader lampShader;
        Shader modelShader;

        Vector3 lightPos = new Vector3(1.2f, 3.0f, 2.0f);

        Matrix4 model;
        Matrix4 view;
        Matrix4 projection;

        private const string uniformModel = "model";
        private const string uniformView = "view";
        private const string uniformProjection = "projection";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            modelShader = new Shader(@"vertexShader.vs", @"ambientShader.fs");
            modelShader.Create();

            lampShader = new Shader(@"vertexShader.vs", @"fragShader.fs");
            lampShader.Create();

            // model
            GL.GenVertexArrays(1, out modelVAO);
            GL.BindVertexArray(modelVAO);

            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // lamp
            GL.GenVertexArrays(1, out lampVAO);
            GL.BindVertexArray(lampVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // projection
            float fov = (float)(45.0f * Math.PI / 180);
            float aspectRatio = Width / Height;
            projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.LineSmooth);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // model(cube) object
            modelShader.UseProgram();
            modelShader.SetVec3("objectColor", 1.0f, 0.5f, 0.31f);
            modelShader.SetVec3("lightColor", 1.0f, 1.0f, 1.0f);

            model = Matrix4.Identity;
            view = Matrix4.LookAt(new Vector3(-1.5f, 1.5f, -3f), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            modelShader.SetMat4(uniformModel, model);
            modelShader.SetMat4(uniformView, view);
            modelShader.SetMat4(uniformProjection, projection);

            GL.BindVertexArray(modelVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // lamp object
            lampShader.UseProgram();
            model *= Matrix4.CreateTranslation(lightPos);
            model *= Matrix4.CreateScale(0.3f);

            lampShader.SetMat4(uniformModel, model);
            lampShader.SetMat4(uniformView, view);
            lampShader.SetMat4(uniformProjection, projection);

            GL.BindVertexArray(lampVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();
        }
    }
}