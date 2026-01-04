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
    class Player 
    {
        public enum Kind{User, Animy};
        private Kind kind;
        private float x;
        private float y;
        private float vx;
        private float vy;
        private Bitmap bitmap;
        private float xTouch;
        private float yTouch;
        private float vMaxPow2 = 0;


        public Player(Kind kind, Context context) 
        {
            if (kind == Kind.User)
            {
                this.bitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer);
                this.x = 100;
                this.y = 100;
                this.kind = Kind.User;
            }
            if (kind == Kind.Animy)
            {
                this.bitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.blue_player);
                this.x = 600;
                this.y = 600;
                this.kind = Kind.Animy;
            }
        }

        public Kind GetKind()
        {
            return this.kind;
        }
        public float GetX()
        {
            return x;
        }
        public float GetY()
        {
            return this.y;
        }
        public float GetVx()
        {
            return this.vx;
        }
        public float GetVy()
        {
            return this.vy;
        }

        public void SetX(float x)
        {
            this.x = x;
        }
        public void SetY(float y)
        {
            this.y = y;
        }
        public void SetVX(float vx)
        {
            this.vx = vx;
        }
        public void SetVY(float vy)
        {
            this.vy = vy;
        }
        public void SetBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void Draw(Canvas c)
        {
            c.DrawBitmap(this.bitmap, this.x, this.y, null);
        }

        public bool Inside (float x, float y)
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
                if (vx * vx + vy * vy > vMaxPow2)
                {
                    vMaxPow2 = vx * vx + vy * vy;
                    Console.WriteLine("********** vMax = " + Math.Sqrt(vMaxPow2)); // מהירות מקסימלית רגעית (?)
                }
                    

                if (this.kind == Kind.User)
                {
                    if (this.y + this.vy >= h / 2)
                        this.y = h / 2; 
                    else
                        this.y += this.vy;
                }
                else if (this.kind == Kind.Animy)
                {
                    if (this.y + this.vy <= h / 2)
                        this.y = h / 2;
                    else
                        this.y += this.vy;
                }

                this.x += this.vx;
                   
                this.xTouch = eX;
                this.yTouch = eY;                
            }
               

                //if (this.x < 0 || this.x > c.Width - this.bitmap.Width)
                //    this.vx = 0;
                //if (this.y < 0 || this.y > c.Height - this.bitmap.Height)
                //    this.vy = 0;

            return true;
        }

        public bool Collision(Ball b)
        {
            float xMiddlePlayer = this.x + this.bitmap.Width/2;
            float yMiddlePlayer = this.y + this.bitmap.Height/2;
            float xMiddleBall = b.GetX() + b.GetBitmap().Width/2;
            float yMiddleBall = b.GetY() + b.GetBitmap().Height/2;
            float player_ballR = (this.bitmap.Width / 2) + (b.GetBitmap().Width / 2);
            if (Math.Pow(xMiddlePlayer - xMiddleBall, 2) + 
                Math.Pow(yMiddlePlayer - yMiddleBall, 2) <= 
                player_ballR * player_ballR)
                return true;
            return false;
        }

        public void CourtCollision(float h, float w)
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


        public bool InDownGoal(Ball b, float h, float w)
        {
            float ly = b.GetY() - this.y;

            float lx = Math.Abs(this.x - b.GetX());

            float by = h - this.y;

            float bx = (lx * by) / ly;

            while (bx > w)
                bx -= w;

            if (this.x > b.GetX())
            {
                if (this.x - bx > 363 && this.x - bx < 739)
                    return true;
            }                
            else if (this.x < b.GetX())
                if (this.x + bx > 363 && this.x + bx < 739)
                    return true;

            return false;
        }

        public bool InUpGoal(Ball b, float h, float w)
        {
            float ly = this.y - b.GetY();

            float lx = Math.Abs(this.x - b.GetX());

            float by = this.y;

            float bx = (lx * by) / ly;

            while (bx > w)
                bx -= w;

            if (this.x > b.GetX())
            {
                if (this.x - bx > 363 && this.x - bx < 739)
                    return true;
            }
            else if (this.x < b.GetX())
                if (this.x + bx > 363 && this.x + bx < 739)
                    return true;

            return false;


        }

        public ListOptions Options(Ball b)//---- checked
        {
            Option option;

            double vBallTotal = Math.Sqrt(Math.Pow(b.GetVy(), 2) + Math.Pow(b.GetVx(), 2));

            //float ly = b.GetY() - this.y;
            //float lx = Math.Abs(this.x - b.GetX());
            //double distanse = Math.Sqrt(Math.Pow(ly, 2) + Math.Pow(lx, 2));            
            //int numOfOptions = (int)(distanse / vBallTotal)+1 ;

            float ly = this.y - b.GetY(); //לבדוק איזה צד
            float lx = Math.Abs(this.x - b.GetX());
            float by = this.y;
            float bx = (lx * by) / ly;
            double bYeter = Math.Sqrt(Math.Pow(bx, 2) + Math.Pow(by, 2));
            int numOfOptions = (int)(bYeter / vBallTotal);

            ListOptions arr = new ListOptions();
            float x = b.GetX();
            float y = b.GetY();
            for (int i = 0; i < numOfOptions; i++)
            {
                option = new Option(x, y, i);
                if (CanArriveOnTime(option))
                {
                    arr.GetOptions().Add(option);
                }
                x += b.GetVx();
                y += b.GetVy();

            }
            return arr;
        }
        public ListOptions BuildAllCyclesOptions(Ball b, float canvasH, float canvasW)//checked
        {
            //save ball x,y position
            float firstX = b.GetX();
            float firstY = b.GetY();

            ListOptions arr = Options(b);

            //restore ball x,y position
            b.SetX(firstX);
            b.SetY(firstY);

            return arr;
        }


        public ListOptions BestOptionToGoal(List<Option> arr, Ball b, float canvasH, float canvasW)
        {
            ListOptions optionsInGoal = new ListOptions();

            for (int i = 0; i < arr.Count; i++)
            {
                b.SetX(arr[i].GetX());
                b.SetY(arr[i].GetY());
                if (InDownGoal(b, canvasH, canvasW))
                    optionsInGoal.GetOptions().Insert(optionsInGoal.GetOptions().Capacity, arr[i]);//לבחור אופציה טובה
            }
            return optionsInGoal;
        }


        public ListOptions BestOptionToDefend(ListOptions arr, Ball b, float canvasH, float canvasW)
        {
            ListOptions optionsToDefend = new ListOptions();
            for (int i = 0; i < arr.GetOptions().Count; i++)
            {
                b.SetX(arr.GetOptions()[i].GetX());
                b.SetY(arr.GetOptions()[i].GetY());
                if (InUpGoal(b, canvasH, canvasW))
                    optionsToDefend.GetOptions().Insert(optionsToDefend.GetOptions().Capacity, arr.GetOptions()[i]);
            }
            return optionsToDefend;
        }


        public bool CanArriveOnTime(Option o)//checked
        {
            float distansY = Math.Abs(this.y - o.GetY());
            float distansX = Math.Abs(this.x - o.GetX());

            double dTotal = Math.Sqrt(Math.Pow(distansY, 2) + Math.Pow(distansX, 2));

            return (Math.Sqrt(vMaxPow2) * o.GetT() >= dTotal);
        }

        public Option BestOption(Ball b, float w)
        {
            bool haveToDefend;
            if (b.GetVy() < 0)
            {
                float x = (b.GetY() * b.GetVx()) / b.GetVy();

                while (x > w)
                    x -= w;

                if (b.GetVx() < 0)
                {
                    if (b.GetX() - x > 363 && b.GetX() - x < 739)
                        haveToDefend = true;
                }
                else if (b.GetVx() > 0)
                    if (b.GetX() + x > 363 && b.GetX() + x < 739)
                        haveToDefend = true;
                    else
                        haveToDefend = false;
            }
            //if (haveToDefend) { }
            return new Option(1, 1, 1); //סתם כדי להריץ

        }

    }
}