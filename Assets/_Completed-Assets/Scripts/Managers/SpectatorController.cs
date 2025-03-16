using UnityEngine;
using UnityEngine.InputSystem;

namespace Complete
{
    public class SpectatorController : MonoBehaviour
    {
        private InputActionReference _moveUpDownActionReference;
        private InputActionReference _moveLeftRightActionReference;

        public void Setup(InputsButtons inputs, Vector3 position)
        {
            transform.position = position;
            _moveUpDownActionReference = inputs.Move;
            _moveLeftRightActionReference = inputs.Turn;
        }
        
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var moveX = _moveLeftRightActionReference.action.ReadValue<float>();
            var moveY = _moveUpDownActionReference.action.ReadValue<float>();
            var moveDirection = new Vector3(moveX, 0, moveY);
            transform.position += moveDirection * Time.deltaTime * 20;
        }
    }
}