﻿using UnityEngine;

public partial class CharacterControl : MonoBehaviour
{
    public class IdleState : State<CharacterControl>
    {
        public override State<CharacterControl> InputHandle(CharacterControl t)
        {
            throw new System.NotImplementedException();
            // ToDo [Status조건 설정]: Idle -> Skill => 스킬 준비 조작을 했을 때
            // ToDo [Status조건 설정]: Idle -> Walk => 이동 조작을 했을 때
        }

        public override void Enter(CharacterControl t)
        {
            base.Enter(t);
        }

        public override void Update(CharacterControl t)
        {
            base.Update(t);
        }

        public override void Exit(CharacterControl t)
        {
            base.Exit(t);
        }
    }
    
    public class SkillState : State<CharacterControl>
    {
        public override State<CharacterControl> InputHandle(CharacterControl t)
        {
            throw new System.NotImplementedException();
            // ToDo [Status조건 설정]: Skill -> Idle => 스킬을 취소했을 때 or 스킬 시전이 완료 되었을 때 
        }

        public override void Enter(CharacterControl t)
        {
            base.Enter(t);
        }

        public override void Update(CharacterControl t)
        {
            base.Update(t);
        }

        public override void Exit(CharacterControl t)
        {
            base.Exit(t);
        }
    }
    
    public class WalkState : State<CharacterControl>
    {
        public override State<CharacterControl> InputHandle(CharacterControl t)
        {
            throw new System.NotImplementedException();
            // ToDo [Status조건 설정]: Walk -> Idle => 이동 조이스틱에서 손을 땠을 때 or 이동 조이스틱 중앙에 손가락이 위치할 때 
        }

        public override void Enter(CharacterControl t)
        {
            base.Enter(t);
        }

        public override void Update(CharacterControl t)
        {
            base.Update(t);
        }

        public override void Exit(CharacterControl t)
        {
            base.Exit(t);
        }
    }

}