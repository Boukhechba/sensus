using Android.App;
using Android.Telephony;
using SensusService;
using SensusService.Probes.Communication;
using System;

namespace Sensus.Android.Probes.Communication
{
    public class AndroidTelephonyProbe : TelephonyProbe
    {
        private TelephonyManager _telephonyManager;
        private EventHandler<string> _outgoingCallCallback;
        private AndroidTelephonyIncomingListener _incomingCallListener;

        public AndroidTelephonyProbe()
        {
            _outgoingCallCallback = (sender, outgoingNumber) =>
                {
                    StoreDatum(new TelephonyDatum(this, DateTimeOffset.UtcNow, CallState.Offhook.ToString(), outgoingNumber));
                };

            _incomingCallListener = new AndroidTelephonyIncomingListener();
            _incomingCallListener.IncomingCall += (o, incomingNumber) =>
                {
                    StoreDatum(new TelephonyDatum(this, DateTimeOffset.UtcNow, CallState.Ringing.ToString(), incomingNumber));
                };
        }

        protected override bool Initialize()
        {
            try
            {
                _telephonyManager = Application.Context.GetSystemService(global::Android.Content.Context.TelephonyService) as TelephonyManager;
                if (_telephonyManager == null)
                    throw new Exception("No telephony present.");

                return base.Initialize();
            }
            catch (Exception ex)
            {
                SensusServiceHelper.Get().Logger.Log("Failed to initialize " + GetType().FullName + ":  " + ex.Message, LoggingLevel.Normal);
                return false;
            }
        }

        public override void StartListening()
        {
            AndroidTelephonyOutgoingBroadcastReceiver.OutgoingCall += _outgoingCallCallback;
            _telephonyManager.Listen(_incomingCallListener, PhoneStateListenerFlags.CallState);
        }

        public override void StopListening()
        {
            AndroidTelephonyOutgoingBroadcastReceiver.OutgoingCall -= _outgoingCallCallback;
            _telephonyManager.Listen(_incomingCallListener, PhoneStateListenerFlags.None);
        }
    }
}