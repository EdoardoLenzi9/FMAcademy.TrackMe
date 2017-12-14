using SimpleInjector;

namespace FactoryMind.TrackMe.Business.Containers
{
    public enum Lifestyle
    {
        Singleton,
        Transient,
        Scoped
    };

    public class Container
    {
        private static Container _instance;
        private static SimpleInjector.Container _container;
        private Container()
        {
            _container = new SimpleInjector.Container();
        }

        public static Container GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Container();
            }
            return _instance;
        }

        public void Register<TService, TImplementation>(Lifestyle lifeStyle)
            where TService : class
            where TImplementation : class, TService
        {
            switch (lifeStyle)
            {
                case Lifestyle.Singleton:
                    _container.Register<TService, TImplementation>(SimpleInjector.Lifestyle.Singleton);
                    break;
                case Lifestyle.Transient:
                    _container.Register<TService, TImplementation>(SimpleInjector.Lifestyle.Transient);
                    break;
                case Lifestyle.Scoped:
                    _container.Register<TService, TImplementation>(SimpleInjector.Lifestyle.Scoped);
                    break;
            }
        }

        public void Register<TImplementation>(Lifestyle lifeStyle)
            where TImplementation : class
        {
            switch (lifeStyle)
            {
                case Lifestyle.Singleton:
                    _container.Register<TImplementation>(SimpleInjector.Lifestyle.Singleton);
                    break;
                case Lifestyle.Transient:
                    _container.Register<TImplementation>(SimpleInjector.Lifestyle.Transient);
                    break;
                case Lifestyle.Scoped:
                    _container.Register<TImplementation>(SimpleInjector.Lifestyle.Scoped);
                    break;
            }
        }

        public T Resolve<T>()
            where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}