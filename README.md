<img src="/logo.png?raw=true" alt="Logo" width="150"/><br>

# Ducktion

A simple, flexible dependency injection solution for Unity!

This package is compatible and tested with Unity 2022.3 LTS and Unity 6.0 LTS.

## Features

- Dependency Injection container which can be used to register and resolve services
- Services can be registered with IDs
- Lazy and non-lazy services
- Singleton and transient services
- Auto Resolving of dependencies
- Dynamic instantiation of services with callbacks
- Resolving of dependencies using the `[Resolve]` attribute

## Installation

This package can be installed via three ways:

### Via package manager

You can download this package using the Unity Package Manager. This will ensure that you always get the newest updates
when they're out. Also you can optionally opt into the development channel to receive bleeding-edge updates.

You can follow these steps to install
it: [Open Documentation](https://ducktion.docs.jkniest.de/installation.html#unity-package-manager)

### Via AssetStore

Alternatively you can also install this package via the AssetStore. The AssetStore version may take a little bit longer
to be updated than the direct package approach.

You can find the package on the assetstore here:

> We are currently in the process of submitting Ducktion to the Unity Asset Store. Once the package is available on the
> Unity Asset Store, we will update this section with instructions on how to install the package using the Unity Asset
> Store.

### Manually download

Not recommended, but possible: You can also download this repository completely and copy it into your project directly.
This way you won't get any updates automatically and are prone to errors, since the files can be modified freely, but
you also don't have any dependency to Github or Unity Asset Store.

You can follow these steps to install
it: [Open Documentation](https://ducktion.docs.jkniest.de/installation.html#manual-installation)

## Usage/Examples

Please have a look at the documentation down below for full examples. This is just a little quick start

```c#
public class MyAwesomeService
{
    /// <summary>
    /// This class requires another class as a dependency!
    /// </summary>
    public MyAwesomeService(AnotherService another)
    {

    }

    public class AnotherService {}

    public class SomeWhereInMyGame()
    {
        public void Start()
        {
            /*
             * This will automatically create a singleton instance of
             * "MyAwesomeService" with all dependencies resolved!
             * 
             * You can modify the behaviour further using Configurators
             * or the configuration in the container itself.
             *
             * @see https://ducktion.docs.jkniest.de
             */
            var awesomeService = Ducktion.singleton.Resolve<MyAwesomeService>();
        }
    }
}
```

## [Documentation](https://ducktion.docs.jkniest.de)

[Documentation](https://ducktion.docs.jkniest.de)

## Contributing

Contributions are always welcome!

See [CONTRIBUTING.md](CONTRIBUTING.md) for ways to get started.

## Authors

- [@jkniest](https://www.github.com/jkniest)
- [All Contributors](../../contributors)

## Security

If you discover any security related issues, please email mail@jkniest.de instead of using the issue tracker.

## License

[MIT](https://choosealicense.com/licenses/mit/)

