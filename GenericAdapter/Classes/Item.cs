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
	public class Item  : IViewFillable
	{
		public string Key;
		public string Value;
		
		public Item()
		{
			Key = "";
			Value = "";
		}
		
		public Item(string key, string Value)
		{
			this.Key = key;
			this.Value = Value;
		}
		
		public override string ToString ()
		{
			return Value;
		}
		
		public void FillView(View v) 
		{
		   TextView tt = (TextView)v.FindViewById(Resource.Id.txtItem);
			if (tt != null) 
			{
            	tt.Text = this.Value;                           
            }
			ImageView img = (ImageView)v.FindViewById(Resource.Id.imageItem);
			if(img !=null)
			{
				img.SetImageResource(Resource.Drawable.Icon);	
			}
		}
		
	}
}

