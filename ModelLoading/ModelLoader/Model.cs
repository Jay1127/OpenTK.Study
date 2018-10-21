using Assimp;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Toolkit;
using Imaging = System.Drawing.Imaging;

namespace ModelLoader
{
    class Model
    {
        List<Mesh> meshes;
        string directory;

        public Model()
        {
            meshes = new List<Mesh>();
        }

        public void Draw(Shader shader)
        {
            meshes.ForEach(mesh =>
            {
                mesh.Draw(shader);
            });
        }

        public void LoadModel(string path)
        {
            AssimpContext importer = new AssimpContext();
            //importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            Assimp.Scene scene = importer.ImportFile(path);

            if (scene.SceneFlags == SceneFlags.Incomplete)
            {
                return;
            }

            directory = $@"{Path.GetDirectoryName(path)}\";
            ProcessNode(scene.RootNode, scene);
        }

        private void ProcessNode(Node node, Assimp.Scene scene)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                var mesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(ProgressMesh(mesh, scene));
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(node.Children[i], scene);
            }
        }

        private Mesh ProgressMesh(Assimp.Mesh mesh, Assimp.Scene scene)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();
            List<Texture> textures = new List<Texture>();

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex();
                vertex.Position = new OpenTK.Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                vertex.Normal = new OpenTK.Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);
                if (mesh.HasTextureCoords(0))
                {
                    vertex.TexCoords = new OpenTK.Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);
                }
                else
                {
                    vertex.TexCoords = new OpenTK.Vector2(0, 0);
                }

                vertices.Add(vertex);
            }

            for (int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];
                for (int j = 0; j < face.IndexCount; j++)
                {
                    indices.Add((uint)face.Indices[j]);
                }
            }

            if (mesh.MaterialIndex >= 0)
            {
                Material material = scene.Materials[mesh.MaterialIndex];
                List<Texture> diffuseMap = LoadMaterialTextures(material, TextureType.Diffuse, "texture_diffuse");
                textures.InsertRange(textures.Count, diffuseMap);

                List<Texture> specularMap = LoadMaterialTextures(material, TextureType.Specular, "texture_specular");
                textures.InsertRange(textures.Count, specularMap);
            }

            return new Mesh(vertices.ToArray(), indices.ToArray(), textures.ToArray());
        }

        private List<Texture> LoadMaterialTextures(Material material, TextureType type, string typeName)
        {
            List<Texture> textures = new List<Texture>();
            for (int i = 0; i < material.GetMaterialTextureCount(type); i++)
            {
                material.GetMaterialTexture(type, i, out TextureSlot slot);
                string textureFilePath = directory + slot.FilePath;
                Texture texture = new Texture()
                {
                    Id = LoadTexture(textureFilePath),
                    Type = typeName,
                    Path = textureFilePath
                };

                textures.Add(texture);
            }

            return textures;
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

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return textureId;
        }
    }
}
