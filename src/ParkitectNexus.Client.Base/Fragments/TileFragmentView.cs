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
using Xwt;

namespace ParkitectNexus.Client.Base.Fragments
{

    public class TileFragmentView : VBox
    {
        private readonly Stack<HBox> _rows = new Stack<HBox> ();
        private readonly List<Widget> _buttons = new List<Widget> ();
        private readonly VBox _box;



        private Size _tileSize = new Size (100, 100);
        private int _buttonsPerRow;

        public TileFragmentView(Size tileSize,int buttonsPerRow)
        {
            _tileSize = tileSize;
            _buttonsPerRow = buttonsPerRow;
        }

        private HBox PushNewRow()
        {
            var v = new HBox { Margin = new WidgetSpacing (5, 5, 5, 5) };
            _rows.Push (v);
            PackStart (v);
            return v;
        }

        protected virtual void ClearTiles()
        {
            foreach (var r in _rows)
                r.Clear ();

            _rows.Clear ();
            _box.Clear ();
            PushNewRow ();
        }

        private int CalculateButtonsPerRow(float width)
        {
            return Math.Max (1,
                (int)Math.Floor ((width - 5 - 25 /*scroll and a bit*/) / (_tileSize.Width + 5)));
        }

        public void HandleSizeUpdate(float width)
        {
            if (CalculateButtonsPerRow (width) == _buttonsPerRow)
                return;

            _buttonsPerRow = CalculateButtonsPerRow (width);
            var i = 0;

            ClearTiles ();
            PushNewRow ();

            foreach (var button in _buttons) {
                if (i >= _buttonsPerRow) {
                    PushNewRow ();
                    i = 0;
                }
                _rows.Peek ().PackStart (button);
                i++;
            }

        }



}