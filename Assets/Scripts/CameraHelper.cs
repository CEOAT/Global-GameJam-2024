using UnityEngine;

namespace GGJ2024
{
    public static class CameraHelper
    {
        static Camera unsafeMainCamera;

        public static Camera mainCamera
        {
            get
            {
                if (unsafeMainCamera == null)
                    unsafeMainCamera = Camera.main;

                return unsafeMainCamera;
            }
        }
    }
}