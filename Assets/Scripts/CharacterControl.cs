using UniRx;
using UniRx.Triggers;
using UnityEngine;

public partial class CharacterControl : MonoBehaviour
{
    private State<CharacterControl> CharacterState = new IdleState();

    private void Start()
    {
        this.UpdateAsObservable().Subscribe(_ =>
        {
            CharacterState.Action(this);
            
            var nextState = CharacterState.InputHandle(this);
            if (!CharacterState.Equals(nextState))
            {
                CharacterState = nextState;
            }
        }).AddTo(this);
    }

    // DamageCollider
    public void OnCollisionEnterInChildren(Collision2D other)
    {
        throw new NotImplementedException();
        // ToDo : 피해
    }

    // ObjectCollider
    public void OnTriggerEnterInChildren(Collider2D other)
    {
        throw new NotImplementedException();
        // ToDo : 오브젝트 교환
    }
}
