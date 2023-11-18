# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased
### Added
- Created this package!
- Added DiContainer component which can be used to register and resolve dependencies
  - Added method `Register<T>` to register a simple service
  - Added method `Register<TKey, TService>` to register a service with a specific implementation
  - Added method `Register(Type)` to register a simple service
  - Added method `Register(KeyType, ServiceType)` to register a service with a specific implementation
  - Added method `Register<T>(Callback)` to register a type with a callback
  - Added method `Register(Type, Callback)` to register a type with a callback
  - Added method `Override<TKey, TService>` to override any service with a specific implementation
  - Added method `Override(KeyType, ServiceType)` to override any service with a specific implementation
  - Added method `Override<TKey>` to override any service configurations, like singleton mode
  - Added method `Override(KeyType)` to override any service configurations, like singleton mode
  - Added method `Override<T>(Callback)` to override any service with a specific callback
  - Added method `Override(Type, Callback)` to override any service with a specific callback
  - Added method `Resolve<T>` to resolve a service
  - Added method `Resolve(Type)` to resolve a service
  - Added method `ResolveDependencies(Instance)` to resolve all dependencies with the `[Resolve]`attribute
  - Added method `Clear` to remove any registered service and singleton instance
  - Added method `ResetSingletons` to remove any singleton instance
  - Added method `AddConfigurator(configurator)` to register a configurator
- The container can automatically resolve dependencies, even if they are not registered
  - This option can be toggled in the configuration
  - Optionally it can be configured if auto resolved services are stored as singletons
- Services can be marked as lazy or non lazy
  - Non lazy services will automatically be resolved when the container initializes
  - Added methods to service definition:
    - `SetLazyMode(lazyMode)` to specify the lazy mode
    - `Lazy()` to mark a service as lazy (Alias for `SetLazyMode(LazyMode.Lazy)`)
    - `NonLazy()` to mark a service as non-lazy (Alias for `SetLazyMode(LazyMode.NonLazy)`)
- Services can be transient or singleton
  - Transient services will always be resolved as a new instance
  - Added methods to service definition:
    - `SetSingletonMode(singletonMode)` to specify the singleton mode
    - `Singleton()` to mark a service as singleton (Alias for `SetSingletonMode(SingletonMode.Singleton)`)
    - `NonSingleton()` to mark a service as non-singleton (Alias for `SetSingletonMode(SingletonMode.NonSingleton)`)
    - `Transient()` as an alias for `NonSingleton()`
- Services can register the singleton instances directly
  - Added method to service definition:
    - `SetInstance(instance)` to specify the singleton instance
- Services can have IDs to be registered multiple times
  - The id can be specified in the registration methods
  - By default every service is registered without an id
- Services can be resolved using the `[Resolve]` attribute
  - It works on public and private fields and properties
  - It works on public and private methods
  - The attribute takes an optional id parameter
- Add component: `DynamicDependencyResolver`
  - It resolves all dependencies of `[Resolve]` attributes for game-objects that are instantiated at runtime
- Services can be registered in configurators
  - Either by using the `IDiConfigurator` interface and manually registering the configurator
  - Or by using the `MonoDiConfigurator` component and adding it to the containers inspector
- Handle security methods and exceptions
  - When a service is abstract / interface / enum
  - When a service is already registered
  - When a service couldn't be resolved
  - When a circular dependency is detected
- Added ability to get a singleton version of the container from anywhere
  - Using `Ducktion.singleton`
  - This will also create a new container if none exists
- The container logs every event in the console
  - This is configurable on the container itself and can be disabled
  - Possible log levels: Debug, Info, Error, Disabled
