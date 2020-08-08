using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sweet_And_Salty_Studios
{
    public class UIButton : UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField]
        private Image buttonIconSprite;

        [SerializeField]
        private Sprite[] buttonIcons;

        private Action buttonAction;

        private Vector2 defaultSize;
        private Vector2 activeSize;

        private void Awake()
        {
            defaultSize = transform.localScale;
            activeSize = defaultSize * 1.1f;
            buttonIconSprite.sprite = buttonIcons[0];
        }

        private void Start()
        {
            buttonAction = EventManager.Instance.GetButtonEvent(gameObject.name);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerEnter(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = activeSize;
            eventData.pointerPress = gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = defaultSize;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (buttonAction != null)
            {
                buttonAction.Invoke();

                transform.localScale = defaultSize;

                buttonIconSprite.sprite = buttonIconSprite.sprite == buttonIcons[0] ? buttonIcons[1] : buttonIcons[0];
            }
        }
    }
}
