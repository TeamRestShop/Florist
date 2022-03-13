using System;
using UnityEngine;

namespace PlayerCharacter
{
    // Player Input System
    public class PlayerControl : MonoBehaviour
    {
        public PlayerKeyBoardControl KeyBoard = new PlayerKeyBoardControl();
        private void Start()
        {
            
        }
        
        private void Update()
        {
            KeyBoard.HorizontalAxis = Input.GetAxis("Horizontal");
            KeyBoard.VerticalAxis = Input.GetAxis("Vertical");
            KeyBoard.EKeyDown = Input.GetKeyDown(KeyCode.E);
            KeyBoard.TKeyDown = Input.GetKeyDown(KeyCode.T);
            KeyBoard.SpaceKeyDown = Input.GetKeyDown(KeyCode.Space);
            KeyBoard.LeftShiftKetDown = Input.GetKeyDown(KeyCode.LeftShift);
            KeyBoard.LeftMouseDown = Input.GetMouseButtonDown(0);
            KeyBoard.LeftMouseUp = Input.GetMouseButtonUp(0);
            KeyBoard.RightMouseDown = Input.GetMouseButtonDown(1);
            
        }

        // ToDo : 플레이어 조작 정보 전달
        // ToDo : 카메라 위치 조정
    }
}
