using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit
{
    public class SceneBase : GameWindow
    {
        public FPSCamera Camera { get; protected set; }
        public Matrix4 View { get; protected set; }
        public Matrix4 Model { get; protected set; }
        public Matrix4 Projection { get; protected set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Camera = new FPSCamera(new Vector3(0, 0, 3));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.W)
            {
                Camera.MoveForward();
            }
            else if (e.Key == Key.S)
            {
                Camera.MoveBackward();
            }
            else if (e.Key == Key.A)
            {
                Camera.MoveLeft();
            }
            else if (e.Key == Key.D)
            {
                Camera.MoveRight();
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if(e.Mouse.LeftButton == ButtonState.Pressed)
            {
                Camera.Rotate(-e.XDelta, e.YDelta);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Camera.Zoom(e.Delta);
        }
    }
}
