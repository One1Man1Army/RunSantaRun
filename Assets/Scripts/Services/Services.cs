namespace RSR.ServicesLogic
{
    /// <summary>
    /// Service locator realization.
    /// Grants us access to sevices adding functionality and provides container with all added services.
    /// Container is available all over the game, but should be used only in classes-initializers, to keep dependencies logic clear.
    /// </summary>
    public sealed class Services
    {
        private static Services _instance;
        public static Services Container
        {
            get
            {
                if (_instance == null)
                    _instance = new Services();

                return _instance;
            }
        }

        public void AddService<T>(T implementation) where T : IService
        {
            Implementation<T>.ServiceInstance = implementation;
        }

        public T GetService<T>() where T : IService
        {
            return Implementation<T>.ServiceInstance;
        }

        private class Implementation<T> where T : IService
        {
            public static T ServiceInstance;
        }
    }
}
