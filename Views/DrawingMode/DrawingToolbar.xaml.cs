using System.Windows;

namespace Usely.Views
{
    public partial class DrawingToolbar : Window
    {
        public DrawingToolbar()
        {
            InitializeComponent();
        }

        private void CursorMode_Click(object sender, RoutedEventArgs e)
        {
            // Set cursor mode
        }

        private void DrawMode_Click(object sender, RoutedEventArgs e)
        {
            // Set draw mode
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
