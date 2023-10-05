using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]private float MainAttackDelay;
        
        private Animator _animator;
        private bool _isAttacking;
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isAttacking)
            {
                StartCoroutine(StartMainAttack());
            }
        }

        public IEnumerator StartMainAttack()
        {
            _isAttacking = true;
            _animator.SetTrigger("MainAttack");
            yield return new WaitForSeconds(MainAttackDelay);
            _isAttacking = false;
        }
    }
}