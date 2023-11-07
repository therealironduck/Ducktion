using UnityEditor;
using UnityEditor.Sprites;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheRealIronDuck.Ducktion.Editor
{
    [CustomEditor(typeof(DiContainer))]
    public class DiContainerInspector : UnityEditor.Editor
    {
        #region CONSTS

        private const int BoxSpacing = 15;
        private const int BoxPadding = 10;

        #endregion

        #region OVERRIDDEN METHODS

        public override VisualElement CreateInspectorGUI()
        {
            var inspector = new VisualElement();

            inspector.Add(BuildGeneralOptionsBox());
            inspector.Add(BuildAutoResolveBox());

            return inspector;
        }

        #endregion

        #region PRIVATE METHODS

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

        private Box BuildGeneralOptionsBox()
        {
            var generalOptions = CreateBox("General options");
            generalOptions.Add(new PropertyField(serializedObject.FindProperty("dontDestroyOnLoad")));
            generalOptions.Add(new PropertyField(serializedObject.FindProperty("logLevel")));
            return generalOptions;
        }

        #endregion

        #region PRIVATE STATIC METHODS

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