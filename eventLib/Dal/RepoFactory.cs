using Microsoft.Extensions.DependencyInjection;

namespace eventLib.Dal
{
    public static class RepoFactory
    {
        private static IServiceScopeFactory? _serviceScopeFactory;

        public static void SetServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public static IRepository GetRepo()
        {
            if (_serviceScopeFactory == null)
            {
                throw new InvalidOperationException("Service scope factory not set. Call SetServiceScopeFactory first.");
            }
            
            using var scope = _serviceScopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IRepository>();
        }
    }
}
