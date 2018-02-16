using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;



namespace Wind2DThing
{
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Contact> mitems = new ObservableCollection<Contact>();
        public MainPage()
        {
            this.InitializeComponent();


            listBox1.ItemsSource = mitems;

            mitems.Add(new Contact { FirstName = "Russell" });
            mitems.Add(new Contact { FirstName = "Ben" });
            mitems.Add(new Contact { FirstName = "Niklas" });
        }
    }
}
