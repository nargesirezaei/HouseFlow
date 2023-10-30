namespace HouseFlowPart1.Interfaces
{
    public interface ILogService
    {
        void WriteExceptionAsync(string request, string requestCode); // Write an exception log with the given request and code
        void WriteVisitAsync(string message); // Write a visit log with the provided message
    }
}
