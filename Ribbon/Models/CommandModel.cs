using System.Windows;
using System.Windows.Input;

namespace Ribbon.Models;

public class CommandModel
{
    public string Header { get; set; }

    public ICommand Command { get; set; }

    public ICommand CommandParameter { get; set; }
}
