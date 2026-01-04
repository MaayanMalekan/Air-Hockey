using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App20
{
    class User : DadPlayer
    {
        private float xTouch;
        private float yTouch;

        public User(Context context, bool b)
            : base(1080 / 2 - (BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer).Width /2 ), 
                  2031 - BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer).Height,
                  BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer))
        { }

        public User(Context context)
            : base(1080 / 2 - (BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer).Width / 2),
                  2031 - BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer).Height, 
                  setting_Activity.bitmap)
        { }

        public bool Inside(float x, float y)
        {
            return x > this.x && x < this.x + this.bitmap.Width &&
                   y > this.y && y < this.y + this.bitmap.Height;
        }

        public bool Move(float eX, float eY, bool bStart, float h)
        {
            if (!Inside(eX, eY))
                return false;

            if (bStart)
            {
                this.xTouch = eX;
                this.yTouch = eY;
            }
            else
            {

                this.vx = eX - this.xTouch;
                this.vy = eY - this.yTouch;
                //if (vx * vx + vy * vy > vMaxPow2)
                //{
                //    vMaxPow2 = vx * vx + vy * vy;
                //    Console.WriteLine("********** vMax = " + Math.Sqrt(vMaxPow2)); // מהירות מקסימלית רגעית (?)
                //}


                if (this.y + this.vy <= h / 2)
                    this.y = h / 2;
                else
                    this.y += this.vy;

                this.x += this.vx;

                this.xTouch = eX;
                this.yTouch = eY;
            }
            return true;
        }

        public void CourtCollision(float h, float w) //שמירה על השחקנים בגבולות המסך, שלא יצאו ממנו
        {
            if (this.x < 0)
                this.x = 0;
            if (this.x > w - this.bitmap.Width)
                this.x = w - this.bitmap.Width;
            if (this.y < 0)
                this.y = 0;
            if (this.y > h - this.bitmap.Height)
                this.y = h - this.bitmap.Width;
        }
    }
}