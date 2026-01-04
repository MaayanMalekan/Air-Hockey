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

namespace App20
{
    [Activity(Label = "instructions_Activity")]
    public class instructions_Activity : Activity, Android.Views.View.IOnClickListener
    {
        Button btnBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.instructions_layout);
            btnBack = FindViewById<Button>(Resource.Id.btnBackI);
            btnBack.SetOnClickListener(this);
        }
        public void OnClick(View v)
        {
            if (v == btnBack)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
        }
    }
}