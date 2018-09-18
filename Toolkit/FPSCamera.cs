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
        public Vector3 CameraPos { get; set; }
        public Vector3 CameraFront { get; set; }
        public Vector3 CameraUp { get; set; }
        public Matrix4 ViewMatrix
        {
            get => Matrix4.LookAt(CameraPos, CameraPos + CameraFront, CameraUp);
        }

        public float Fov { get; private set; }
        private float yaw = -90.0f;
        private float pitch = 0.0f;
        private float cameraSpeed = 2.5f;
        private float sensitivity = 0.1f;

        public FPSCamera(Vector3 cameraPos)
        {
            CameraPos = cameraPos;
            CameraFront = new Vector3(0.0f, 0.0f, -1.0f);
            CameraUp = new Vector3(0.0f, 1.0f, 0.0f);

            Fov = 45.0f;
        }

        public void MoveForward()
        {
            CameraPos += cameraSpeed * CameraFront;
        }

        public void MoveBackward()
        {
            CameraPos -= cameraSpeed * CameraFront;
        }

        public void MoveLeft()
        {
            CameraPos -= Vector3.Cross(CameraFront, CameraUp).Normalized() * cameraSpeed;
        }

        public void MoveRight()
        {
            CameraPos -= Vector3.Cross(CameraFront, CameraUp).Normalized() * cameraSpeed;
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
            CameraFront = Vector3.Normalize(front);
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
