using System;
using UnityEngine;

namespace RPGToolkit
{
    public class InputEvents : MonoBehaviour
    {
        public event Action onSubmitPressed;
        public void SubmitPressed()
        {
            if (onSubmitPressed != null) 
            {
                onSubmitPressed();
            }
        }
    }
}