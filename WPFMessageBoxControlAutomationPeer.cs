using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows;

namespace MessageBoxUtils
{
    class WPFMessageBoxControlAutomationPeer : UIElementAutomationPeer
    {
        public WPFMessageBoxControlAutomationPeer( WPFMessageBoxControl owner) : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            return "WPFMessageBoxControl";
        }
    }
}
