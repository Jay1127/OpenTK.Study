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

        private float yaw = -90.0f;
        private float pitch = 0.0f;
        private float fov = 45.0f;
        private float cameraSpeed = 2.5f;
        private float sensitivity = 0.1f;

        public FPSCamera(Vector3 cameraPos)
        {

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
            if (fov >= 1.0f && fov <= 45.0f)
                fov -= zoomFactor;
            if (fov <= 1.0f)
                fov = 1.0f;
            if (fov >= 45.0f)
                fov = 45.0f;
        }
    }
}
