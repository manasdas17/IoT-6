using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ST.IoT.Demos.Utils;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LEDPanelTouch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SolidColorBrush _offBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush _onBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x7f, 0x7f, 0xff));

        private Adafrut8x8LEDBackpack _backpack;

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var width = this.ActualWidth;
            var height = this.ActualHeight;

            var min = Math.Min(width, height);

            var cwidth = min/8;
            for (var r = 0; r < 8; r++)
            {
                for (var c = 0; c < 8; c++)
                {
                    var ellipse = new Ellipse() {Width = cwidth, Height = cwidth};
                    grid.Children.Add(ellipse);
                    ellipse.Tag = r*8 + c;
                    ellipse.Stroke = new SolidColorBrush(Colors.White);
                    ellipse.Fill = _offBrush;
                    ellipse.PointerPressed += Ellipse_PointerPressed;
                    ellipse.SetValue(Grid.RowProperty, r);
                    ellipse.SetValue(Grid.ColumnProperty, c);
                }
            }

            _backpack = new Adafrut8x8LEDBackpack();
            await _backpack.initializeAsync();
        }

        private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var ellipse = sender as Ellipse;
            var on = ellipse.Fill == _onBrush;
            ellipse.Fill = on ? _offBrush : _onBrush;

            var index = (int)ellipse.Tag;
            var x = index%8;
            var y = index/8;

            var color = ellipse.Fill == _onBrush;

            _backpack.setPixel(x, y, color);
        }
    }
}