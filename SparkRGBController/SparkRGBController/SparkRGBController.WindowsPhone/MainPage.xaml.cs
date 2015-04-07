using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SparkRGBController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        //well use this so its not sending data while data is currently being sent
        bool isBusy = false;
        async void setRGB()
        {
            isBusy = true;
            string rgb = (sbRed.Value).ToString() + "," + (sbGreen.Value).ToString() + "," + sbBlue.Value.ToString();
            //+----------------------------------------+
            //REPLACE ALL OF THIS WITH YOUR SPARKS STUFF
            //+----------------------------------------+
            string deviceID = "55ff6f066678505553211367";

            string accessToken = "06979f834fbe2ebd2537e0dfd84f2cd5df160eba";
            //uri for the sparkcore
            Uri site = new Uri("https://api.spark.io/v1/devices/" + deviceID + "/setRgb");
            var http = new HttpClient();
            //declare the content type
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            //add our post values
            HttpContent content = new FormUrlEncodedContent(new[]
                     {   
                         new KeyValuePair<string, string>("access_token", accessToken),
                            new KeyValuePair<string, string>("args", rgb)
                     });
            //post
            var response = await http.PostAsync(site, content);

            string data = await response.Content.ReadAsStringAsync();

            //here is the JSON response, we dont really care about the contents in this app
            data = data.Replace("{", "");
            data = data.Replace('"', ' ');
            data = data.Replace(',', ' ');

            txtRGB.Text = data + "\n" + rgb;

            //we dont need to send a value change every single number, well delay for 50ms to save CPU
            await System.Threading.Tasks.Task.Factory.StartNew(() => System.Threading.Tasks.Task.Delay(50));
            isBusy = false;
        }

        private void sb_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!isBusy)
                setRGB();
        }
    }
}
