using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit;

namespace Material
{
    class Game : GameWindow
    {
        float[] vertices = new float[]
        {
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

        uint VBO;
        uint modelVAO;
        uint lampVAO;
        Shader lampShader;
        Shader modelShader;

        Vector3 lightPos = new Vector3(2.0f, 3.0f, 5.0f);
        Vector3 viewerPos = new Vector3(-2.0f, 2.0f, -2.0f);

        Matrix4 model;
        Matrix4 view;
        Matrix4 projection;

        private const string uniformModel = "model";
        private const string uniformView = "view";
        private const string uniformProjection = "projection";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            modelShader = new Shader(@"materialShader.vs", @"materialShader.fs");
            modelShader.Create();

            lampShader = new Shader(@"lampShader.vs", @"lampShader.fs");
            lampShader.Create();

            // model
            GL.GenVertexArrays(1, out modelVAO);
            GL.BindVertexArray(modelVAO);

            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // lamp
            GL.GenVertexArrays(1, out lampVAO);
            GL.BindVertexArray(lampVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // projection
            float fov = (float)(45.0f * Math.PI / 180);
            float aspectRatio = Width / Height;
            projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100.0f);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.LineSmooth);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            modelShader.UseProgram();

            modelShader.SetVec3("material.ambient", 1.0f, 0.5f, 0.31f);
            modelShader.SetVec3("material.diffuse", 1.0f, 0.5f, 0.31f);
            modelShader.SetVec3("material.specular", 0.5f, 0.5f, 0.5f);
            modelShader.SetFloat("material.shininess", 32.0f);

            modelShader.SetVec3("light.ambient", 0.2f, 0.2f, 0.2f);
            modelShader.SetVec3("light.diffuse", 1.0f, 1.0f, 1.0f); 
            modelShader.SetVec3("light.specular", 0.5f, 0.5f, 0.5f);

            modelShader.SetVec3("viewPos", viewerPos);

            model = Matrix4.Identity;
            view = Matrix4.LookAt(new Vector3(-1.5f, 1.5f, 4f), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            modelShader.SetMat4(uniformModel, model);
            modelShader.SetMat4(uniformView, view);
            modelShader.SetMat4(uniformProjection, projection);

            GL.BindVertexArray(modelVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // light cube
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
