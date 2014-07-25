using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for HttpStatusWindow.xaml
    /// </summary>
    public partial class HttpStatusWindow : Window
    {
        public HttpStatusWindow(string httpErrorTextBlock_text)
        {
            InitializeComponent();
            this.httpErrorTextBlock.Text = httpErrorTextBlock_text;
        }
    }
}
