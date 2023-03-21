using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class TouchControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public Action<Vector3> ChangingEvent;
        public Action<Vector3> ChangedEvent;

        [SerializeField]
        private RectTransform Mask = null;

        [SerializeField]
        private RectTransform Background = null;

        [SerializeField]
        private RectTransform Control = null;

        [SerializeField]
        private float MaxTapDist = 100f;

        private Vector2 _tapPosition;
        private EventSystem _eventSystem;
        private GraphicRaycaster _raycaster;
        private bool _canProcess = false;

        // Start is called before the first frame update
        private void Start()
        {
            _raycaster = GetComponentInParent<GraphicRaycaster>();
            _eventSystem = GetComponent<EventSystem>();

            _canProcess = false;
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private bool IsEmptyArea()
        {
            var pointerEventData = new PointerEventData(_eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            _raycaster.Raycast(pointerEventData, results);


            foreach (RaycastResult result in results)
            {
                if (result.gameObject != this.gameObject &&
                    result.gameObject != Background.gameObject &&
                    result.gameObject != Control.gameObject &&
                    result.gameObject != Mask.gameObject)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual float ChangeOutput(float input)
        {
            return input;
        }

        public void Reset()
        {
            Control.localPosition = Background.localPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsEmptyArea())
            {
                _tapPosition = eventData.position;
                transform.position = _tapPosition;

                _canProcess = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_canProcess)
            {
                var dist = (eventData.position - _tapPosition).magnitude;
                if (dist > MaxTapDist)
                {
                    dist = MaxTapDist;
                }

                var dir = (eventData.position - _tapPosition).normalized;

                Reset();

                ChangedEvent?.Invoke(dir * ChangeOutput(dist / MaxTapDist));

                _canProcess = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_canProcess)
            {
                var dist = (eventData.position - _tapPosition).magnitude;
                if (dist > MaxTapDist)
                {
                    dist = MaxTapDist;
                }

                var dir = (eventData.position - _tapPosition).normalized;

                Control.localPosition = Background.localPosition + (Vector3)dir * dist;

                ChangingEvent?.Invoke(dir * ChangeOutput(dist / MaxTapDist));
            }
        }
    }
}