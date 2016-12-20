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
using System.Collections.Generic;
using System.Threading;
using ParkitectNexus.Client.Base.Components;
using ParkitectNexus.Client.Base.Components.Tiles;
using ParkitectNexus.Client.Base.Utilities;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Pages
{
    public class AssetScenarioPage : HBox, IPresenter , IPageView
    {
        private readonly TileView _tileView;
        private readonly SidebarContainer _sidebarContainer;

        private IParkitect _parkitect;
        private ILogger _logger;
        private IWebsite _website;
        public AssetScenarioPage(ILogger logger, IParkitect parkitect,IWebsite website)
        {
            _website = website;
            _parkitect = parkitect;
            _logger = logger;

            var sideBox = new VBox
            {
                MinWidth = 280,
                WidthRequest = 280
            };


            _sidebarContainer = new SidebarContainer();
            sideBox.PackStart(_sidebarContainer, true, true);

            _tileView = new TileView(logger, token => getBlueprintTiles(token));

            PackStart(_tileView , true);
            PackEnd(sideBox);

            parkitect.Assets.AssetAdded += OnAssetsCollectionChanged;
            parkitect.Assets.AssetRemoved += OnAssetsCollectionChanged;

        }

        private void OnAssetsCollectionChanged(object obj, AssetEventArgs e)
        {
            if (e.Asset.Type == AssetType.Scenario)
                _tileView.RefreshTiles();
        }


        private IEnumerable<Tile> getBlueprintTiles(CancellationToken token)
        {
            var tiles = new List<Tile>();

            if (_parkitect?.Assets == null)
            {
                return new Tile[0] as IEnumerable<Tile>;
            }

            try
            {
                foreach (var asset in _parkitect.Assets[AssetType.Scenario])
                {
                    token.ThrowIfCancellationRequested();

                    try
                    {
                        Image image = null;
                        try
                        {
                            image = asset.GetImage().ToXwtImage ();
                        }
                        catch (Exception e)
                        {
                            ObjectFactory.GetInstance<ILogger>().WriteException(e);
                        }
                        var tile = new Tile(image, asset.Name,
                            () =>
                            {
                            _sidebarContainer.ShowWidget(asset.Name,new AssetSidebarContainer(asset,_logger,_parkitect,_sidebarContainer,_website));
                            });
                        tiles.Add(tile);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteLine($"Failed to load asset tile for {AssetType.Scenario} {asset.Name}.");
                        _logger.WriteException(e);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.WriteLine($"Failed to load asset tiles for {AssetType.Scenario}.");
                _logger.WriteException(e);
            }
            return tiles;
        }

        public string DisplayName => "Scenarios";
        public event EventHandler DisplayNameChanged;
        public void HandleSizeUpdate(float width, float height)
        {
            this._tileView.HandleSizeUpdate(width);
        }
    }
}