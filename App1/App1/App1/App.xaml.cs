using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App1
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			//MainPage = new App1.Main();
            MainPage = new ContentPage
            {
               
                //Content = new Label
                //{
                //    Text = "Hello, Forms !",
                //    VerticalOptions = LayoutOptions.CenterAndExpand,
                //    HorizontalOptions = LayoutOptions.CenterAndExpand,
                //    BackgroundColor = Color.Black,
                //    TextColor = Color.White
                //}
            };

            
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
