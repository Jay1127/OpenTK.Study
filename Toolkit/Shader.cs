using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Toolkit
{
    public class Shader
    {
        public int Id
        {
            get => _shaderProgram;
        }

        private int _shaderProgram;
        private int _vertexShader;
        private int _fragShader;
        private string _vertexShaderPath;
        private string _fragShaderPath;

        public Shader(string vertexShaderPath, string fragmentShaderPath)
        {
            _vertexShaderPath = vertexShaderPath;
            _fragShaderPath = fragmentShaderPath;
        }

        public void Create()
        {
            _vertexShader = CreateShader(_vertexShaderPath, ShaderType.VertexShader);
            _fragShader = CreateShader(_fragShaderPath, ShaderType.FragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, _vertexShader);
            GL.AttachShader(_shaderProgram, _fragShader);
            GL.LinkProgram(_shaderProgram);

            GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out int success);
            if (success == -1)
            {
                GL.GetProgramInfoLog(_shaderProgram, out string infoLog);
                throw new Exception($"shader program is not linked : {infoLog}");
            }

            GL.DeleteShader(_vertexShader);
            GL.DeleteShader(_fragShader);
        }

        public void SetInt(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(Id, name), value);            
        }

        public void SetVec3(string name, float x, float y, float z)
        {
            GL.Uniform3(GL.GetUniformLocation(Id, name), x, y, z);
        }

        public void SetMat4(string name, Matrix4 matrix)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(Id, name), false, ref matrix);
        }

        public void UseProgram()
        {
            GL.UseProgram(_shaderProgram);
        }

        private int CreateShader(string shaderFilePath, ShaderType shaderType)
        {
            int id = GL.CreateShader(shaderType);

            using (StreamReader sr = new StreamReader(shaderFilePath))
            {
                GL.ShaderSource(id, sr.ReadToEnd());
            }

            GL.CompileShader(id);

            GL.GetShader(id, ShaderParameter.CompileStatus, out int success);
            if (success == -1)
            {
                GL.GetShaderInfoLog(id, out string infoLog);
                throw new InvalidDataException(infoLog);
            }

            return id;
        }
    }
}
