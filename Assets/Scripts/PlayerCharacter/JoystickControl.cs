using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerCharacter
{
    // Player Joystick Input
    public class JoystickControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public readonly Subject<Vector2> JoystickDirection = new Subject<Vector2>();
        public readonly BoolReactiveProperty IsJoystickDown = new BoolReactiveProperty();

        // Components
        private RectTransform _thisTransform; // 현재 조이스틱
        private RectTransform _pointTransform; // 조이스틱 손잡이

        // Constants
        private const float MinRange = 0.3f; // 입력 반지름 최소 범위
        private const float MaxRange = 1.3f; // 입력 반지름 최대 범위

        private Vector2 _pointPosition; // point 기본 자리
        private float _pointRadius; // point 반지름
        private float _posToDir; // 입력 위치 > 입력 방향으로 변환할때 사용하는 값 (1 / _pointRadius)

        private void Start()
        {
            _thisTransform = GetComponent<RectTransform>();
            _pointTransform = transform.GetChild(0).GetComponent<RectTransform>();

            _pointPosition = _pointTransform.position;
            _pointRadius = _thisTransform.sizeDelta.x * 0.3f;
            _posToDir = 1 / _pointRadius;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var inputPosition = eventData.position - _pointPosition;
            _pointTransform.anchoredPosition = inputPosition.magnitude > _pointRadius * MaxRange
                ? inputPosition.normalized * _pointRadius * MaxRange
                : inputPosition;

            JoystickDirection.OnNext(Position2Direction(inputPosition));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsJoystickDown.Value = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointTransform.anchoredPosition = Vector2.zero;
            IsJoystickDown.Value = false;
        }

        private Vector2 Position2Direction(Vector2 position)
        {
            position *= _posToDir;

            if (position.magnitude > MaxRange)
            {
                position = position.normalized * MaxRange;
            }
            else if (position.magnitude < MinRange)
            {
                position = Vector2.zero;
            }

            return position.normalized * (position.magnitude - MinRange);
        }
    }
}
