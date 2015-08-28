using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Test.Droid
{
    [Activity(Label = "SimpleList", MainLauncher = false, Icon = "@drawable/icon")]
    public class SimpleListActivity : ListActivity
    {
        string[] items;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var list = new List<string>();
            for (int i = 0; i < 50; i++)
                list.AddRange(new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" });
            items = list.ToArray();
            // The ListView itself supports different selection modes, regardless of the accessory being displayed. 
            // To avoid confusion, use Single selection mode with Checked and SingleChoice accessories and the Multiple mode with the MultipleChoice style. The selection mode is controlled by the ChoiceMode property of the ListView.
            // ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            // ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
            ListAdapter = new SimpleListAdapter(this, items);
            ListView.FastScrollEnabled = true;

            // For targeting Gingerbread the ChoiceMode is an int, otherwise it is an
            // enumeration.

            ListView.ChoiceMode = Android.Widget.ChoiceMode.Single; // 1
            //lv.ChoiceMode = Android.Widget.ChoiceMode.Multiple; // 2
            //lv.ChoiceMode = Android.Widget.ChoiceMode.None; // 0

            // Use this block if targeting Gingerbread or lower
            /*
            lv.ChoiceMode = Android.Widget.ChoiceMode.Single; // Single
            //lv.ChoiceMode = 0; // none
            //lv.ChoiceMode = 2; // Multiple
            //lv.ChoiceMode = 3; // MultipleModal
            */
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = items[position];
            Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
            // Set the initially checked row ("Fruits")
            l.SetItemChecked(position, true);
        }

        // To determine which row has been selected in Single mode use the CheckedItemPosition integer property:
        // FindViewById<ListView>(Android.Resource.Id.List).CheckedItemPosition

        // To determine which rows have been selected in Multiple mode you need to loop through the CheckedItemPositions SparseBooleanArray. A sparse array is like a dictionary that only contains entries where the value has been changed, so you must traverse the entire array looking for true values to know what has been selected in the list as illustrated in the following code snippet:
        //var sparseArray = FindViewById<ListView>(Android.Resource.Id.List).CheckedItemPositions;
        //for (var i = 0; i < sparseArray.Size(); i++ )
        //{
        //   Console.Write(sparseArray.KeyAt(i) + "=" + sparseArray.ValueAt(i) + ",");
        //}
        //Console.WriteLine();
    }

    public class SimpleListAdapter : BaseAdapter<string>//, ISectionIndexer
    {
        string[] items;
        Activity context;
        //Dictionary<string, int> alphaIndex;
        //string[] sections;
        //Object[] sectionsObjects;

        public SimpleListAdapter(Activity context, string[] items)
            : base()
        {
            this.context = context;
            this.items = items;

            //alphaIndex = new Dictionary<string, int>();
            //for (int i = 0; i < items.Length; i++)
            //{ // loop through items
            //    var key = items[i][0].ToString();
            //    if (!alphaIndex.ContainsKey(key))
            //        alphaIndex.Add(key, i); // add each 'new' letter to the index
            //}
            //sections = new string[alphaIndex.Keys.Count];
            //alphaIndex.Keys.CopyTo(sections, 0); // convert letters list to string[]
            //// Interface requires a Java.Lang.Object[], so we create one here
            //sectionsObjects = new Java.Lang.Object[sections.Length];
            //for (int i = 0; i < sections.Length; i++)
            //{
            //    sectionsObjects[i] = new Java.Lang.String(sections[i]);
            //}
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Length; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
            {
                //view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                //view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Heading;
                //view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = item.SubHeading;
                //view.FindViewById<ImageView>(Android.Resource.Id.Icon).SetImageResource(item.ImageResourceId); // only use with ActivityListItem

                //// The group view is set in the GetGroupView method
                //view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleExpandableListItem1, null);
                //// The child view is set in the GetChildView
                //view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleExpandableListItem2, null);

                // SimpleListItemChecked – Creates a single-selection list with a check as the indicator.
                // SimpleListItemSingleChoice – Creates radio-button-type lists where only one choice is possible.
                // SimpleListItemMultipleChoice – Creates checkbox-type lists where multiple choices are possible.
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null);
            }
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
            return view;
        }

        //#region ISectionIndexer Members

        //public int GetPositionForSection(int sectionIndex)
        //{
        //    return alphaIndex[sections[sectionIndex]];
        //}

        //public int GetSectionForPosition(int position)
        //{
        //    // this method isn't called in this example, but code is provided for completeness
        //    int prevSection = 0;
        //    for (int i = 0; i < sections.Length; i++)
        //    {
        //        if (GetPositionForSection(i) > position)
        //        {
        //            break;
        //        }
        //        prevSection = i;
        //    }
        //    return prevSection;
        //}

        //public Object[] GetSections()
        //{
        //    return sectionsObjects;
        //}

        //#endregion
    }
}

