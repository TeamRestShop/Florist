using Photon.Pun;
using UniRx;
using UnityEngine;

namespace PlayerCharacter
{
    // Player Character Control System
    public partial class CharacterControl : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        
        public readonly Subject<Collider2D> objectCollider = new Subject<Collider2D>();
        public readonly Subject<Collision2D> damageCollider = new Subject<Collision2D>();

        private static readonly int MainState = Animator.StringToHash("MainState");
        private Animator _animator;
        private PhotonView _photonView;
        private SpriteRenderer _arrowSprite;
        private Transform _arrowTransform;

        private Flower _havingFlower;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _photonView = GetComponent<PhotonView>();
            _arrowSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
            _arrowTransform = _arrowSprite.transform;
            _arrowSprite.enabled = false;
            _havingFlower = null;

            if (_photonView.IsMine)
            {
                StateInit();

                damageCollider.Subscribe(_ =>
                {
                    /* 피해 */
                });
                objectCollider.Subscribe(_ =>
                {
                    /* 오브젝트 교환 */
                    Debug.Log("으아아아아아아아");
                });
            }
            /*
             * Enter -> ReactiveProperty에서
             * Update -> stateStream
             * Exit -> ReactiveProperty에서
             */
        }
    }
}
