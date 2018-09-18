using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit
{
    public class FPSCamera
    {
        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; }
        public Vector3 Up { get; set; }
        public Matrix4 ViewMatrix
        {
            get => Matrix4.LookAt(Position, Position + Front, Up);
        }

        public float Fov { get; private set; }
        private float yaw = -90.0f;
        private float pitch = 0.0f;
        private readonly float cameraSpeed = 2.5f;
        private readonly float sensitivity = 0.1f;

        public FPSCamera(Vector3 cameraPos)
        {
            Position = cameraPos;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);

            Fov = 45.0f;
        }

        public void MoveForward()
        {
            Position += cameraSpeed * Front;
        }

        public void MoveBackward()
        {
            Position -= cameraSpeed * Front;
        }

        public void MoveLeft()
        {
            Position -= Vector3.Cross(Front, Up).Normalized() * cameraSpeed;
        }

        public void MoveRight()
        {
            Position -= Vector3.Cross(Front, Up).Normalized() * cameraSpeed;
        }

        public void Rotate(float xDelta, float yDelta)
        {
            yaw += xDelta * sensitivity;
            pitch += yDelta * sensitivity;

            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            Vector3 front;
            front.X = (float)(Math.Cos(MathUtil.ToRadian(yaw)) * Math.Cos(MathUtil.ToRadian(pitch)));
            front.Y = (float)Math.Sin(MathUtil.ToRadian(pitch));
            front.Z = (float)(Math.Sin(MathUtil.ToRadian(yaw)) * Math.Cos(MathUtil.ToRadian(pitch)));
            Front = Vector3.Normalize(front);
        }

        public void Zoom(float zoomFactor)
        {
            if (Fov >= 1.0f && Fov <= 45.0f)
                Fov -= zoomFactor;
            if (Fov <= 1.0f)
                Fov = 1.0f;
            if (Fov >= 45.0f)
                Fov = 45.0f;
        }
    }
}
