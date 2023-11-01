using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Shaders
{
    [ExecuteInEditMode]
    public class ScreenApplyColorReduction : MonoBehaviour
    {
        public Camera Cam;
        public Material Mat;

        public Texture2D colors;


        private void Start()
        {
            Cam.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            
            Color[] finalColors = new Color[colors.width];
            for (var index = 0; index < finalColors.Length; index++)
            {
                finalColors[index] = colors.GetPixelBilinear((float)index/finalColors.Length,0);
            }
            Mat.SetColorArray("_Pallet", finalColors);
            Mat.SetInt("_PixelPalletCount", finalColors.Length);

            Graphics.Blit(src, dest, Mat);
        }
    }
}