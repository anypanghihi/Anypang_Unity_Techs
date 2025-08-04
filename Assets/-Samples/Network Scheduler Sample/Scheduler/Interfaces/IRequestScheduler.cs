using System.Collections.Generic;

public interface IRequestScheduler<T>
{
    public void Register(T data);
    public void Unregister(T data);
    public IEnumerable<T> GetAll();
    public void Clear();
}