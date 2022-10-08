using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Manager
{
    public class UIManager : MonoBehaviour
    {
        public GameObject StaticJoyStickLeft;
        public GameObject JoyStick;

        private void Awake()
        {
            //StaticJoyStickLeft.SetActive(false);
            JoyStick.SetActive(false);
        }

        void Update()
        {

        }
    }
}
