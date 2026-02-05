using Ribbon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Ribbon.Interfaces;

public interface IRibbon
{
    Collection<CommandModel> Commands { get; } 
}
