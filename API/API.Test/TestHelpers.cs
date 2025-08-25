using Microsoft.JSInterop;
using API.Web;
using API.ApiService.DB;
using MySqlConnector;

namespace API.Test
{
    public static class TestHelpers
    {
        public static OsobyService CreateOsobyService()
        {
            var fakeDataSource = (MySqlDataSource)null!;
            var fakeSession = new UserSessionService();
            var fakeJsRuntime = new FakeJsRuntime();

            return new OsobyService(fakeDataSource, fakeSession, fakeJsRuntime);
        }
    }
    
    public class FakeJsRuntime : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] args)
        {
            return new ValueTask<TValue>(default(TValue)!);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[] args)
        {
            return new ValueTask<TValue>(default(TValue)!);
        }
    }
}