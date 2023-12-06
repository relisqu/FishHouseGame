using System;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Drops
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private Transform TextTransform;
        private Camera _camera;
        private TMP_Text _tmpText;

        public void SetText(String text)
        {
            _tmpText.SetText(text);
        }


        public void Interact()
        {
        }

        public void Start()
        {
            _tmpText = TextTransform.GetComponentInChildren<TMPro.TMP_Text>();
            _camera = Camera.main;
            TextTransform.localScale = new Vector3(1f, 0f, 1f);
        }

        private void Update()
        {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement _))
            {
                ShowText();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement _))
            {
                HideText();
            }
        }


        public void ShowText()
        {
            TextTransform.DOScaleY(1f, 0.3f);
        }

        public void HideText()
        {
            TextTransform.DOScaleY(0f, 0.3f);
        }
    }
}