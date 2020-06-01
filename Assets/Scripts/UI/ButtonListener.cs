using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonListener : MonoBehaviour
    {
        public ButtonType buttonType;
        
        private MenuActionHandler _menuActionHandler;

        private Button _button;
    
        private void Awake()
        {
            _menuActionHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MenuActionHandler>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy() => _button.onClick.RemoveListener(OnButtonClick);
        
        private void OnButtonClick() => _menuActionHandler.OnButtonClicked(buttonType);
    }
}
