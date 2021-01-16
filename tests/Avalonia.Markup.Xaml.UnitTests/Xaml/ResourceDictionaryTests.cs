using System;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.UnitTests;
using Xunit;

namespace Avalonia.Markup.Xaml.UnitTests.Xaml
{
    public class ResourceDictionaryTests : XamlTestBase
    {
        [Fact]
        public void StaticResource_Works_In_ResourceDictionary()
        {
            using (StyledWindow())
            {
                var xaml = @"
<ResourceDictionary xmlns='https://github.com/avaloniaui'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
  <Color x:Key='Red'>Red</Color>
  <SolidColorBrush x:Key='RedBrush' Color='{StaticResource Red}'/>
</ResourceDictionary>";
                var resources = (ResourceDictionary)AvaloniaRuntimeXamlLoader.Load(xaml);
                var brush = (SolidColorBrush)resources["RedBrush"];

                Assert.Equal(Colors.Red, brush.Color);
            }
        }

        [Fact]
        public void DynamicResource_Finds_Resource_In_Parent_Dictionary()
        {
            var dictionaryXaml = @"
<ResourceDictionary xmlns='https://github.com/avaloniaui'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
  <SolidColorBrush x:Key='RedBrush' Color='{DynamicResource Red}'/>
</ResourceDictionary>";

            using (StyledWindow(assets: ("test:dict.xaml", dictionaryXaml)))
            {
                var xaml = @"
<Window xmlns='https://github.com/avaloniaui'
        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
    <Window.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceInclude Source='test:dict.xaml'/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
        <Color x:Key='Red'>Red</Color>
    </Window.Resources>
    <Button Name='button' Background='{DynamicResource RedBrush}'/>
</Window>";

                var window = (Window)AvaloniaRuntimeXamlLoader.Load(xaml);
                var button = window.FindControl<Button>("button");

                var brush = Assert.IsType<SolidColorBrush>(button.Background);
                Assert.Equal(Colors.Red, brush.Color);

                window.Resources["Red"] = Colors.Green;

                Assert.Equal(Colors.Green, brush.Color);
            }
        }

        [Fact]
        public void DynamicResource_Shared_Works_Correctly()
        {
            using (StyledWindow())
            {
                var xaml = @"
<ResourceDictionary xmlns='https://github.com/avaloniaui'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
  <x:String x:Key='s1'>A</x:String>
  <x:String x:Key='s2' x:Shared='True'>B</x:String>
  <x:String x:Key='s3' x:Shared='True'>C</x:String>
  <x:Object x:Key='o1' />
  <x:Object x:Key='o2' x:Shared='True' />
  <x:Object x:Key='o3' x:Shared='False' />
  <SolidColorBrush x:Key='b1' Color='Red' />
  <SolidColorBrush x:Key='b2' x:Shared='True' Color='Red' />
  <SolidColorBrush x:Key='b3' x:Shared='False' Color='Red' />
</ResourceDictionary>";
                var resources = (ResourceDictionary)AvaloniaRuntimeXamlLoader.Load(xaml);

                Assert.True(Object.ReferenceEquals(resources["s1"], resources["s1"]));
                Assert.True(Object.ReferenceEquals(resources["s2"], resources["s2"]));
                // WPF actually creates a new string instance
                //Assert.False(Object.ReferenceEquals(resources["s3"], resources["s3"]));

                Assert.True(Object.ReferenceEquals(resources["o1"], resources["o1"]));
                Assert.True(Object.ReferenceEquals(resources["o2"], resources["o2"]));
                Assert.False(Object.ReferenceEquals(resources["o3"], resources["o3"]));

                Assert.True(Object.ReferenceEquals(resources["b1"], resources["b1"]));
                Assert.True(Object.ReferenceEquals(resources["b2"], resources["b2"]));
                Assert.False(Object.ReferenceEquals(resources["b3"], resources["b3"]));
            }
        }

        private IDisposable StyledWindow(params (string, string)[] assets)
        {
            var services = TestServices.StyledWindow.With(
                assetLoader: new MockAssetLoader(assets),
                theme: () => new Styles
                {
                    WindowStyle(),
                });

            return UnitTestApplication.Start(services);
        }

        private Style WindowStyle()
        {
            return new Style(x => x.OfType<Window>())
            {
                Setters =
                {
                    new Setter(
                        Window.TemplateProperty,
                        new FuncControlTemplate<Window>((x, scope) =>
                            new ContentPresenter
                            {
                                Name = "PART_ContentPresenter",
                                [!ContentPresenter.ContentProperty] = x[!Window.ContentProperty],
                            }.RegisterInNameScope(scope)))
                }
            };
        }
    }
}
