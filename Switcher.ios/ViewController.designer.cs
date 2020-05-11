// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Switcher.ios
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch LightSwitcher { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UISwitchController { get; set; }

        [Action ("UISwitchController_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UISwitchController_OnTouch (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (LightSwitcher != null) {
                LightSwitcher.Dispose ();
                LightSwitcher = null;
            }

            if (UISwitchController != null) {
                UISwitchController.Dispose ();
                UISwitchController = null;
            }
        }
    }
}