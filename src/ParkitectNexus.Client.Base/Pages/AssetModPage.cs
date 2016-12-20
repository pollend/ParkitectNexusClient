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
using ParkitectNexus.Client.Base.Main;
using ParkitectNexus.Client.Base.Utilities;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Pages
{
    public class AssetModPage : HBox, IPresenter, IPageView
    {
        private readonly TileView _tileView;
        private readonly SidebarContainer _sidebarContainer;

        private IParkitect _parkitect;
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IWebsite _website;
        private ILogger _logger;

        public event EventHandler DisplayNameChanged;
        private MainView _mainView;

        public AssetModPage (ILogger logger,IPresenter parent, IParkitect parkitect, IQueueableTaskManager queueableTaskManager, IWebsite website)
        {
            _mainView = (ParkitectNexus.Client.Base.Main.MainView)parent;
            _parkitect = parkitect;
            _queueableTaskManager = queueableTaskManager;
            _website = website;
            _logger = logger;

            var sideBox = new VBox {
                MinWidth = 280,
                WidthRequest = 280
            };


            _sidebarContainer = new SidebarContainer ();
            sideBox.PackStart (_sidebarContainer, true, true);

            _tileView = new TileView (logger, token => getModTiles (token));

            PackStart (_tileView, true);
            PackEnd (sideBox);

            parkitect.Assets.AssetAdded += (o, args) => OnAssetsCollectionChanged(o, args);
            parkitect.Assets.AssetRemoved += (o, args) => OnAssetsCollectionChanged(o, args);

        }

        private void OnAssetsCollectionChanged (object obj, AssetEventArgs e)
        {
            if (e.Asset.Type == AssetType.Mod)
                _tileView.RefreshTiles ();
        }


        private IEnumerable<Tile> getModTiles (CancellationToken token)
        {
            var tiles = new List<Tile> ();

            if (_parkitect?.Assets == null) {
                return new Tile [0] as IEnumerable<Tile>;
            }

            try {
                foreach (var asset in _parkitect.Assets [AssetType.Mod]) {
                    token.ThrowIfCancellationRequested ();

                    try {
                        Image image = null;
                        try {
                            image = asset.GetImage ().ToXwtImage ();
                        } catch (Exception e) {
                            ObjectFactory.GetInstance<ILogger> ().WriteException (e);
                        }
                        var tile = new Tile (image, asset.Name,
                            () => {
                            _sidebarContainer.ShowWidget (asset.Name, new AssetSidebarModContainer (asset,_logger,_mainView, _queueableTaskManager,_website,_parkitect,_sidebarContainer));
                            });
                        tiles.Add (tile);
                    } catch (Exception e) {
                        _logger.WriteLine ($"Failed to load asset tile for {AssetType.Mod} {asset.Name}.");
                        _logger.WriteException (e);
                    }
                }
            } catch (Exception e) {
                _logger.WriteLine ($"Failed to load asset tiles for {AssetType.Mod}.");
                _logger.WriteException (e);
            }
            return tiles;
        }
        public void HandleSizeUpdate (float width, float height)
        {
            this._tileView.HandleSizeUpdate (width);
        }
        public string DisplayName => "Mods";
    }
}
