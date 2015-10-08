﻿// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class MenuUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexusWebsite _parkitectNexusWebsite;
        private readonly ParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;

        public MenuUserControl(Parkitect parkitect, ParkitectNexusWebsite parkitectNexusWebsite,
            ParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));

            _parkitect = parkitect;
            _parkitectNexusWebsite = parkitectNexusWebsite;
            _parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;

            InitializeComponent();
        }

        private void manageModsButton_Click(object sender, EventArgs e)
        {
            WizardForm.Attach(new ManageModsUserControl(this, _parkitect, _parkitectOnlineAssetRepository));
        }

        private void launchParkitectButton_Click(object sender, EventArgs e)
        {
            WizardForm.Hide();
            try
            {
                _parkitect.LaunchWithMods();
            }
            catch (Exception ex)
            {
                Log.WriteLine("Launching failed in an unusual way.", LogLevel.Fatal);
                Log.WriteException(ex);

                using (var focus = new FocusForm())
                {
                    MessageBox.Show(focus,
                        $"Launching the game with mods failed.\n\nThe error has been logged to:\n{Log.LoggingPath}",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            WizardForm.Close();
        }

        private void visitParkitectNexusButton_Click(object sender, EventArgs e)
        {
            _parkitectNexusWebsite.Launch();
            WizardForm.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            WizardForm.Close();
        }
    }
}