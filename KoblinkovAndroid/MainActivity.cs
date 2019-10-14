using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Reflection.Emit;
using Android.Graphics;
using Android.Content;
using Android.Content.Res;
using System.IO;
using System.Collections.Generic;
using System;
using static Android.Views.View;
using Android.Views;

namespace KoblinkovAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        //public System.Type ObjGameActivity = typeof(GameActivity);
        Dictionary<string, int[]> ImageDictionary = new Dictionary<string, int[]> {
            {"start", new int[] {Resource.Drawable.start_unpressed, Resource.Drawable.start_pressed}},
            {"settings", new int[] {Resource.Drawable.settings_unpressed, Resource.Drawable.settings_pressed}},
            {"easy", new int[] {Resource.Drawable.easy_unpressed, Resource.Drawable.easy_pressed}},
            {"normal", new int[] {Resource.Drawable.normal_unpressed, Resource.Drawable.normal_pressed}},
            {"hard", new int[] {Resource.Drawable.hard_unpressed, Resource.Drawable.hard_pressed}}
        };
        public Intent ObjGameActivity;
        public Typeface RuskyFont;
        public TextView Obj_TxtView_Title;
        public ImageView Obj_Button_Start;
        public ImageView Obj_Button_Settings;
        public ImageView Obj_Button_Easy;
        public ImageView Obj_Button_Normal;
        public ImageView Obj_Button_Hard;

        private string Game_Difficulty;
        public event System.EventHandler Difficulty_Changed;
        public string ChangeDifficulty
        {
            get { return Game_Difficulty; }
            set
            {
                Game_Difficulty = value;
                OnDifficulty_Changed();
            }
        }

        protected virtual void OnDifficulty_Changed()
        {
            if (Difficulty_Changed != null) Difficulty_Changed(this, EventArgs.Empty);
            
                
                if (Game_Difficulty == "Easy")
                {
                    Obj_Button_Easy.SetImageResource(ImageDictionary["easy"][1]);
                    Obj_Button_Normal.SetImageResource(ImageDictionary["normal"][0]);
                    Obj_Button_Hard.SetImageResource(ImageDictionary["hard"][0]);
                }
                else if (Game_Difficulty == "Normal")
                {
                    Obj_Button_Easy.SetImageResource(ImageDictionary["easy"][0]);
                    Obj_Button_Normal.SetImageResource(ImageDictionary["normal"][1]);
                    Obj_Button_Hard.SetImageResource(ImageDictionary["hard"][0]);
                }
                else if (Game_Difficulty == "Hard")
                {
                    Obj_Button_Easy.SetImageResource(ImageDictionary["easy"][0]);
                    Obj_Button_Normal.SetImageResource(ImageDictionary["normal"][0]);
                    Obj_Button_Hard.SetImageResource(ImageDictionary["hard"][1]);
                }
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set window fade in animation
            Window.SetWindowAnimations(Android.Resource.Animation.FadeIn);

            SetContentView(Resource.Layout.activity_main);


            // Setting other variables
            ObjGameActivity = new Intent(this, typeof(GameActivity));

            // Declare items from assets folder
            Typeface KremlinTF = Typeface.CreateFromAsset (this.Assets, "kremlin.ttf");

            // Declaring views from activity
            Obj_TxtView_Title = (TextView)FindViewById(Resource.Id.TxtView_Title);
            Obj_Button_Start = (ImageView)FindViewById(Resource.Id.Start_Btn);
            Obj_Button_Settings = (ImageView)FindViewById(Resource.Id.Settings_Btn);
            Obj_Button_Easy = (ImageView)FindViewById(Resource.Id.Easy_Btn);
            Obj_Button_Normal = (ImageView)FindViewById(Resource.Id.Normal_Btn);
            Obj_Button_Hard = (ImageView)FindViewById(Resource.Id.Hard_Btn);

            // Changing view options
            Obj_TxtView_Title.SetTypeface(KremlinTF, TypefaceStyle.Normal);
            Obj_Button_Start.SetImageResource(ImageDictionary["start"][0]);
            Obj_Button_Settings.SetImageResource(ImageDictionary["settings"][0]);
            Obj_Button_Easy.SetImageResource(ImageDictionary["easy"][1]);
            Obj_Button_Normal.SetImageResource(ImageDictionary["normal"][0]);
            Obj_Button_Hard.SetImageResource(ImageDictionary["hard"][0]);

            // Binding buttons
            Obj_Button_Start.Touch += Obj_Button_Start_Touch;
            Obj_Button_Settings.Touch += Obj_Button_Settings_Touch;
            Obj_Button_Easy.Touch += Obj_Button_Easy_Touch;
            Obj_Button_Normal.Touch += Obj_Button_Normal_Touch;
            Obj_Button_Hard.Touch += Obj_Button_Hard_Touch;

            //Obj_Button_Easy.Touch += Obj_Button_Easy_Touch;

            Game_Difficulty = "Easy";
        }
        private void Obj_Button_Easy_Touch(object sender, TouchEventArgs e)
        {
            ChangeDifficulty = "Easy";
        }
        private void Obj_Button_Normal_Touch(object sender, TouchEventArgs e)
        {
            ChangeDifficulty = "Normal";
        }
        private void Obj_Button_Hard_Touch(object sender, TouchEventArgs e)
        {
            ChangeDifficulty = "Hard";
        }

        private void Obj_Button_Settings_Touch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                Obj_Button_Settings.SetImageResource(ImageDictionary["settings"][0]);
            }
            else if (e.Event.Action == MotionEventActions.Move)
            {
                Obj_Button_Settings.SetImageResource(ImageDictionary["settings"][1]);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        private void Obj_Button_Start_Touch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                Obj_Button_Start.SetImageResource(ImageDictionary["start"][0]);
                ObjGameActivity.PutExtra("Mode", Game_Difficulty);
                StartActivity(ObjGameActivity);
                OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
            }
            else if (e.Event.Action == MotionEventActions.Move)
            {
                Obj_Button_Start.SetImageResource(ImageDictionary["start"][1]);
            }

        }
    }
}