using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.CameraAction
{
    public interface ICameraAction
    {
        void SetCamera(Camera camera);
        void UpdateCamera();
        void FixedUpdateCamera();
    }
}
