

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ParkitectNexus.Client.Base.Components.Tiles;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Web;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base
{
    public class MenuPage : HBox, IPresenter, IPageView
    {
        private readonly TileView _tileView;
        private readonly IWebsite _website;
        private readonly IParkitect _parkitect;

        public MenuPage (Data.Utilities.ILogger logger,IWebsite webiste,IParkitect parkitect)
        {
            _parkitect = parkitect;
            _website = webiste;
            _tileView = new TileView (logger, token => getMenuTiles (token));

            PackStart(_tileView,true,true);


        }


        private IEnumerable<Tile> getMenuTiles (CancellationToken token)
        {
            var tiles = new List<Tile> ();
            tiles.Add( CreateTile ("Visit ParkitectNexus", App.Images ["parkitectnexus_logo-64x64.png"],
                                   Color.FromBytes (0xf3, 0x77, 0x35), _website.Launch));
            

            if (Data.Utilities.OperatingSystem.Detect () == SupportedOperatingSystem.Linux)
                tiles.Add(
                    CreateTile ("Download URL", App.Images ["appbar.browser.wire.png"], Color.FromBytes (0xf3, 0x77, 0x35),
                        () => {
                            var entry = new TextEntry ();

                            var box = new HBox ();
                            box.PackStart (new Label ("URL:"));
                            box.PackStart (entry, true, true);

                            var dialog = new Dialog {
                                Width = 300,
                                Icon = ParentWindow.Icon,
                                Title = "Enter URL to download",
                                Content = box
                            };
                            dialog.Buttons.Add (new DialogButton (Command.Cancel));
                            dialog.Buttons.Add (new DialogButton (Command.Ok));


                            var result = dialog.Run (ParentWindow);

                            NexusUrl url;
                            if (result.Label.ToLower () == "ok" && NexusUrl.TryParse (entry.Text, out url))
                                ObjectFactory.GetInstance<IApp> ().HandleUrl (url);
                            dialog.Dispose ();
                    }));


            tiles.Add(CreateTile ("Launch Parkitect", App.Images ["parkitect_logo.png"], Color.FromBytes (45, 137, 239),
                                  () => { _parkitect.Launch (); }));

            tiles.Add(CreateTile ("Help", App.Images ["appbar.information.png"], Color.FromBytes (45, 137, 239), () => {
                // Temporary help solution.
                Process.Start (
                    "https://parkitectnexus.com/forum/2/parkitectnexus-website-client/70/troubleshooting-mods-and-client");
            }));

            tiles.Add ( CreateTile ("Donate!",App.Images ["appbar.thumbs.up.png"], Color.FromBytes (45, 137, 239), () => {
                if (MessageDialog.AskQuestion ("Maintaining this client and adding new features takes a lot of time.\n" +
                                              "If you appreciate our work, please consider sending a donation our way!\n" +
                                              "All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
                                              "\nSelect Yes to visit PayPal and send a donation.", 1, Command.No,
                    Command.Yes) == Command.Yes) {
                    Process.Start ("https://paypal.me/ikkentim");
                }
            }));

            return tiles;
        }

        public Tile CreateTile (string text, Image image, Color backgroundColor, Action clickedAction)
        {
            return new Tile (image, text, clickedAction) { BackgroundColor = backgroundColor };
        }

        public string DisplayName => "Menu";

        public event EventHandler DisplayNameChanged;

        public void HandleSizeUpdate (float width, float height)
        {
        }
    }
}
