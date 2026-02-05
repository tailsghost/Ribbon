using Ribbon.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ribbon.Interfaces;

public interface IRibbonCommandStrategy
{
    void OnButtonAdding(IRibbon ribbon,RibbonButtonViewModel button);
}
