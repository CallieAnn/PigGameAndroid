using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Xml.Serialization;
using System.IO;
using Android.Content;
using Android.Content.Res;
using Android.Content.PM;

namespace PigGame.LittlePig
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        PigLogic game;
        EditText player1;
        string p1Name;
        EditText player2;
        string p2Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //lock to portrait small screen, landscape large screen
            if ((Application.ApplicationContext.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask) == ScreenLayout.SizeLarge)
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            }

            else
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }

            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.MainActivity);

            // see if a dual-pane layout is loaded
            bool isDualPane = (FindViewById(Resource.Id.secondFragment) != null);


            game = new PigLogic();

            player1 = FindViewById<EditText>(Resource.Id.player_one);
            player2 = FindViewById<EditText>(Resource.Id.player_two);

            //set the players' names whenever the EditTexts are changed
            player1.TextChanged += (sender, e) =>
            {
                p1Name = player1.Text;
                game.Player1Name = p1Name;
            };

            player2.TextChanged += (sender, e) =>
            {
                p2Name = player2.Text;
                game.Player2Name = p2Name;
            };

            var startButton = FindViewById<Button>(Resource.Id.start_button);
            startButton.Click += delegate
            {
                var second = new Intent(this, typeof(SecondActivity));
                second.PutExtra("OneName", p1Name);
                second.PutExtra("TwoName", p2Name);
                StartActivity(second);

            };
        }
    }
}