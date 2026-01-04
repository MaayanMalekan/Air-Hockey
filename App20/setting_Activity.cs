using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App20
{
    [Activity(Label = "setting_Activity")]
    public class setting_Activity : Activity, Android.Views.View.IOnClickListener, SeekBar.IOnSeekBarChangeListener
    {
        Button btnBack;
        ImageView iv0, iv1, iv2, iv3, iv4, iv5, iv6, iv7, iv8, iv9, iv10, iv11;
        SeekBar sb;
        Switch sw;
        public static ISharedPreferences sp;
        public static Bitmap bitmap;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
           
            SetContentView(Resource.Layout.setting_layout);
            btnBack = FindViewById<Button>(Resource.Id.btnBackS);
            iv0 = FindViewById<ImageView>(Resource.Id.iv0);
            iv1 = FindViewById<ImageView>(Resource.Id.iv1);
            iv2 = FindViewById<ImageView>(Resource.Id.iv2);
            iv3 = FindViewById<ImageView>(Resource.Id.iv3);
            iv4 = FindViewById<ImageView>(Resource.Id.iv4);
            iv5 = FindViewById<ImageView>(Resource.Id.iv5);
            iv6 = FindViewById<ImageView>(Resource.Id.iv6);
            iv7 = FindViewById<ImageView>(Resource.Id.iv7);
            iv8 = FindViewById<ImageView>(Resource.Id.iv8);
            iv9 = FindViewById<ImageView>(Resource.Id.iv9);
            iv10 = FindViewById<ImageView>(Resource.Id.iv10);
            iv11 = FindViewById<ImageView>(Resource.Id.iv11);
            sb = FindViewById<SeekBar>(Resource.Id.sb);
            sw = FindViewById<Switch>(Resource.Id.sw);
            sw.CheckedChange += OnChekedChanged;
            sb.SetOnSeekBarChangeListener(this);
            btnBack.SetOnClickListener(this);
            iv0.SetOnClickListener(this);
            iv1.SetOnClickListener(this);
            iv2.SetOnClickListener(this);
            iv3.SetOnClickListener(this);
            iv4.SetOnClickListener(this);
            iv5.SetOnClickListener(this);
            iv6.SetOnClickListener(this);
            iv7.SetOnClickListener(this);
            iv8.SetOnClickListener(this);
            iv9.SetOnClickListener(this);
            iv10.SetOnClickListener(this);
            iv11.SetOnClickListener(this);



            DrawSurfaceView.am = (AudioManager)GetSystemService(AudioService);
            sb.Max = DrawSurfaceView.am.GetStreamMaxVolume(Stream.Music);
            //sb.Min = 0;
            DrawSurfaceView.am.SetStreamVolume(Stream.Music, sb.Max/2, 0);
            int volume = sb.Max;
            sb.Progress = volume;
            sp = GetSharedPreferences("setting", FileCreationMode.Private);

            int myVolume = sp.GetInt("volume", -1);
            if (myVolume != -1)
            {
                sb.Progress = myVolume;
            }

            sw.Checked = sp.GetBoolean("mute", true);
        }

        private void OnChekedChanged(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (!e.IsChecked)
            {
                DrawSurfaceView.am.SetStreamVolume(Stream.Music, 0, VolumeNotificationFlags.PlaySound);
                sb.Visibility = Android.Views.ViewStates.Invisible;
            }
            else
                sb.Visibility = Android.Views.ViewStates.Visible;
            var editor = sp.Edit();
            editor.PutBoolean("mute", e.IsChecked);
            editor.Commit();

        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            DrawSurfaceView.am.SetStreamVolume(Stream.Music, progress, VolumeNotificationFlags.PlaySound);
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            var editor = sp.Edit();
            editor.PutInt("volume", seekBar.Progress);
            editor.Commit();
        }

        public void OnClick(View v)
        {
            if (v == btnBack)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }

            if (v is ImageView)
            {
                int tag = int.Parse((string)(((ImageView)v).Tag));
                ImageView iv = (ImageView)v;               
                bitmap = Bitmap.CreateBitmap(iv.Width, iv.Height, Bitmap.Config.Rgb565);
                Canvas canvas = new Canvas(bitmap);

                iv.Draw(canvas);
                if (tag == 0)
                {
                    DrawSurfaceView.user = new User(this);
                }
                if (tag == 1)
                {
                    DrawSurfaceView.computer = new Enemy(this);
                }
            }
        }
    }
}