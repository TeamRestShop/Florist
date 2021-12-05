using System;

public abstract class State<T>
{
    public Action<T> Action;
    public abstract State<T> InputHandle(T t);

    public State()
    {
        Action = Enter;
    }

    public virtual void Enter(T t)
    {
        Action = Update;
    }

    public virtual void Update(T t)
    {
        
    }

    public virtual void Exit(T t)
    {
        
    }
}
