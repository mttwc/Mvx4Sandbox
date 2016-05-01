using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

namespace Mvx4Sandbox.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        private const int REQUEST_IMAGE_CAPTURE = 1;
        
        private Tesseract.ITesseractApi Tesseract
        {
            get;
            set;
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Tesseract = Mvx.GetSingleton<Tesseract.ITesseractApi>();

            // TODO: Tesseract init code and tessdata asset should probably be moved into Setup.cs as it's a singleton
            await Tesseract.Init("eng");

            SetContentView(Resource.Layout.FirstView);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
            {
                Bundle extras = data.Extras;
                Bitmap imageBitmap = (Bitmap)extras.Get("data");

                var detectedTextView = (TextView)FindViewById(Resource.Id.detectedTextView);
                detectedTextView.Text = "The picture was successfully taken.";
            }
        }

        [Export("OnButtonClicked")]
        public void OnButtonClicked(View v)
        {
            if (v.Id == Resource.Id.cameraButton)
            {
                Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);
                if (takePictureIntent.ResolveActivity(PackageManager) != null)
                {
                    StartActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
                }
            }
        }
    }
}
