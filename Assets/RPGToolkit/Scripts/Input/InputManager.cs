using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGToolkit
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public void SubmitPressed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                EventsManager.Instance.inputEvents.SubmitPressed();
            }
        }

        // public void QuestLogTogglePressed(InputAction.CallbackContext context)
        // {
        //     if (context.started)
        //     {
        //         EventsManager.instance.inputEvents.QuestLogTogglePressed();
        //     }
        // }
    }
}