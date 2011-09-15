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
	
	class GenericAdapter<T> : ArrayAdapter<T>  where T : IViewFillable
	{
		private IList<T> items;
		private Func<IList<T>> Binder;
		
	
		public  GenericAdapter(Context context, int textViewResourceId, Func<IList<T>> Binder) :base(context, textViewResourceId, new List<T>())
		{
			this.Binder = Binder;
			Bind();
		}
		
		public void Bind()
		{
			this.items = Binder();
			NotifyDataSetChanged();
		}
		
		
 
		public new T GetItem(int position)
		{
			return this.items[position];
		}
		
		public new void Add (T anItem)
		{
			throw new Exception("Method Add is not supported on this Adapter. Update your underlying data and just call Bind() method");
		}
		
		public override int Count
		{
			get 
			{
				return items.Count ;
			}
		}
		
        public override View GetView (int position, View convertView, ViewGroup parent) 
		{
                View v = convertView;
                if (v == null) 
				{
                    LayoutInflater vi = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                    v = vi.Inflate(Resource.Layout.IdAndNameListItem, null);
                }
				if(position<this.Count) 
				{
	                T o = this.items[position];
	                if (o != null) 
					{
						o.FillView(v);
	                }
				}
                return v;
        }
	}
}
	


