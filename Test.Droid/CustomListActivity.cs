using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Test.Droid
{
	[Activity (Label = "CustomList", MainLauncher = true, Icon = "@drawable/icon")]
	public class CustomListActivity : Activity
	{
		ListView lv_vector, lv_raster;
		List<TableItem> ti_vector = new List<TableItem> ();
		List<TableItem> ti_raster = new List<TableItem> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			SetContentView (Resource.Layout.LV_1);
			lv_vector = FindViewById<ListView> (Resource.Id.List_Vector);
			lv_raster = FindViewById<ListView> (Resource.Id.List_Raster);

			ti_vector.Add (new TableItem () { Name = "Vegetables", Date = "65 items", Type = "vector" });
			ti_vector.Add (new TableItem () { Name = "Fruits", Date = "17 items", Type = "vector" });
			ti_raster.Add (new TableItem () { Name = "Flower Buds", Date = "5 items", Type = "raster", Checked = true });
			ti_raster.Add (new TableItem () { Name = "Legumes", Date = "33 items", Type = "raster" });
			ti_vector.Add (new TableItem () { Name = "Bulbs", Date = "18 items", Type = "vector", Checked = true });
			ti_raster.Add (new TableItem () { Name = "Tubers", Date = "43 items", Type = "raster" });

			lv_vector.Adapter = new CustomAdapter1 (this, ti_vector);
			lv_vector.ItemClick += OnVectorListItemClick;
			lv_vector.ChoiceMode = ChoiceMode.Single;

			lv_raster.Adapter = new CustomAdapter1 (this, ti_raster);
			lv_raster.ChoiceMode = ChoiceMode.None;
		}

		void OnVectorListItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			var lv = sender as ListView;
			var t = ti_vector [e.Position];
			Android.Widget.Toast.MakeText (this, t.Name, Android.Widget.ToastLength.Short).Show ();
			e.View.FindViewById<CheckBox> (Resource.Id.Check).Checked = true;
			t.Checked = true;
			var sparseArray = lv.CheckedItemPositions;
			var sb = new System.Text.StringBuilder ();
			for (var i = 0; i < sparseArray.Size (); i++) {
				if (sparseArray.ValueAt (i))
					sb.Append (sparseArray.KeyAt (i) + " ");
			}
			Android.Util.Log.Debug ("CustomActivity", "Select " + sb.ToString ());
		}
	}

	public class TableItem
	{
		public string Name { get; set; }

		public string Date { get; set; }

		public string Type { get; set; }

		public string Count { get; set; }

		public bool Checked { get; set; }
	}

	public class CustomAdapter1 : BaseAdapter<TableItem>
	{
		List<TableItem> items;
		Activity context;

		public CustomAdapter1 (Activity context, List<TableItem> items)
			: base ()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override TableItem this [int position] {
			get { return items [position]; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items [position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate (Resource.Layout.LV_CustomRow_1, null);
			view.FindViewById<TextView> (Resource.Id.Text1).Text = item.Name;
			view.FindViewById<TextView> (Resource.Id.Text2).Text = item.Date;
			var isVector = item.Type == "vector";
			var id = isVector ? Resource.Drawable.vector : Resource.Drawable.raster;
			view.FindViewById<ImageView> (Resource.Id.Image).SetImageResource (id);
			view.FindViewById<CheckBox> (Resource.Id.Check).Checked = item.Checked;

			var spinner = view.FindViewById<Spinner> (Resource.Id.ColorSpinner);
			// spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
			var adapter = new ColorSpinnerAdapter (this.context.Resources.GetStringArray (Resource.Array.colors_array), this.context);
			spinner.Adapter = adapter;
			if (!isVector) {
				// spinner.Enabled = false;
				spinner.Visibility = ViewStates.Invisible;
			}
			return view;
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			// Spinner spinner = (Spinner)sender;
			// string toast = string.Format ("The planet is {0}", spinner.GetItemAtPosition (e.Position));
			// Toast.MakeText(this.context, toast, ToastLength.Long).Show();
		}
	}

	/// <summary>
	/// Generic adapter for Spinners throughout the app
	/// </summary>
	public class ColorSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		string[] items;
		Context context;

		public override int Count {
			get {
				return items.Length;
			}
		}

		public ColorSpinnerAdapter (string[] items, Context context)
			: base ()
		{
			this.items = items;
			this.context = context;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return items [position];
		}

		public override long GetItemId (int position)
		{
			return (long)position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items.ElementAtOrDefault (position);
			var color = Color.ParseColor (item);
			if (convertView != null) {
				convertView.SetBackgroundColor (color);
				return convertView;
			} else {
				var view = new TextView (context);
				view.Text = "      ";
				view.TextSize = 20;
				view.SetBackgroundColor (color);
				return view;
			}
		}

		protected override void Dispose (bool disposing)
		{
			context = null;
			base.Dispose (disposing);
		}
	}
}