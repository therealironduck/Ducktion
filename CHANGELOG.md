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
  - Added method `Register<T>(Instance)` to register a type with a singleton instance
  - Added method `Register(Type, Instance)` to register a type with a singleton instance
  - Added method `Register<T>(Callback)` to register a type with a callback
  - Added method `Register(Type, Callback)` to register a type with a callback
  - Added method `Override<TKey, TService>` to override any service with a specific implementation
  - Added method `Override(KeyType, ServiceType)` to override any service with a specific implementation
  - Added method `Override<T>(Instance)` to override any service with a specific singleton instance
  - Added method `Override(Type, Instance)` to override any service with a specific singleton instance
  - Added method `Override<T>(Callback)` to override any service with a specific callback
  - Added method `Override(Type, Callback)` to override any service with a specific callback
  - Added method `Resolve<T>` to resolve a service
  - Added method `Resolve(Type)` to resolve a service
  - Added method `Clear` to remove any registered service and singleton instance
  - Added method `ResetSingletons` to remove any singleton instance
- The container can automatically resolve dependencies, even if they are not registered
  - This option can be toggled in the configuration
  - Optionally it can be configured if auto resolved services are stored as singletons
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
