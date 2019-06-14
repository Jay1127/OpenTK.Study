using System;
using OpenTK;
using Toolkit;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Imaging = System.Drawing.Imaging;
using OpenTK.Input;

namespace BlinnPhong
{
    public class BlinnPhongScene : GameWindow
    {
        FPSCamera camera = new FPSCamera(new Vector3(3.0f, 3.0f, 3.0f));
        Shader shader;

        float[] planeVertices = {
            // positions            // normals         // texcoords
            10.0f, -0.5f,  10.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -10.0f, -0.5f,  10.0f,  0.0f, 1.0f, 0.0f,   0.0f,  0.0f,
            -10.0f, -0.5f, -10.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,

             10.0f, -0.5f,  10.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -10.0f, -0.5f, -10.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,
             10.0f, -0.5f, -10.0f,  0.0f, 1.0f, 0.0f,  10.0f, 10.0f
        };

        uint planeVAO;
        uint planeVBO;
        uint floorTexture;
        int blinn = 0;
        Vector3 lightPos = new Vector3();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            shader = new Shader(@"vertexShader.vs", @"fragmentShader.fs");
            shader.Create();

            // cube            
            GL.GenVertexArrays(1, out planeVAO);
            GL.BindVertexArray(planeVAO);

            GL.GenBuffers(1, out planeVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, planeVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * planeVertices.Length, planeVertices, BufferUsageHint.StaticDraw);

            // position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normal
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texture
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            floorTexture = LoadTexture(@"Resources\wood.png");

            shader.UseProgram();
            shader.SetInt("texture1", 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.UseProgram();

            var view = camera.ViewMatrix;
            var projection = Matrix4.CreatePerspectiveFieldOfView((float)(45.0f * Math.PI / 180),
                                                                  Width / Height, 0.1f, 100.0f);

            shader.SetMat4("view", view);
            shader.SetMat4("projection", projection);
            shader.SetVec3("viewPos", camera.Position);
            shader.SetVec3("lightPos", lightPos);
            shader.SetInt("blinn", blinn);

            GL.BindVertexArray(planeVAO);
            GL.BindTexture(TextureTarget.Texture2D, floorTexture);
            shader.SetMat4("model", Matrix4.Identity);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            SwapBuffers();
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
            else if(e.Key == Key.B)
            {
                if(blinn == 0)
                {
                    blinn = -1;
                }
                else
                {
                    blinn = 0;
                }
            }
        }
    }
}