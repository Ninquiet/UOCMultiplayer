using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Complete
{
    public class InputsManager : MonoBehaviour
    {
        [SerializeField]
        private InputsButtons[] _inputsButtons = default;

        private List<int> _initializedInputs = new List<int>();

        public Action<InputsButtons> NewInputSetIsActive;
        
        private void Start()
        {
            foreach (var input in _inputsButtons)
            {
                input.Fire.action.Enable();
                input.Move.action.Enable();
                input.Turn.action.Enable();
            }
        }
        
        private void Update()
        {
            for (int i = 0; i < _inputsButtons.Length; i++)
            {
                if (_inputsButtons[i].Fire.action.triggered)
                {
                    if (!_initializedInputs.Contains(i))
                    {
                        _initializedInputs.Add(i);
                        NewInputSetIsActive?.Invoke(_inputsButtons[i]);
                    }
                }
            }
        }
    }

    [Serializable]
    public class InputsButtons
    {
        public InputActionReference Fire;
        public InputActionReference Move;
        public InputActionReference Turn;
    }
}