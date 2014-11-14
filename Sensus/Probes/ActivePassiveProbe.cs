﻿using Sensus.UI.Properties;

namespace Sensus.Probes
{
    public abstract class ActivePassiveProbe : ActiveProbe
    {
        private bool _passive;

        [BooleanUiProperty("Passive:", true)]
        public bool Passive
        {
            get { return _passive; }
            set
            {
                if (value != _passive)
                {
                    _passive = value;
                    OnPropertyChanged();

                    if (State == ProbeState.Started)
                        if (_passive)
                        {
                            // switch from active to passive
                            base.StopAsync();
                            App.Get().ProbeInitializer.InitializeProbe(this);
                            StartAsync();
                        }
                        else
                        {
                            // switch from passive to active
                            StopAsync();
                            App.Get().ProbeInitializer.InitializeProbe(this);
                            base.StartAsync();
                        }
                }
            }
        }

        public override void StartAsync()
        {
            if (_passive)
            {
                ChangeState(ProbeState.Initialized, ProbeState.Starting);
                StartListening();
                ChangeState(ProbeState.Starting, ProbeState.Started);
            }
            else
                base.StartAsync();
        }

        public override void StopAsync()
        {
            if (_passive)
            {
                ChangeState(ProbeState.Started, ProbeState.Stopping);
                StopListening();
                ChangeState(ProbeState.Stopping, ProbeState.Stopped);
            }
            else
                base.StopAsync();
        }

        protected abstract void StartListening();

        protected abstract void StopListening();
    }
}
