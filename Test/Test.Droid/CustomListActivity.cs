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
		List<VectorItem> ti_vector = new List<VectorItem> () {
			new VectorItem () { MainText = "Vegetables", SubText = "65 items", Color = 0x800080 },
			new VectorItem () { MainText = "Bulbs", SubText = "18 items", Checked = true, Color = 0xf0f8ff },
			new VectorItem () { MainText = "Fruits", SubText = "17 items", Color = 0x794044 }
		};
		List<RasterItem> ti_raster = new List<RasterItem> () {
			new RasterItem () { MainText = "Tubers", SubText = "43 items" },
			new RasterItem () { MainText = "Flower Buds", SubText = "5 items", Checked = true },
			new RasterItem () { MainText = "Legumes", SubText = "33 items" }
		};

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			SetContentView (Resource.Layout.LV_1);
			lv_vector = FindViewById<ListView> (Resource.Id.List_Vector);
			lv_raster = FindViewById<ListView> (Resource.Id.List_Raster);

			lv_vector.Adapter = new VectorAdapter (this, ti_vector);
			lv_vector.ItemClick += OnVectorListItemClick;
			lv_vector.ChoiceMode = ChoiceMode.Single;

			lv_raster.Adapter = new RasterAdapter (this, ti_raster);
			lv_raster.ChoiceMode = ChoiceMode.None;
		}

		void OnVectorListItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			// var lv = sender as ListView;
			var t = ti_vector [e.Position];
			e.View.FindViewById<CheckBox> (Resource.Id.Check).Checked = true;
			t.Checked = true;
			Toast.MakeText (this, "Select " + t.MainText + " as Edit Layer", ToastLength.Short).Show ();

//			var sparseArray = lv.CheckedItemPositions;
//			var sb = new System.Text.StringBuilder ();
//			for (var i = 0; i < sparseArray.Size (); i++) {
//				if (sparseArray.ValueAt (i))
//					sb.Append (sparseArray.KeyAt (i) + " ");
//			}
//			Android.Util.Log.Debug ("CustomActivity", "Select " + sb.ToString ());
		}
	}

	public class VectorItem
	{
		public int Id { get; set; }

		public string MainText{ get; set; }

		public string SubText{ get; set; }

		public bool Checked{ get; set; }

		public int Color{ get; set; }
	}

	public class RasterItem
	{
		public string Key { get; set; }

		public string MainText{ get; set; }

		public string SubText{ get; set; }

		public bool Checked{ get; set; }
	}

	internal class VectorAdapter : BaseAdapter<VectorItem>
	{
		List<VectorItem> items;
		Activity context;

		public VectorAdapter (Activity context, List<VectorItem> items)
			: base ()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override VectorItem this [int position] {
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
				view = context.LayoutInflater.Inflate (Resource.Layout.LvVectorRow, null);
			view.FindViewById<TextView> (Resource.Id.Text1).Text = item.MainText;
			view.FindViewById<TextView> (Resource.Id.Text2).Text = item.SubText;
			view.FindViewById<ImageView> (Resource.Id.Image).SetImageResource (Resource.Drawable.vector);
			view.FindViewById<CheckBox> (Resource.Id.Check).Checked = item.Checked;
				
			var spinner = view.FindViewById<Spinner> (Resource.Id.ColorSpinner);
			// spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
			var adapter = new ColorSpinnerAdapter (this.context.Resources.GetStringArray (Resource.Array.colors_array), this.context);
			spinner.Adapter = adapter;
			var pos = adapter.GetPosition (item.Color);
			spinner.SetSelection (pos);
			return view;
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			// Spinner spinner = (Spinner)sender;
			// string toast = string.Format ("The planet is {0}", spinner.GetItemAtPosition (e.Position));
			// Toast.MakeText(this.context, toast, ToastLength.Long).Show();
		}
	}

	internal class RasterAdapter : BaseAdapter<RasterItem>
	{
		List<RasterItem> items;
		Activity context;

		public RasterAdapter (Activity context, List<RasterItem> items)
			: base ()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override RasterItem this [int position] {
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
				view = context.LayoutInflater.Inflate (Resource.Layout.LvRasterRow, null);
			view.FindViewById<TextView> (Resource.Id.Text1).Text = item.MainText;
			view.FindViewById<TextView> (Resource.Id.Text2).Text = item.SubText;
			view.FindViewById<ImageView> (Resource.Id.Image).SetImageResource (Resource.Drawable.raster);
			view.FindViewById<CheckBox> (Resource.Id.Check).Checked = item.Checked;
			return view;
		}
	}

	/// <summary>
	/// Generic adapter for Spinners throughout the app
	/// </summary>
	internal class ColorSpinnerAdapter : BaseAdapter, ISpinnerAdapter
	{
		string[] items;
		Context context;

		public int GetPosition (int color)
		{
			var c = new Color (color);
			string hex = string.Format ("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B).ToLower();
			int ret = 0;
			for (int i = 0; i < items.Length; i++) {
				if (hex == items [i]) {
					ret = i;
					break;
				}
			}
			return ret;
		}

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
				view.Text = "          ";
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