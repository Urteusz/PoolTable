using System;
using System.Windows;
using System.Windows.Threading;
using ModelView;

namespace Presentation
{
    public partial class MainWindow : Window
    {
        private MainViewModel view;

        public MainWindow()
        {
            InitializeComponent();
            view = new MainViewModel();
            DataContext = view;
        }

    }
}