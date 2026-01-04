using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace App20
{
    [Activity(Label = "Air Hockey",Theme = "@style/AppTheme")]
    public class GameActivity : AppCompatActivity
    {
        DrawSurfaceView ds;
        bool userAskBack = false;
        ISharedPreferences sp;
        //int numU, numE;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            ds = new DrawSurfaceView(this);
            SetContentView(ds);
            ds.t.Start();

            DrawSurfaceView.startSound.Start();


            //sp = GetSharedPreferences("setting", FileCreationMode.Private);            
            //numU = sp.GetInt("user", -1);
            //numE = sp.GetInt("enemy", -1);
            //if (numU != -1)
            //    DrawSurfaceView.setNumU(numU);
            //if (numE != -1)
            //    DrawSurfaceView.setNumE(numE);

            //if (dExit)
            //    Finish();
            //if (dAgain)
            //{
            //    Intent intent = new Intent(this, typeof(MainActivity));
            //    StartActivity(intent);
            //}
        }


        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)       
        {
            if (item.ItemId == Resource.Id.action_restart)
            {
                Toast.MakeText(this, "you selected to start the game again", ToastLength.Long).Show();
                Intent intent = new Intent(this, typeof(GameActivity));
                StartActivity(intent);
                return true;
            }

            else if (item.ItemId == Resource.Id.action_back)
            {
                Toast.MakeText(this, "you selected to go to the main menu", ToastLength.Long).Show();
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                return true;
            }

            else if (item.ItemId == Resource.Id.action_exit)//*********************לא יוצא לגמרי*********************
            {
                Toast.MakeText(this, "you selected to exit the game", ToastLength.Long).Show();
                Activity activity = (Activity)this;
                activity.FinishAffinity();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (ds != null)
            {
                ds.resume();
                Toast.MakeText(this, "OnResume", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "ds is null OnResume", ToastLength.Long).Show();
            }
        }

        protected override void OnStart()
        {
            base.OnStart(); ;
            Toast.MakeText(this, "OnStart", ToastLength.Long).Show();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Toast.MakeText(this, "OnDestroy", ToastLength.Long).Show();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Toast.MakeText(this, "OnStop", ToastLength.Long).Show();

        }


        protected override void OnPause()
        {
            base.OnPause();
            if (userAskBack)
            {
                Toast.MakeText(this, "OnPause1", ToastLength.Long).Show();
            }
            else if (ds != null)
            {
                ds.pause();
                Toast.MakeText(this, "OnPause2", ToastLength.Long).Show();

            }
            Toast.MakeText(this, "OnPause3", ToastLength.Long).Show();
        }

        public override void Finish()
        {
            base.Finish();
            userAskBack = true;
            ds.threadRunning = false;
            while (true)
            {
                try
                {
                    ds.t.Join();
                }
                catch (InterruptedException e)
                {
                    //Toast.MakeText(this,"some problem happened",ToastLength.Long).Show();
                }
                break;
            }
            Toast.MakeText(this, "Finish" + ds.threadRunning, ToastLength.Long).Show();
        }
    }
}