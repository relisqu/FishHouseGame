using Sirenix.Utilities;
using UnityEngine;

namespace Shaders
{
    [ExecuteInEditMode]
    public class ScreenApplyColorReduction : MonoBehaviour
    {
        public Camera Cam;
        public Material Mat;

        public Gradient colors;


        private void Start()
        {
            Cam.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            var colorsGradient = colors.colorKeys;
            Color[] finalColors = new Color[colorsGradient.Length];
            for (var index = 0; index < colorsGradient.Length; index++)
            {
                finalColors[index] = colorsGradient[index].color;
            }
            Mat.SetColorArray("_Pallet", finalColors);
            Mat.SetInt("_PixelPalletCount", finalColors.Length);

            Graphics.Blit(src, dest, Mat);
        }
    }
}