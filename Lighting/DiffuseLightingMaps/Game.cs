using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.IO;
using Toolkit;
using Imaging = System.Drawing.Imaging;

namespace DiffuseLightingMaps
{
    class Game : GameWindow
    {
        private const string uniformModel = "model";
        private const string uniformView = "view";
        private const string uniformProjection = "projection";

        private float[] vertices = new float[]
        {
            // positions          // normals           // texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };

        private uint VBO;
        private uint modelVAO;
        private uint lampVAO;
        private uint texture;

        private Shader lampShader;
        private Shader modelShader;

        private Vector3 lightPos = new Vector3(2.4f, 2.0f, 4.0f);
        private Vector3 viewerPos = new Vector3(-3.0f, -2.0f, 3.0f);

        private Matrix4 model;
        private Matrix4 view;
        private Matrix4 projection;

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

            // position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // color
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texture
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bitmap = new Bitmap(@"Resources\container2.png");

            Imaging.BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Imaging.ImageLockMode.ReadOnly,
                                                        Imaging.PixelFormat.Format32bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          bitmap.Width, bitmap.Height, 0,
                          PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            // lamp
            GL.GenVertexArrays(1, out lampVAO);
            GL.BindVertexArray(lampVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
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

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
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

            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            modelShader.UseProgram();

            modelShader.SetInt("material.diffuse", 0);
            modelShader.SetVec3("material.specular", 0.5f, 0.5f, 0.5f);
            modelShader.SetFloat("material.shininess", 32.0f);

            modelShader.SetVec3("light.position", lightPos);
            modelShader.SetVec3("light.ambient", 0.2f, 0.2f, 0.2f);
            modelShader.SetVec3("light.diffuse", 1.0f, 1.0f, 1.0f);
            modelShader.SetVec3("light.specular", 0.5f, 0.5f, 0.5f);

            modelShader.SetVec3("viewPos", viewerPos);

            model = Matrix4.Identity;
            view = Matrix4.LookAt(viewerPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            modelShader.SetMat4(uniformModel, model);
            modelShader.SetMat4(uniformView, view);
            modelShader.SetMat4(uniformProjection, projection);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);

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