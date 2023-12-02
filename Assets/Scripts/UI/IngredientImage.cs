using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class IngredientImage : MonoBehaviour
    {
        [SerializeField] private Image Image;

        public Image GetImage()
        {
            return Image;
        }
    }
}