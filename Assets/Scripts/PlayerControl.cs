using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    // Player Input System
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private JoystickControl movingJoystick;
        [SerializeField] private JoystickControl objectJoystick;
        [SerializeField] private EmotionListControl emotionList;
        [SerializeField] private Button changeButton;

        private void Start()
        {
            var changeButtonStream = changeButton.OnClickAsObservable();

        }

        /* ToDo : 플레이어 조작
         * 이동 : 이동값
         * 오브젝트 : 터치 이벤트, 이동값
         * 이동, 오브젝트 : 홀드 -> 드래그 or 드랍
         * 감정 : 선택 이벤트
         * 교체 : 클릭 이벤트
         */

        // ToDo : 카메라 위치 조정
    }
}
