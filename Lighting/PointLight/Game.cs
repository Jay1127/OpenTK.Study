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

namespace PointLight
{
    public class Game : GameWindow
    {
        float[] vertices =
        {
            // positions          // normals           // texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f
        };

        Vector3[] cubePositions =
        {
            new Vector3( 0.0f,  0.0f,  0.0f),
            new Vector3( 2.0f,  5.0f, -15.0f),
            new Vector3(-1.5f, -2.2f, -2.5f),
            new Vector3(-3.8f, -2.0f, -12.3f),
            new Vector3( 2.4f, -0.4f, -3.5f),
            new Vector3(-1.7f,  3.0f, -7.5f),
            new Vector3( 1.3f, -2.0f, -2.5f),
            new Vector3( 1.5f,  2.0f, -2.5f),
            new Vector3( 1.5f,  0.2f, -1.5f),
            new Vector3(-1.3f,  1.0f, -1.5f)
        };

        Vector3 viewerPos = new Vector3(-3.0f, -2.0f, 10.0f);
        Vector3 lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        int VBO;
        int cubeVAO;
        int lightVAO;
        uint diffuseMap;
        uint specularMap;
        Shader modelShader;
        Shader lampShader;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            modelShader = new Shader(@"point_light.vs", @"point_light.fs");
            modelShader.Create();

            lampShader = new Shader(@"lampShader.vs", @"lampShader.fs");
            lampShader.Create();

            // cube            
            GL.GenVertexArrays(1, out cubeVAO);
            GL.BindVertexArray(cubeVAO);

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

            // lamp
            GL.GenVertexArrays(1, out lightVAO);
            GL.BindVertexArray(lightVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // texture load
            diffuseMap = LoadTexture(@"Resources\container2.png");
            specularMap = LoadTexture(@"Resources\container2_specular.png");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.LineSmooth);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            modelShader.UseProgram();

            modelShader.SetFloat("material.shininess", 32.0f);

            modelShader.SetVec3("light.position", lightPos);
            modelShader.SetVec3("light.ambient", 0.2f, 0.2f, 0.2f);
            modelShader.SetVec3("light.diffuse", 0.5f, 0.5f, 0.5f);
            modelShader.SetVec3("light.specular", 1.0f, 1.0f, 1.0f);
            modelShader.SetFloat("light.constant", 1.0f);
            modelShader.SetFloat("light.linear", 0.09f);
            modelShader.SetFloat("light.quadratic", 0.032f);

            modelShader.SetVec3("viewPos", viewerPos);

            var view = Matrix4.LookAt(viewerPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelShader.SetMat4("view", view);

            var projection = Matrix4.CreatePerspectiveFieldOfView((float)(45.0f * Math.PI / 180),
                                                                  Width / Height, 0.1f, 100.0f);

            modelShader.SetMat4("projection", projection);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, diffuseMap);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, specularMap);

            GL.BindVertexArray(cubeVAO);

            for (int i = 0; i < 10; i++)
            {
                float angle = 20.0f * i;

                var translation = Matrix4.CreateTranslation(cubePositions[i]);
                var rotation = Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), MathUtil.ToRadian(angle));
                var model = rotation * translation;

                modelShader.SetMat4("model", model);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            lampShader.UseProgram();
            lampShader.SetMat4("projection", projection);
            lampShader.SetMat4("view", view);

            var lampModel = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(lightPos);
            lampShader.SetMat4("model", lampModel);

            GL.BindVertexArray(lightVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();
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

            // light position
            if (e.Key == Key.W)
            {
                lightPos.Y++;
            }
            else if (e.Key == Key.S)
            {
                lightPos.Y--;
            }

            else if (e.Key == Key.A)
            {
                lightPos.X--;
            }

            else if (e.Key == Key.D)
            {
                lightPos.X++;
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
