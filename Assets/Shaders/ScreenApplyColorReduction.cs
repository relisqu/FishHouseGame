using System.Collections.Generic;
using System.Linq;
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

        private List<Color> finalColors;
        private void Start()
        {
            Cam.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            
            finalColors = new List<Color>();
            finalColors.Clear();
            for (var index = 0; index < colors.width; index++)
            {
                var color = colors.GetPixelBilinear((float)index /  colors.width, 0);
                if (!finalColors.Contains(color))
                {
                    finalColors.Add(color);
                }
            }
            Mat.SetColorArray("_Pallet", finalColors.ToArray());
            Mat.SetInt("_PixelPalletCount", finalColors.Count);

            Graphics.Blit(src, dest, Mat);
        }
    }
}