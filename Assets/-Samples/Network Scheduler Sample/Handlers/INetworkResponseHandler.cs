public interface INetworkResponseHandler<T>
{
    bool CanHandle();
    void HandleResponse(T result);
}