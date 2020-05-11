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
            mqttClient.OnTopicRecieved += MqttClient_OnTopicRecieved;
            await mqttClient.ConnectAsync();
            await mqttClient.SubscribeAsync("light/state");
        }


        private void MqttClient_OnTopicRecieved(object sender, string e)
        {
            LightSwitcher.BeginInvokeOnMainThread(() =>
            {
                LightSwitcher.SetState(e == "on", true);
            });
        }

        async partial void UISwitchController_OnTouch(UIButton sender)
        {
            await mqttClient.PublishAsync("light", LightSwitcher.On ? "off" : "on");
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}