
> __Info: This project is still WIP and not ready to be used on production right now!__

# Ducktion

A simple, flexible dependency injection solution for Unity!

This package is compatible and tested with Unity 2022.3 LTS.
## Features

- Nothing yet :-)


## Installation

This package can be installed via three ways:

### Via package manager
You can download this package using the Unity Package Manager. This will ensure that you always get the newest updates when they're out. Also you can optionally opt into the development channel to receive bleeding-edge updates.

You can follow these steps to install it:

> __Info: Not ready yet__


### Via AssetStore
Alternatively you can also install this package via the AssetStore. The AssetStore version may take a little bit longer to be updated than the direct package approach.

You can find the package on the assetstore here:

> __Info: Not ready yet__


### Manually download
Not recommended, but possible: You can also download this repository completely and copy it into your project directly. This way you won't get any updates automatically and are prone to errors, since the files can be modified freely, but you also don't have any dependency to Github or Unity Asset Store.
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

