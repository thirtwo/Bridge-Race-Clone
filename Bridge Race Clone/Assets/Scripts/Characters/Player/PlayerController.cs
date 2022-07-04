using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirtwo.InputSystem;

namespace Thirtwo.Character
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerController : CharController
    {
        [Header("Player Settings")]
        [SerializeField] private float playerSpeed = 5;
        [SerializeField] private float rotationSpeed = 5;
        private InputHandler inputHandler;
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (!isCharActive) return;
            Move();
        }

        protected override void Init()
        {
            base.Init();
            inputHandler = GetComponent<InputHandler>();
        }
        protected override void Move()
        {
            if (Mathf.Abs(inputHandler.HorizontalInput) > 0.1f || Mathf.Abs(inputHandler.VerticalInput) > 0.1f)
            {
                var direction = Vector3.right * inputHandler.HorizontalInput + Vector3.forward * inputHandler.VerticalInput;
                Rotate(direction);
                transform.Translate(playerSpeed * Time.deltaTime * Vector3.forward);
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
        }
        private void Rotate(Vector3 direction)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(direction.normalized, Vector3.up), rotationSpeed * Time.deltaTime);
        }
        public override void GetActiveFloor()
        {
            base.GetActiveFloor();
            if (activeFloor == null)
            {
                GameManager.FinishGame(true, transform);
            }
        }
        protected override void GameManager_OnGameFinished(bool isWin)
        {
            isCharActive = false;
            if (isWin)
            {
                animator.SetTrigger("Win");
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
