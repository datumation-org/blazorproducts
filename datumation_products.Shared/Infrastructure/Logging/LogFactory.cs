namespace datumation_products.Shared.Infrastructure.Logging
{
    public class LogFactory
    {
        private static ILogFactory _config;

        // Private constructor to avoid other instantiation
        // This must be present otherwise the compiler provide
        // a default public constructor
        private LogFactory() { }

        public ILogFactory Log()
        {
            return _config;
        }

        /// <summary>
        /// Return an instance
        /// </summary>
        public static LogFactory Instance
        {
            // An instance of Singleton wont be created until the very first
            // call to the sealed class. This a CLR optimization that ensure that
            // we have properly lazy-loading singleton.
            get
            {
                return SingletonCreator.CreatorInstance;
            }
        }

        /// <summary>
        /// Sealed class to avoid any heritage from this helper class
        /// </summary>
        private sealed class SingletonCreator
        {
            // Retrieve a single instance of a Singleton
            private static readonly LogFactory _instance = new LogFactory();

            /// <summary>
            /// Return an instance of the class
            /// </summary>
            public static LogFactory CreatorInstance
            {
                get { return _instance; }
            }
        }

        public static void Initialize(ILogFactory c)
        {
            _config = c;
        }
    }
}