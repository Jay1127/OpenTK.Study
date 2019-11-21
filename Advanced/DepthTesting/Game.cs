using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using Imaging = System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit;
using OpenTK.Input;

namespace DepthTesting
{
    class Game : GameWindow
    {
        float[] cubeVertices = 
        {
            // positions          // texture Coords
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        float[] planeVertices =
        {
            // positions          // texture Coords (note we set these higher than 1 (together with GL_REPEAT as texture wrapping mode). this will cause the floor texture to repeat)
             5.0f, -0.5f,  5.0f,  2.0f, 0.0f,
            -5.0f, -0.5f,  5.0f,  0.0f, 0.0f,
            -5.0f, -0.5f, -5.0f,  0.0f, 2.0f,

             5.0f, -0.5f,  5.0f,  2.0f, 0.0f,
            -5.0f, -0.5f, -5.0f,  0.0f, 2.0f,
             5.0f, -0.5f, -5.0f,  2.0f, 2.0f
        };

        int cubeVAO;
        int cubeVBO;
        int planeVAO;
        int planeVBO;

        uint cubeTextureId;
        uint planeTextureId;

        Shader shader;
        FPSCamera camera = new FPSCamera(new Vector3(0, 0, 5));

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            shader = new Shader(@"vertexShader.vs", @"fragShader.fs");
            shader.Create();

            // cube            
            GL.GenVertexArrays(1, out cubeVAO);
            GL.BindVertexArray(cubeVAO);

            GL.GenBuffers(1, out cubeVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, cubeVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * cubeVertices.Length, cubeVertices, BufferUsageHint.StaticDraw);

            // position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // texture
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // floor            
            GL.GenVertexArrays(1, out planeVAO);
            GL.BindVertexArray(planeVAO);

            GL.GenBuffers(1, out planeVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, planeVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * planeVertices.Length, planeVertices, BufferUsageHint.StaticDraw);

            // position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // texture
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texture load
            cubeTextureId = LoadTexture(@"Resources\marble.jpg");
            planeTextureId = LoadTexture(@"Resources\metal.png");

            shader.UseProgram();
            shader.SetInt("texture1", 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //shader.UseProgram();

            var view = camera.ViewMatrix;
            var projection = Matrix4.CreatePerspectiveFieldOfView((float)(45.0f * Math.PI / 180),
                                                                  Width / Height, 0.1f, 100.0f);

            shader.SetMat4("view", view);
            shader.SetMat4("projection", projection);

            // cubes
            GL.BindVertexArray(cubeVAO);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, cubeTextureId);

            var model = Matrix4.CreateTranslation(-1.0f, 0.0f, -1.0f);
            shader.SetMat4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            model = Matrix4.CreateTranslation(2.0f, 0.0f, 0.0f);
            shader.SetMat4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // plane
            GL.BindVertexArray(planeVAO);
            GL.BindTexture(TextureTarget.Texture2D, planeTextureId);
            shader.SetMat4("model", Matrix4.Identity);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.BindVertexArray(0);

            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            // camera position
            if (e.Key == Key.Up)
            {
                camera.MoveForward();
            }
            else if (e.Key == Key.Down)
            {
                camera.MoveBackward();
            }
            else if (e.Key == Key.Left)
            {
                camera.MoveLeft();
            }
            else if (e.Key == Key.Right)
            {
                camera.MoveRight();
            }
        }

        private uint LoadTexture(string path)
        {
            GL.GenTextures(1, out uint textureId);

            Bitmap bitmap = new Bitmap(path);
            Imaging.BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                        Imaging.ImageLockMode.ReadOnly,
                                                        Imaging.PixelFormat.Format32bppRgb);

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          bitmap.Width, bitmap.Height, 0,
                          PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return textureId;
        }
    }
}
