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
  - Added method `Override<TKey, TService>` to override any service with a specific implementation
  - Added method `Resolve<T>` to resolve a service
  - Added method `Resolve(Type)` to resolve a service
  - Added method `Clear` to remove any registered service and singleton instance
  - Added method `Reset` to remove any singleton instance
- Handle security methods and exceptions
  - When a service is abstract / interface / enum
  - When a service is already registered
  - When a service couldn't be resolved
  - When a circular dependency is detected
