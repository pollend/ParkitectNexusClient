// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.



using System;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;
using System.Linq;
using ParkitectNexus.Client.Base.Utilities;

namespace ParkitectNexus.Client.Base.Components
{
    public class AssetSidebarContainer : VBox
    {
        private string [] _requiredAssets;
        protected readonly IParkitect _parkitect;
        protected readonly SidebarContainer _sidebarContainer;
        protected readonly IWebsite _website;
        protected readonly ILogger _logger;
        public AssetSidebarContainer(IAsset asset,ILogger logger,IParkitect parkitect,SidebarContainer sideBarContainer,IWebsite website)
        {
            _logger = logger;
            _website = website;
            _sidebarContainer = sideBarContainer;
            _parkitect = parkitect;
            GetRequiredMods ();

            PopulateViewBoxWithTitle(this, asset);
            PopulateViewBoxWithImage(this, asset);
            PopulateViewBoxWithButtons(this, asset);
        }


        protected virtual void PopulateViewBoxWithTitle(VBox vBox, IAsset asset)
        {
            var title = new Label(asset.Name)
            {
                Font = Xwt.Drawing.Font.SystemFont.WithSize(17.5).WithWeight(Xwt.Drawing.FontWeight.Bold)
            };
            vBox.PackStart(title);
        }

        protected virtual void PopulateViewBoxWithImage(VBox vBox, IAsset asset)
        {
            try
            {
                var image = asset.GetImage ();

                if (image != null)
                {
                    
                    var imageView = new ImageView(image.ToXwtImage ().WithSize(250));
                    vBox.PackStart(imageView);
                }
            }
            catch (Exception e)
            {
                _logger.WriteException(e);
            }
        }


        private async void GetRequiredMods ()
        {
            try {
                _requiredAssets = await _website.API.GetRequiredModIdentifiers ();
            } catch {
            }
        }

        protected virtual void PopulateViewBoxWithButtons(VBox vBox, IAsset asset)
        {
            var canDelete = _requiredAssets == null || !_requiredAssets.Contains(asset.Id);

            var deleteButton = new Button("Delete") { Sensitive = canDelete };

            deleteButton.Clicked += (sender, args) =>
            {
                if (
                    MessageDialog.AskQuestion(
                        $"Are you sure you wish to delete \"{asset.Name}\"? This action cannot be undone!", 0,
                        Command.No,
                        Command.Yes) == Command.Yes)
                {
                    _parkitect.Assets.DeleteAsset(asset);
                    _sidebarContainer.Clear ();

                    //MainView.ShowSidebarWidget(null, null);
                }
            };
            vBox.PackStart(deleteButton);
        }


    }
}