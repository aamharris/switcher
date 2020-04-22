using Foundation;
using Switcher.Core;
using System;
using UIKit;

namespace Switcher.ios
{
    public partial class ViewController : UIViewController
    {
        private MqttClient mqttClient;


        public ViewController(IntPtr handle) : base(handle)
        {
            mqttClient = new MqttClient();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            await mqttClient.ConnectAsync();
            mqttClient.OnTopicRecieved += MqttClient_OnTopicRecieved;
            await mqttClient.SubscribeAsync("light/state");

            LightSwitcher.ValueChanged += LightSwitcher_ValueChanged;

        }

        private void MqttClient_OnTopicRecieved(object sender, string e)
        {
            if (e == "on")
            {
                LightSwitcher.BeginInvokeOnMainThread(() =>
                {
                    LightSwitcher.SetState(true, true);
                });
            }
            else
            {
                LightSwitcher.BeginInvokeOnMainThread(() =>
                {
                    LightSwitcher.SetState(false, true);
                });
            }
        }

        private async void LightSwitcher_ValueChanged(object sender, EventArgs e)
        {
            bool isOn = LightSwitcher.On;
            await mqttClient.PublishAsync("light", isOn ? "on" : "off");

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}