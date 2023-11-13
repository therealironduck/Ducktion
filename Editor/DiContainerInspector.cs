using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheRealIronDuck.Ducktion.Editor
{
    /// <summary>
    /// This is the custom inspector for the DIContainer. We use this to render
    /// a nicer inspector for the container and display helpful information.
    /// </summary>
    [CustomEditor(typeof(DiContainer))]
    public class DiContainerInspector : UnityEditor.Editor
    {
        #region CONSTS

        /// <summary>
        /// Default spacing between the different boxes.
        /// </summary>
        private const int BoxSpacing = 15;

        /// <summary>
        /// Default padding within each box.
        /// </summary>
        private const int BoxPadding = 10;

        #endregion

        #region OVERRIDDEN METHODS

        /// <summary>
        /// This method actually renders the inspector. We just all out to different methods.
        /// </summary>
        /// <returns>The inspector</returns>
        public override VisualElement CreateInspectorGUI()
        {
            var inspector = new VisualElement();

            inspector.Add(BuildGeneralOptionsBox());
            inspector.Add(BuildAutoResolveBox());
            inspector.Add(BuildDefaultsBox());
            inspector.Add(BuildConfiguratorsBox());

            return inspector;
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Render the box which contains the general options.
        /// - DontDestroyOnLoad
        /// - LogLevel
        /// </summary>
        /// <returns>The general options box</returns>
        private Box BuildGeneralOptionsBox()
        {
            var generalOptions = CreateBox("General options");
            generalOptions.Add(new PropertyField(serializedObject.FindProperty("dontDestroyOnLoad")));
            generalOptions.Add(new PropertyField(serializedObject.FindProperty("logLevel")));
            return generalOptions;
        }

        /// <summary>
        /// Render the box which contains the auto resolve options.
        /// - EnableAutoResolve
        /// - AutoResolveSingletonMode (only visible when auto resolve is enabled)
        /// </summary>
        /// <returns>The auto resolve box</returns>
        private Box BuildAutoResolveBox()
        {
            var autoResolve = CreateBox("Auto Resolve");

            // HELP BOX
            autoResolve.Add(new HelpBox(
                "When auto resolving is disabled, you need to manually register all your dependencies. " +
                "Please read the documentation for more information.",
                HelpBoxMessageType.Info
            )
            {
                style = { marginBottom = BoxPadding }
            });
            // HELP BOX END

            var enableProperty = serializedObject.FindProperty("enableAutoResolve");
            autoResolve.Add(new PropertyField(enableProperty));

            var autoResolveOptions = new VisualElement
            {
                style = { display = enableProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None }
            };

            autoResolveOptions.Add(
                new PropertyField(serializedObject.FindProperty("autoResolveSingletonMode"), "Singleton Mode")
            );

            autoResolve.TrackPropertyValue(serializedObject.FindProperty("enableAutoResolve"),
                _ =>
                {
                    autoResolveOptions.style.display = enableProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
                }
            );

            autoResolve.Add(autoResolveOptions);

            return autoResolve;
        }

        /// <summary>
        /// Render the box which contains the default options.
        /// - DefaultLazyMode
        /// </summary>
        /// <returns>The defaults box</returns>
        private Box BuildDefaultsBox()
        {
            var defaults = CreateBox("Defaults");

            // HELP BOX
            defaults.Add(new HelpBox(
                "You can override any setting here when registering a service. These are the default values " +
                "which will be used when registering a service without specifying any of these options.",
                HelpBoxMessageType.Info
            )
            {
                style = { marginBottom = BoxPadding }
            });
            // HELP BOX END

            defaults.Add(new PropertyField(serializedObject.FindProperty("defaultLazyMode")));

            return defaults;
        }

        /// <summary>
        /// Render the box which contains the configurators.
        /// - DefaultConfigurators
        /// </summary>
        /// <returns>The configurators box</returns>
        private Box BuildConfiguratorsBox()
        {
            var configurators = CreateBox("Configurators");

            // HELP BOX
            configurators.Add(new HelpBox(
                "Here you may add all configurators which will automatically run on startup. Alternatively, " +
                "you can manually register them using `AddConfigurator` method.",
                HelpBoxMessageType.Info
            )
            {
                style = { marginBottom = BoxPadding }
            });
            // HELP BOX END

            var defaultConfiguratorsProperty = serializedObject.FindProperty("defaultConfigurators");
            defaultConfiguratorsProperty.isExpanded = true;
            configurators.Add(new PropertyField(defaultConfiguratorsProperty));

            return configurators;
        }

        #endregion

        #region PRIVATE STATIC METHODS

        /// <summary>
        /// Small little helper method to create a box with a title.
        /// </summary>
        /// <param name="title">The title of the box</param>
        /// <returns>The generated box</returns>
        private static Box CreateBox(string title)
        {
            var box = new Box
            {
                style =
                {
                    marginTop = BoxSpacing,
                    paddingBottom = BoxPadding,
                    paddingLeft = BoxPadding,
                    paddingRight = BoxPadding,
                    paddingTop = BoxPadding
                }
            };

            var titleLabel = new Label(title)
            {
                style =
                {
                    marginBottom = BoxPadding,
                    fontSize = 16,
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold)
                }
            };
            box.Add(titleLabel);

            return box;
        }

        #endregion
    }
}