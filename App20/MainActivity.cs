using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Media;

namespace App20
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity, Android.Views.View.IOnClickListener
    {
        Button btnStart, btnSetting, btnInstructions;
        BroadcastBattery broadcastBattery;
        TextView tvBattery;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnStart = (Button)FindViewById(Resource.Id.btnStart);
            btnSetting = (Button)FindViewById(Resource.Id.btnSetting);
            btnInstructions = (Button)FindViewById(Resource.Id.btnInstructions);
            tvBattery = FindViewById<TextView>(Resource.Id.tvBattery);
            btnStart.SetOnClickListener(this);
            btnSetting.SetOnClickListener(this);
            btnInstructions.SetOnClickListener(this);
            broadcastBattery = new BroadcastBattery(tvBattery);
        }

        public void OnClick(View v)
        {
            if (v == btnStart)
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                StartActivity(intent);
            }

            if (v == btnSetting)
            {
                Intent intent = new Intent(this, typeof(setting_Activity));
                StartActivity(intent);
            }

            if (v == btnInstructions)
            {
                Intent intent = new Intent(this, typeof(instructions_Activity));
                StartActivity(intent);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadcastBattery, new IntentFilter(Intent.ActionBatteryChanged));
        }

        protected override void OnPause()
        {
            UnregisterReceiver(broadcastBattery);
            base.OnPause();
        }

        
    }
}