public interface IBind<T> where T : ISavable
{
    void Bind(T t);
}