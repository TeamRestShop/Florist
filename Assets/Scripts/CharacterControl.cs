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
    
    
}
