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
    class Enemy : DadPlayer
    {
        public Enemy(Context context, bool b)
            : base (1080 / 2 - (BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer).Width / 2), 
                  0, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.blue_player))
        { }

        public Enemy(Context context)
            : base(1080 / 2 - (BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer).Width / 2),
                  0, setting_Activity.bitmap)
        { }

        public bool InDownGoal(Ball b, float h, float w) // בדיקת האם הדסקית נכנסה לשער התחתון
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


        public bool InUpGoal(Ball b, float h, float w) //
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


        public ListOptions Options(Ball b, float h)//---- checked
        {
            Option option;

            double vBallTotal = Math.Sqrt(Math.Pow(b.GetVy(), 2) + Math.Pow(b.GetVx(), 2)); //המהירות הכוללת של הדסקית


            //float ly = b.GetY() - this.y; //לבדוק איזה צד
            //float lx = Math.Abs(this.x - b.GetX());
            //float by = b.GetY();
            //float bx = (lx * by) / ly;
            //double bYeter = Math.Sqrt(Math.Pow(bx, 2) + Math.Pow(by, 2));
            //int numOfOptions = (int)(bYeter / vBallTotal);
            double ly = b.GetY() - this.y; //לבדוק איזה צד
            double numOfOptions1 = 1.0*ly / (-b.GetVy());
            int numOfOptions = (int)(numOfOptions1) +1;

            ListOptions list = new ListOptions();
            float x = b.GetX();
            float y = b.GetY();
            for (int i = 0; i < numOfOptions; i++)
            {
                if (y < h / 2)
                {
                    option = new Option(x, y, i);
                    if (CanArriveOnTime(option))
                    {
                        list.GetOptions().Add(option);
                    }
                }
                x += b.GetVx();
                y += b.GetVy();

            }
            return list;
        }


        public ListOptions BuildAllCyclesOptions(Ball b, float h)//checked
        {
            //save ball x,y position
            float firstX = b.GetX();
            float firstY = b.GetY();

            ListOptions list = Options(b, h);

            //restore ball x,y position
            b.SetX(firstX);
            b.SetY(firstY);

            return list;
        }


        public ListOptions BestOptionToGoal(ListOptions list, float canvasH, float canvasW)
        {
            Ball b = new Ball();
            ListOptions optionsInGoal = new ListOptions();

            for (int i = 0; i < list.GetOptions().Count; i++)
            {
                b.SetX(list.GetOptions()[i].GetX());
                b.SetY(list.GetOptions()[i].GetY());
                if (InDownGoal(b, canvasH, canvasW))
                    optionsInGoal.GetOptions().Insert(optionsInGoal.GetOptions().Count, list.GetOptions()[i]);//לבחור אופציה טובה
            }
            return optionsInGoal;
        }


        public ListOptions BestOptionToDefend(ListOptions list,float canvasH, float canvasW)
        {
            Ball b = new Ball();
            ListOptions optionsToDefend = new ListOptions();
            for (int i = 0; i < list.GetOptions().Count; i++)
            {
                b.SetX(list.GetOptions()[i].GetX());
                b.SetY(list.GetOptions()[i].GetY());
                if (InUpGoal(b, canvasH, canvasW))
                {
                    int index = optionsToDefend.GetOptions().Count;
                    optionsToDefend.GetOptions().Insert(index, list.GetOptions()[i]);
                }
            }
            string strCar="127,34,578,999";

            int inxPsic = strCar.IndexOf(",");
            string strx= strCar.Substring(0,inxPsic);
            int x = int.Parse(strx);
            strCar = strCar.Substring(inxPsic + 1);

            return optionsToDefend;
        }


        public bool CanArriveOnTime(Option o)//checked
        {
            float distansY = Math.Abs(this.y - o.GetY());
            float distansX = Math.Abs(this.x - o.GetX());

            double distans = Math.Sqrt(Math.Pow(distansY, 2) + Math.Pow(distansX, 2));
            double vMax = Math.Sqrt(vMaxPow2);
            int time = o.GetT();
            bool bCanArrive = vMax * time >= distans;
            if (bCanArrive)
                bCanArrive = bCanArrive;
            else
                bCanArrive = bCanArrive;
            return (bCanArrive);
        }

        public bool ComputerHit (Ball ball, float canvasW, float canvasH)
        {
            Option option = BestOption(ball, canvasW, canvasH);
            if (option != null)
            {
                // -- calculate velocity to hit
                float xo = option.GetX();
                float yo = option.GetY();
                float t = option.GetT();
                float dX = xo - this.x;
                float dY = yo - this.y;
                this.vx = dX / t;
                this.vy = dY / t;
                return true;
            }
            return false;
        }
        public Option BestOption(Ball b, float w, float h)
        {
            bool haveToDefend = false; ;
            ListOptions list = BuildAllCyclesOptions(b, h);
            if (list.GetOptions().Count == 0)
            {
                return null;
            }

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
                {
                    if (b.GetX() + x > 363 && b.GetX() + x < 739)
                        haveToDefend = true;
                }
                else
                {
                    if (b.GetX() > 363 && b.GetX() < 739)
                        haveToDefend = true;
                }

            }
            ListOptions lastList;
            if (haveToDefend)
            {
                lastList = BestOptionToDefend(list, h, w);
            }

            else
            {
                lastList = BestOptionToGoal(list, h, w);
            }
            if (lastList.GetOptions().Count == 0)
                return null;
            return lastList.GetOption(0);
        }

        public void Move(float h)
        {
            this.x += this.vx;
            if (this.y + this.vy >= h / 2 - this.bitmap.Height)
                this.vy = -this.vy;
            this.y += this.vy;
        }

        public void CourtCollision(float h, float w) //שמירה על השחקנים בגבולות המסך, שלא יצאו ממנו
        {
            if (this.x < 0)
                this.vx = -this.vx;
            if (this.x > w - this.bitmap.Width)
                this.vx = -this.vx;
            if (this.y < 0)
                this.vy = -this.vy;
            if (this.y > h - this.bitmap.Height)
                this.vy = -this.vy;
        }
    }
}