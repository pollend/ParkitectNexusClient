
// This file has been generated by the GUI designer. Do not modify.
namespace ParkitectNexus.Client.Linux
{
	public partial class ModUri
	{
		private global::Gtk.VBox vbox2;
		
		private global::Gtk.Alignment alignment2;
		
		private global::Gtk.Label label2;
		
		private global::Gtk.Alignment alignment1;
		
		private global::Gtk.Entry txtNexusURI;
		
		private global::Gtk.Button button186;
		
		private global::Gtk.Button ButtonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ParkitectNexus.Client.Linux.ModUri
			this.Name = "ParkitectNexus.Client.Linux.ModUri";
			this.Icon = global::Gdk.Pixbuf.LoadFromResource ("ParkitectNexus.Client.Linux.parkitectnexus_logo.png");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Resizable = false;
			this.AllowGrow = false;
			// Internal child ParkitectNexus.Client.Linux.ModUri.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.alignment2 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment2.Name = "alignment2";
			this.alignment2.LeftPadding = ((uint)(30));
			this.alignment2.RightPadding = ((uint)(30));
			this.alignment2.BottomPadding = ((uint)(20));
			// Container child alignment2.Gtk.Container+ContainerChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("A proper parkitectnexus:\\\\ url is required to proceed with this installation. ");
			this.label2.Wrap = true;
			this.alignment2.Add (this.label2);
			this.vbox2.Add (this.alignment2);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.alignment2]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			this.alignment1.LeftPadding = ((uint)(20));
			this.alignment1.RightPadding = ((uint)(20));
			// Container child alignment1.Gtk.Container+ContainerChild
			this.txtNexusURI = new global::Gtk.Entry ();
			this.txtNexusURI.CanFocus = true;
			this.txtNexusURI.Name = "txtNexusURI";
			this.txtNexusURI.IsEditable = true;
			this.txtNexusURI.InvisibleChar = '●';
			this.alignment1.Add (this.txtNexusURI);
			this.vbox2.Add (this.alignment1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.alignment1]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox2]));
			w6.Position = 0;
			// Internal child ParkitectNexus.Client.Linux.ModUri.ActionArea
			global::Gtk.HButtonBox w7 = this.ActionArea;
			w7.Name = "__gtksharp_123_Stetic_TopLevelDialog_ActionArea";
			w7.Spacing = 10;
			w7.BorderWidth = ((uint)(5));
			w7.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child __gtksharp_123_Stetic_TopLevelDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.button186 = new global::Gtk.Button ();
			this.button186.CanFocus = true;
			this.button186.Name = "button186";
			this.button186.UseStock = true;
			this.button186.UseUnderline = true;
			this.button186.Label = "gtk-cancel";
			this.AddActionWidget (this.button186, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w7 [this.button186]));
			w8.Expand = false;
			w8.Fill = false;
			// Container child __gtksharp_123_Stetic_TopLevelDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.ButtonOk = new global::Gtk.Button ();
			this.ButtonOk.CanFocus = true;
			this.ButtonOk.Name = "ButtonOk";
			this.ButtonOk.UseStock = true;
			this.ButtonOk.UseUnderline = true;
			this.ButtonOk.Label = "gtk-ok";
			w7.Add (this.ButtonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w9 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w7 [this.ButtonOk]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 383;
			this.DefaultHeight = 172;
			this.Show ();
			this.ButtonOk.Clicked += new global::System.EventHandler (this.Submit_URI);
		}
	}
}