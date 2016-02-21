﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class InstallAssetTask : UrlQueueableTask
    {
        private readonly IParkitect _parkitect;
        private readonly IRemoteAssetRepository _remoteAssetRepository;
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IWebsite _website;

        public InstallAssetTask(IParkitect parkitect, IWebsite website, IRemoteAssetRepository remoteAssetRepository, IQueueableTaskManager queueableTaskManager)
        {
            _parkitect = parkitect;
            _website = website;
            _remoteAssetRepository = remoteAssetRepository;
            _queueableTaskManager = queueableTaskManager;

            Name = "Install asset";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            // Cast the data instance to InstallUrlAction.
            var data = Data as InstallUrlAction;
            if (data == null)
            {
                UpdateStatus("Invalid URL supplied.", 100, TaskStatus.Canceled);
                return;
            }

            // Fetch asset information from the PN API.
            UpdateStatus("Fetching asset information...", 0, TaskStatus.Running);

            var assetData = await _website.API.GetAsset(data.Id);

            // Download the asset trough the RemoveAssetRepository.
            UpdateStatus($"Installing {assetData.Type.ToString().ToLower()} '{assetData.Name}'...", 15,
                TaskStatus.Running);
            var downloadedAsset = await _remoteAssetRepository.DownloadAsset(assetData);

            // Store the asset in in the appropriate location.
            var asset = await _parkitect.Assets.StoreAsset(downloadedAsset);

            // If the downloaded asset is a mod, add a "compile mod" task to the queue;
            var modAsset = asset as IModAsset;
            if (modAsset != null)
                _queueableTaskManager.Add(ObjectFactory.Container.With(modAsset).GetInstance<CompileModTask>());

            // Update the status of the task.
            UpdateStatus($"Installed {assetData.Type.ToString().ToLower()} '{assetData.Name}'.", 100, TaskStatus.Finished);
        }

        #endregion
    }
}