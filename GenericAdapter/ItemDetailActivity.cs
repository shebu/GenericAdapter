using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GenericAdapter
{
	[Activity (Label = "ItemDetailActivity")]			
	public class ItemDetailActivity : Activity
	{
		public Item CurrentItem;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			SetContentView(Resource.Layout.ItemDetail);
			// Create your application here
			
			string serialiszedItem  = Intent.GetStringExtra("CurrentItem");
			CurrentItem= new Item();
			CurrentItem.Key = serialiszedItem.Split('|')[0];
			CurrentItem.Value  = serialiszedItem.Split('|')[1];
			
			
			
			TextView keyView = FindViewById<TextView>(Resource.Id.idView);
			TextView valueView = FindViewById<TextView>(Resource.Id.nameView);
			if(CurrentItem!=null) 
			{
				keyView.Text = CurrentItem.Key;
				valueView.Text = CurrentItem.Value;
			}
		}
	}
}

