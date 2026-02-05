using Ribbon.Interfaces;
using Ribbon.Models;
using Ribbon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ribbon.Controls
{
    /// <summary>
    /// Логика взаимодействия для RibbonControl.xaml
    /// </summary>
    [ContentProperty(nameof(Commands))]
    public partial class RibbonControl : UserControl, IDisposable, IRibbon
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

        public static readonly DependencyProperty CommandStrategyProperty =
            DependencyProperty.Register(
                nameof(CommandStrategy),
                typeof(IRibbonCommandStrategy),
                typeof(RibbonControl),
                new PropertyMetadata(null));

        public IRibbonCommandStrategy? CommandStrategy
        {
            get => (IRibbonCommandStrategy?)GetValue(CommandStrategyProperty);
            set => SetValue(CommandStrategyProperty, value);
        }

        public static readonly DependencyProperty CommandsProperty =
        DependencyProperty.Register(
            nameof(Commands),
            typeof(Collection<CommandModel>),
            typeof(RibbonControl),
            new PropertyMetadata(new Collection<CommandModel>()));

        public Collection<CommandModel> Commands
        {
            get
            {
                var col = (Collection<CommandModel>?)GetValue(CommandsProperty);
                if (col == null)
                {
                    col = [];
                    SetValue(CommandsProperty, col);
                }
                return col;
            }
            set => SetValue(CommandsProperty, value);
        }

        public RibbonControl(RibbonViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
        }

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

        public void Dispose()
        {
            ViewModel = null;
        }
    }
}
