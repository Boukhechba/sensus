﻿using SensusUI.UiProperties;

namespace SensusService.Probes
{
    public abstract class ListeningOrPollingProbe : ListeningProbe, IPollingProbe
    {
        [OnOffUiProperty("Listening Mode:", true, int.MaxValue)]
        public bool Listening
        {
            get { return Controller is ListeningProbeController; }
            set
            {
                if (value != Listening)
                {
                    ProbeController newController = value ? new ListeningProbeController(this) as ProbeController : new PollingProbeController(this) as ProbeController;

                    if (Controller.Running)
                    {
                        SensusServiceHelper.Get().Logger.Log("Restarting " + DisplayName + " as " + (value ? "listening" : "polling") + " probe.", LoggingLevel.Normal);

                        Controller.Stop();
                        Controller = newController;

                        OnPropertyChanged();

                        InitializeAndStart();
                    }
                    else
                    {
                        Controller = newController;
                        OnPropertyChanged();
                    }
                }
            }
        }

        protected override ProbeController DefaultController
        {
            get { return new PollingProbeController(this); }  // listening is often more resource intensive than polling
        }

        public abstract Datum Poll();
    }
}
