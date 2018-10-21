using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit;

namespace ModelLoader
{
    class Scene : GameWindow
    {
        Shader modelShader;

        Model model;

        Vector3 viewerPos = new Vector3(10, 10, 10);

        Matrix4 matModel;
        Matrix4 matView;
        Matrix4 matProj;

        private const string uniformModel = "model";
        private const string uniformView = "view";
        private const string uniformProjection = "projection";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            modelShader = new Shader(@"modelShader.vs", @"modelShader.fs");
            modelShader.Create();

            model = new Model();
            model.LoadModel(@"Resources\nanosuit\nanosuit.obj");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // projection
            float fov = (float)(45.0f * Math.PI / 180);
            float aspectRatio = Width / Height;
            matProj = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100.0f);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            // camera position
            if (e.Key == Key.Up)
            {
                viewerPos.Y++;
            }
            else if (e.Key == Key.Down)
            {
                viewerPos.Y--;
            }

            else if (e.Key == Key.Left)
            {
                viewerPos.X--;
            }

            else if (e.Key == Key.Right)
            {
                viewerPos.X++;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.LineSmooth);

            GL.ClearColor(0.05f, 0.05f, 0.05f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            modelShader.UseProgram();

            matModel = Matrix4.Identity;
            matView = Matrix4.LookAt(viewerPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            // model cube
            modelShader.SetMat4(uniformModel, matModel);
            modelShader.SetMat4(uniformView, matView);
            modelShader.SetMat4(uniformProjection, matProj);
            model.Draw(modelShader);

            SwapBuffers();
        }
    }
}
