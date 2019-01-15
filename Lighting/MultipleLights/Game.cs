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
using Imaging = System.Drawing.Imaging;

namespace MultipleLights
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

        Vector3[] pointLightPositions =
        {
            new Vector3( 0.7f,  0.2f,  2.0f),
            new Vector3( 2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f,  2.0f, -12.0f),
            new Vector3( 0.0f,  0.0f, -3.0f)
        };

        FPSCamera camera = new FPSCamera(new Vector3(0.0f, 0.0f, 5.0f));

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

            modelShader = new Shader(@"multiple_light.vs", @"multiple_light.fs");
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

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

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

            // directional light
            modelShader.SetVec3("dirLight.direction", -0.2f, -1.0f, -0.3f);
            modelShader.SetVec3("dirLight.ambient", 0.05f, 0.05f, 0.05f);
            modelShader.SetVec3("dirLight.diffuse", 0.4f, 0.4f, 0.4f);
            modelShader.SetVec3("dirLight.specular", 0.5f, 0.5f, 0.5f);

            // point light 1
            modelShader.SetVec3("pointLights[0].position", pointLightPositions[0]);
            modelShader.SetVec3("pointLights[0].ambient", 0.05f, 0.05f, 0.05f);
            modelShader.SetVec3("pointLights[0].diffuse", 0.8f, 0.8f, 0.8f);
            modelShader.SetVec3("pointLights[0].specular", 1.0f, 1.0f, 1.0f);
            modelShader.SetFloat("pointLights[0].constant", 1.0f);
            modelShader.SetFloat("pointLights[0].linear", 0.09f);
            modelShader.SetFloat("pointLights[0].quadratic", 0.032f);
            // point light 2
            modelShader.SetVec3("pointLights[1].position", pointLightPositions[1]);
            modelShader.SetVec3("pointLights[1].ambient", 0.05f, 0.05f, 0.05f);
            modelShader.SetVec3("pointLights[1].diffuse", 0.8f, 0.8f, 0.8f);
            modelShader.SetVec3("pointLights[1].specular", 1.0f, 1.0f, 1.0f);
            modelShader.SetFloat("pointLights[1].constant", 1.0f);
            modelShader.SetFloat("pointLights[1].linear", 0.09f);
            modelShader.SetFloat("pointLights[1].quadratic", 0.032f);
            // point light 3
            modelShader.SetVec3("pointLights[2].position", pointLightPositions[2]);
            modelShader.SetVec3("pointLights[2].ambient", 0.05f, 0.05f, 0.05f);
            modelShader.SetVec3("pointLights[2].diffuse", 0.8f, 0.8f, 0.8f);
            modelShader.SetVec3("pointLights[2].specular", 1.0f, 1.0f, 1.0f);
            modelShader.SetFloat("pointLights[2].constant", 1.0f);
            modelShader.SetFloat("pointLights[2].linear", 0.09f);
            modelShader.SetFloat("pointLights[2].quadratic", 0.032f);
            // point light 4
            modelShader.SetVec3("pointLights[3].position", pointLightPositions[3]);
            modelShader.SetVec3("pointLights[3].ambient", 0.05f, 0.05f, 0.05f);
            modelShader.SetVec3("pointLights[3].diffuse", 0.8f, 0.8f, 0.8f);
            modelShader.SetVec3("pointLights[3].specular", 1.0f, 1.0f, 1.0f);
            modelShader.SetFloat("pointLights[3].constant", 1.0f);
            modelShader.SetFloat("pointLights[3].linear", 0.09f);
            modelShader.SetFloat("pointLights[3].quadratic", 0.032f);

            // spotLight
            modelShader.SetVec3("spotLight.position", camera.Position);
            modelShader.SetVec3("spotLight.direction", camera.Front);
            modelShader.SetVec3("spotLight.ambient", 0.0f, 0.0f, 0.0f);
            modelShader.SetVec3("spotLight.diffuse", 1.0f, 1.0f, 1.0f);
            modelShader.SetVec3("spotLight.specular", 1.0f, 1.0f, 1.0f);
            modelShader.SetFloat("spotLight.constant", 1.0f);
            modelShader.SetFloat("spotLight.linear", 0.09f);
            modelShader.SetFloat("spotLight.quadratic", 0.032f);
            modelShader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathUtil.ToRadian(12.5f)));
            modelShader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathUtil.ToRadian(15.0f)));

            modelShader.SetFloat("material.shininess", 32.0f);
            modelShader.SetVec3("viewPos", camera.Position);

            modelShader.SetMat4("view", camera.ViewMatrix);

            var projection = Matrix4.CreatePerspectiveFieldOfView((float)(45.0f * Math.PI / 180),
                                                                  Width / Height, 0.1f, 100.0f);

            modelShader.SetMat4("projection", projection);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, diffuseMap);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, specularMap);

            GL.BindVertexArray(cubeVAO);

            // draw container box
            for (int i = 0; i < cubePositions.Length; i++)
            {
                float angle = 20.0f * i;

                var translation = Matrix4.CreateTranslation(cubePositions[i]);
                var rotation = Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), MathUtil.ToRadian(angle));
                var model = rotation * translation;

                modelShader.SetMat4("model", model);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            // draw point light
            lampShader.UseProgram();
            lampShader.SetMat4("projection", projection);
            lampShader.SetMat4("view", camera.ViewMatrix);
            for (int i = 0; i < pointLightPositions.Length; i++)
            {
                var lampModel = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(pointLightPositions[i]);
                lampShader.SetMat4("model", lampModel);

                GL.BindVertexArray(lightVAO);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

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
