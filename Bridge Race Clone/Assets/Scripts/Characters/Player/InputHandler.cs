using UnityEngine;
namespace Thirtwo.InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        private float horizontalInput = 0;
        private float verticalInput = 0;
        public float HorizontalInput { get { return horizontalInput; } }
        public float VerticalInput { get { return verticalInput; } }


        private Vector2 firstPos;
        void Update()
        {
            MobileControl();
        }

        private void MobileControl()
        {
            if (Input.touchCount <= 0)
                return;
            if (!GameManager.isGameStarted) GameManager.StartGame();
            var touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                firstPos = touch.position;
            }
            if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                var diff = touch.position - firstPos;
                diff.Normalize();
                verticalInput = diff.y;
                horizontalInput = diff.x;
            }
            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                verticalInput = 0;
                horizontalInput = 0;
            }
        }
    }
}
