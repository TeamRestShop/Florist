using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Player
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

            idleStream.Merge(walkStream).Where(_ => Input.GetKeyDown(KeyCode.Space))
                .ThrottleFirst(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
                {
                    /* 줍기 */
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
                .SkipUntil(mouseLeftDown).TakeUntil(mouseLeftUp)
                .RepeatUntilDisable(this).Subscribe(_ =>
                {
                    /* 오브젝트 던지기 조준 */
                    Debug.Log("[Act] 던지기 조준");
                }).AddTo(this);

            mouseRightDown.Subscribe(_ =>
            {
                objectThrow.Dispose(); /* 던지기 취소 */
                Debug.Log("[Act] 던지기 취소");
            }).AddTo(this);

            skillStream.Subscribe(_ => { }).AddTo(this);

            var keyEDown = idleStream.Where(_ => Input.GetKeyDown(KeyCode.E));
            var keyEUp = idleStream.Where(_ => Input.GetKeyUp(KeyCode.E));
            keyEDown.SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3f))).TakeUntil(keyEUp)
                .RepeatUntilDisable(this)
                .Subscribe(_ =>
                {
                    /* 스킬 준비 */
                    currentState.Value = State.Skill;
                    Debug.LogError("Skill EEEE");
                }).AddTo(this);

            skillStream.Where(_ => Input.GetKeyDown(KeyCode.E))
                .ThrottleFirst(TimeSpan.FromSeconds(1.5)).Subscribe(_ =>
                {
                    /* 스킬 시전 */
                    currentState.Value = State.Idle;
                }).AddTo(this);

            damageCollider.Subscribe(_ =>
            {
                /* 피해 */
            });
            objectCollider.Subscribe(_ =>
            {
                /* 오브젝트 교환 */
            });
        }

        public class IdleState : IState<CharacterControl>
        {
            // ToDo : 상태별 동작 설정

            public void Enter(CharacterControl t)
            {
                // ToDo : 애니메이션 재생
            }

            public void Exit(CharacterControl t)
            {
            }
        }

        public class SkillState : IState<CharacterControl>
        {
            // ToDo : 상태별 동작 설정
            public void Enter(CharacterControl t)
            {
                // ToDo : 애니메이션 재생
            }

            public void Exit(CharacterControl t)
            {
            }
        }

        public class WalkState : IState<CharacterControl>
        {
            // ToDo : 상태별 동작 설정
            public void Enter(CharacterControl t)
            {
                // ToDo : 애니메이션 재생
            }

            public void Exit(CharacterControl t)
            {
            }
        }
    }
}
