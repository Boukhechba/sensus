﻿using SensusService;
using SensusService.Probes;
using System;
using Xamarin.Forms;
using System.Linq;

namespace SensusUI
{
    public class SensusNavigationPage : NavigationPage
    {
        public SensusNavigationPage()
            : base(new MainPage())
        {
            #region main page
            MainPage.ProtocolsTapped += async (o, e) =>
                {
                    await PushAsync(new ProtocolsPage());
                };

            MainPage.OptionsTapped += async (o, e) =>
                {
                    await PushAsync(new OptionsPage(UiBoundSensusServiceHelper.Get()));
                };
            #endregion

            #region options page
            OptionsPage.ViewLogTapped += async (o, e) =>
                {
                    await PushAsync(new ViewTextLinesPage("Sensus Log", UiBoundSensusServiceHelper.Get().Logger.Read(int.MaxValue), () => UiBoundSensusServiceHelper.Get().Logger.Clear()));
                };
            #endregion

            #region protocols page
            ProtocolsPage.EditProtocol += async (o, e) =>
                {
                    await PushAsync(new ProtocolPage(o as Protocol));
                };
            #endregion

            #region protocol page
            ProtocolPage.EditDataStoreTapped += async (o, e) =>
                {
                    if (e.DataStore != null)
                        await PushAsync(new DataStorePage(e));
                };

            ProtocolPage.CreateDataStoreTapped += async (o, e) =>
                {
                    await PushAsync(new CreateDataStorePage(e));
                };

            ProtocolPage.ProbeTapped += async (o, e) =>
                {
                    await PushAsync(new ProbePage(e.Item as Probe));
                };

            ProtocolPage.DisplayProtocolReport += async (o, report) =>
                {
                    await PushAsync(new ViewTextLinesPage("Protocol Report", report.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(), null));
                };
            #endregion

            #region create data store page
            CreateDataStorePage.CreateTapped += async (o, e) =>
                {
                    await PopAsync();
                    await PushAsync(new DataStorePage(e));
                };
            #endregion

            #region data store page
            DataStorePage.OkTapped += async (o, e) =>
                {
                    await PopAsync();
                };
            #endregion
        }
    }
}
