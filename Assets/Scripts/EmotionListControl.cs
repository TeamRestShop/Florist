using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    // Player Emotion Input
    public class EmotionListControl : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public readonly Subject<int> EmotionIndex = new Subject<int>();

        private GameObject _buttons;

        private void Start()
        {
            _buttons = transform.GetChild(0).gameObject;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        // ToDo : 감정 표현 UI 조작하고 손가락 뗐을때 선택한 감정 표현 Subject로 넘기기 
        // 오브젝트 이동하고 캐릭터 교체 조작 어떻게 정지하지
    }
}
