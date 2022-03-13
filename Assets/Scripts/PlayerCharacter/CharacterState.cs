using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace PlayerCharacter
{
// Define Player Character FSM State 
    public partial class CharacterControl
    {
        public enum State
        {
            Idle,
            Skill,
            Walk
        }

        public readonly ReactiveProperty<State> currentState = new ReactiveProperty<State>(State.Idle);
        private readonly IState<CharacterControl>[] states = {new IdleState(), new SkillState(), new WalkState()};
        private const float RadianToDegree = 180 / Mathf.PI;
        [SerializeField] float speed = 5f;

        private void StateInit()
        {
            // Enter
            currentState.Subscribe(state =>
            {
                states[(int) state].Enter(this);
                Debug.Log($"[State] -> {state}");
            }).AddTo(this);
            currentState.Scan((previous, current) => previous).Subscribe(state =>
            {
                states[(int) state].Exit(this);
            }).AddTo(this);
            var idleStream = this.UpdateAsObservable().Where(_ => currentState.Value == State.Idle);
            var walkStream = this.UpdateAsObservable().Where(_ => currentState.Value == State.Walk);
            var skillStream = this.UpdateAsObservable().Where(_ => currentState.Value == State.Skill);

            idleStream.Subscribe(_ =>
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    currentState.Value = State.Walk;
                    return;
                }
            }).AddTo(this);
            walkStream.Subscribe(_ =>
            {
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    currentState.Value = State.Idle;
                    return;
                }

                var move = new Vector2(Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;
                transform.Translate(move);
            }).AddTo(this);

            objectCollider.Where(col => 
                    col.CompareTag("Flower") && Input.GetKey(KeyCode.Space) && 
                    (currentState.Value == State.Idle || currentState.Value == State.Walk))
                .ThrottleFirst(TimeSpan.FromSeconds(1f)).Subscribe(flower =>
                {
                    /* 줍기 */
                    if (!_havingFlower)
                    {
                        _havingFlower = flower.GetComponent<Flower>();
                        _havingFlower.transform.parent = transform;
                        _havingFlower.transform.localPosition = new Vector3(0, 0.3f, 0);
                    }
                    Debug.Log("[Act] 줍기");
                }).AddTo(this);

            idleStream.Merge(walkStream).Where(_ => Input.GetKeyDown(KeyCode.T))
                .ThrottleFirst(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
                {
                    /* 감정표현 */
                    Debug.Log("[Act] 감점 표현");
                }).AddTo(this);

            idleStream.Merge(walkStream).Where(_ => Input.GetKeyDown(KeyCode.LeftShift))
                .ThrottleFirst(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
                {
                    /* 캐 교체 */
                    Debug.Log("[Act] 교체");
                }).AddTo(this);

            var mouseLeftDown = idleStream.Merge(walkStream).Where(_ => Input.GetMouseButtonDown(0));
            var mouseLeftUp = idleStream.Merge(walkStream).Where(_ => Input.GetMouseButtonUp(0));
            
            var mouseRightDown = idleStream.Merge(walkStream).Where(_ => Input.GetMouseButtonDown(1));

            var objectThrow = idleStream.Merge(walkStream)
                .SkipUntil(mouseLeftDown).TakeUntil(mouseLeftUp).RepeatUntilDisable(this);

            objectThrow.Subscribe(_ =>
            {
                _arrowSprite.enabled = true;
                PointArrow(CurrentPointDirection());
            }).AddTo(this);
            
            mouseLeftUp.Subscribe(_ =>
            {
                /* 오브젝트 던지기 실행 */
                if (_havingFlower)
                {
                    // _havingFlower.GetComponent<Rigidbody2D>()
                    //     .AddForce(CurrentPointDirection() * 10, ForceMode2D.Impulse);
                    _havingFlower.GetComponent<Rigidbody2D>().AddForce(CurrentPointDirection() * speed, ForceMode2D.Impulse);
                    Debug.Log($"{CurrentPointDirection() * speed}");
                    _havingFlower.transform.parent = null;
                    _havingFlower = null;
                    Debug.Log("[Act] 던지기 실행");
                }

                _arrowSprite.enabled = false;
            }).AddTo(this);
            
            mouseRightDown.Subscribe(_ =>
            {
                _arrowSprite.enabled = false;
                Debug.Log("[Act] 던지기 취소");
            }).AddTo(this);

            skillStream.Subscribe(_ => { }).AddTo(this);

            var keyEDown = idleStream.Where(_ => Input.GetKeyDown(KeyCode.E));
            var keyEUp = idleStream.Where(_ => Input.GetKeyUp(KeyCode.E));
            keyEDown.SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3f))).TakeUntil(keyEUp)
                .RepeatUntilDisable(this)
                .Subscribe(_ =>
                {
                    currentState.Value = State.Skill;
                }).AddTo(this);

            skillStream.Where(_ => Input.GetKeyDown(KeyCode.E))
                .ThrottleFirst(TimeSpan.FromSeconds(1.5)).Subscribe(_ =>
                {
                    /* 스킬 시전 */
                    currentState.Value = State.Idle;
                }).AddTo(this);
        }

        private Vector2 CurrentPointDirection()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos - transform.position;

            return dir;
        }
        
        private void PointArrow(Vector2 direction)
        {
            var pos = _arrowTransform.localScale;
            pos.x = Mathf.Min(2, direction.magnitude);
            _arrowTransform.localScale = pos;

            var z = Mathf.Atan2(direction.y, direction.x) * RadianToDegree;
            _arrowTransform.localRotation = Quaternion.Euler(0,0, z);
        }

        public class IdleState : IState<CharacterControl>
        {
            public void Enter(CharacterControl t)
            {
                t._animator.SetInteger(MainState, (int)State.Idle);
            }

            public void Exit(CharacterControl t)
            {
            }
        }

        public class SkillState : IState<CharacterControl>
        {
            public void Enter(CharacterControl t)
            {
                t._animator.SetInteger(MainState, (int)State.Skill);
            }

            public void Exit(CharacterControl t)
            {
            }
        }

        public class WalkState : IState<CharacterControl>
        {
            public void Enter(CharacterControl t)
            {
                t._animator.SetInteger(MainState, (int)State.Walk);
            }

            public void Exit(CharacterControl t)
            {
            }
        }
    }
}
