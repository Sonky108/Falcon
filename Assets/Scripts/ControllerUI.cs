using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Falcon
{
    public class ControllerUI : MonoBehaviourSingleton<ControllerUI>
    {
        public event Action TurnLeftDownListeners;
        public event Action TurnLeftUpListeners;
        public event Action TurnRightDownListeners;
        public event Action TurnRightUpListeners;
        public event Action ThrottleDownListeners;
        public event Action ThrottleUpListeners;

        [Header("Buttons")]

        [SerializeField]
        private PointerController TurnLeftObject;

        [SerializeField]
        private PointerController TurnRightObject;

        [SerializeField]
        private PointerController ThrottleObject;

        public override void OnAwake()
        {
            base.OnAwake();
            UnbindButtons();
            BindButtons();
        }

        private void BindButtons()
        {
            TurnLeftObject.OnPointerDown.AddListener(OnTurnLeftButtonPressed);
            TurnRightObject.OnPointerDown.AddListener(OnTurnRightButtonPressed);
            ThrottleObject.OnPointerDown.AddListener(OnThrottleButtonPressed);

            TurnLeftObject.OnPointerUp.AddListener(OnTurnLeftButtonReleased);
            TurnRightObject.OnPointerUp.AddListener(OnTurnRightButtonReleased);
            ThrottleObject.OnPointerUp.AddListener(OnThrottleButtonReleased);
        }

        private void OnTurnRightButtonReleased()
        {
            TurnRightUpListeners?.Invoke();
        }

        private void OnTurnLeftButtonReleased()
        {
            TurnLeftUpListeners?.Invoke();
        }

        private void OnThrottleButtonReleased()
        {
            ThrottleUpListeners?.Invoke();
        }

        private void OnThrottleButtonPressed()
        {
            ThrottleDownListeners?.Invoke();
        }

        private void OnTurnRightButtonPressed()
        {
            TurnRightDownListeners?.Invoke();
        }

        private void OnTurnLeftButtonPressed()
        {
            TurnLeftDownListeners?.Invoke();
        }

        private void UnbindButtons()
        {
            TurnLeftObject.OnPointerDown.RemoveListeners();
            TurnRightObject.OnPointerDown.RemoveListeners();
            ThrottleObject.OnPointerDown.RemoveListeners();

            TurnLeftObject.OnPointerUp.RemoveListeners();
            TurnRightObject.OnPointerUp.RemoveListeners();
            ThrottleObject.OnPointerUp.RemoveListeners();
        }
    }
}