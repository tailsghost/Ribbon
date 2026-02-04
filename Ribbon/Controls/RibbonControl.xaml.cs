using Ribbon.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ribbon.Controls
{
    /// <summary>
    /// Логика взаимодействия для RibbonControl.xaml
    /// </summary>
    public partial class RibbonControl : UserControl
    {
        public RibbonViewModel ViewModel
        {
            get => field;
            set
            {
                DataContextChanged -= RibbonControl_DataContextChanged;
                field = value;
                DataContext = this;
            }
        }
        public RibbonControl(RibbonViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
        }

        private bool _init = false;

        public RibbonControl()
        {
            DataContextChanged += RibbonControl_DataContextChanged;
            InitializeComponent();
        }

        private void RibbonControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is RibbonViewModel vm)
            {
                ViewModel = vm;
            }
        }

        private void TabButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb && tb.DataContext is RibbonTabViewModel tab)
            {
                if (ReferenceEquals(ViewModel.SelectedTab, tab)) return;
                ViewModel.SelectedTab = tab;
            }
        }
    }
}
