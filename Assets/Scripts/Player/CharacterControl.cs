using UniRx;
using UnityEngine;

namespace Player
{
    // Player Character Control System
    public partial class CharacterControl : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        
        public readonly Subject<Collider2D> objectCollider = new Subject<Collider2D>();
        public readonly Subject<Collision2D> damageCollider = new Subject<Collision2D>();

        // Todo : 플레이어 캐릭터 기본 세팅(상태0, 히트박스0, 이동)

        private void Start()
        {
            StateInit();
            
            /*
             * Enter -> ReactiveProperty에서
             * Update -> stateStream
             * Exit -> ReactiveProperty에서
             */
        }
    }
}
