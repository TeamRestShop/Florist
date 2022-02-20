public interface IState<T>
{
    void Enter(T t);
    void Exit(T t);
}

