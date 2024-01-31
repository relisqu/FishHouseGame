using System;
using UnityEngine;

namespace Shaders
{
    [ExecuteInEditMode]
    public class ScreenApplyDeathMat : MonoBehaviour
    {
        public Camera Cam;
        public Material Mat;

        private void Start()
        {
            Cam.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        { 
           
            Graphics.Blit(src, dest, Mat);
        }
    }
}