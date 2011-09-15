using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.IO;
using System.Data;
using Mono.Data.Sqlite;

namespace GenericAdapter
{
	[Activity (Label = "a Generic Adapter Usage", MainLauncher = true)]
	public class GenericAdapterUsage : Activity
	{
		string dbPath = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal),"MotorCycle.db3");
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			
			// Settings data
			if(!File.Exists (dbPath))
			{
				CreateDatabase();
				FillDataBase();
			}
			
			// Autocomplete component (I wanna test this feature too)
			AutoCompleteTextView input = FindViewById<AutoCompleteTextView>(Resource.Id.txtInput);
			ArrayAdapter autoCompleteData = new ArrayAdapter<string>(this,Resource.Layout.SimpleListItem, new string[]{"Harley","Honda","Yamaha","Suzuki","Kawazaki","Triumph","Voxan","Ducati","BMW","Moto Guzzy"});
			input.Adapter = autoCompleteData;
			
			// Setting the genericAdapter and the ListView		
			ListView listOfMotorCycle = FindViewById<ListView>(Resource.Id.listMotorCycle);
			// the genericAdapter is instantiate with a method that retrieve datas from our database
			GenericAdapter<Item> myGenericAdapter = new GenericAdapter<Item>(this,Resource.Layout.IdAndNameListItem, GetData);
			
			listOfMotorCycle.Adapter = myGenericAdapter;
			listOfMotorCycle.ItemClick+= new EventHandler<ItemEventArgs>(showDetails);
			
			// finaly, attach an event to the button 'save'
			Button btnSave = FindViewById<Button> (Resource.Id.btnSave);
			btnSave.Click += delegate 
							{
								InsertData(input.Text);
								myGenericAdapter.Bind();
								input.Text="";
							};
			
		}
		
		// a dumb DataGrabber, but it will do the trick for this test 
		private List<Item> GetData() 
		{
			List<Item> items = new List<Item>();
			var connection = new SqliteConnection("Data Source="+dbPath);
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText = "Select key,value from  [items] order by value";
			SqliteDataReader reader =   command.ExecuteReader();
			 while(reader.Read())
			{
				items.Add(new Item((string)reader[0],(string)reader[1]));
			}
			connection.Close();
			return items;
		}
		
		private void CreateDatabase()
		{
			if(File.Exists (dbPath))
				return;
			
			SqliteConnection.CreateFile (dbPath);
			var connection = new SqliteConnection ("Data Source=" + dbPath);

			connection.Open ();
	        using (var c = connection.CreateCommand ()) 
			{
			    c.CommandText = "CREATE TABLE [Items] (Key ntext, Value ntext)";
        	    c.ExecuteNonQuery ();
			}
			connection.Close();
		}

		private void Log(string method, string message)
		{
				Android.Util.Log.WriteLine(Android.Util.LogPriority.Debug,method,message);
		}
		
		private void FillDataBase()
		{
			var connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			List<string> Datas = new List<string>() {"Yamaha", "Suzuki","Honda","Harley-Davidson"};
			foreach(string data in Datas)
			{
				InsertData(data);
			}
		}

		private void InsertData(string data)
		{
			var connection = new SqliteConnection("Data Source="+dbPath);
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText = string.Format("Insert into [items](key, value) values ('{0}','{1}');",GetKey(),data);
			command.ExecuteReader();
			connection.Close();
		}

		private string GetKey()
		{
			return Guid.NewGuid().ToString();
		}
		
		private void showDetails(object sender, ItemEventArgs  e)
		{
			ListView aListView = (ListView)sender;
			GenericAdapter<Item> adapter = (GenericAdapter<Item>)aListView.Adapter;
			Item selectedItem = new Item();
			selectedItem = adapter.GetItem(e.Position);
			Intent intent = new Intent(this.BaseContext, (Java.Lang.Class) new ItemDetailActivity().Class); 
			intent.AddFlags(ActivityFlags.NewTask); 
			intent.PutExtra("CurrentItem",string.Format("{0}|{1}",selectedItem.Key,selectedItem.Value));
			StartActivity(intent); 
		}
		
	}
}


