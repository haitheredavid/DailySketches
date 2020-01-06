using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class ImageCapture : MonoBehaviour {

    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int resWidth = 1920;
    public int resHeight = 1080;

    public bool optimizeForManyScreenshots = true;

    public Camera screenCamera;

    public enum Format {

        JPG,
        PNG,
        RAW,
        PPM

    }

    public Format format = Format.PNG;

    public string folder;

    private bool _captureImage;
    private bool _captureVideo;

    private int counter;

    private string BuildFileName( int width, int height )
        {
            if ( string.IsNullOrEmpty( folder ) ) {
                folder = Application.dataPath;
                if ( Application.isEditor ) {
                    var path = folder + "/..";
                    folder = Path.GetFullPath( path );
                }
                folder += "/screenshots";

                Directory.CreateDirectory( folder );
                string mask = $"screen_{width}x{height}*.{format.ToString( ).ToLower( )}";
                counter = Directory.GetFiles( folder, mask, SearchOption.TopDirectoryOnly ).Length;
            }

            var filename = $"{folder}/screen_{width}x{height}_{counter}.{format.ToString( ).ToLower( )}";

            // up counter for next call
            ++counter;

            // return unique filename
            return filename;
        }

    public void TakeScreenShot( )
        {
            _captureImage = true;
        }

    public void TakeBurstScreenShots( )
        {
            _captureVideo = true;
        }

    private void Update( )
        {
            if ( Input.GetKeyDown( KeyCode.A ) ) {
                TakeScreenShot( );
            }

            _captureVideo = Input.GetKey( KeyCode.B );
        }

    private void LateUpdate( )
        {
            if ( !_captureImage || !_captureVideo ) return;

            _captureImage = true;

            //create render texture that will catch the rendering
            RenderTexture rt = new RenderTexture( resWidth, resHeight, 24 );
            //link render texture to camera
            screenCamera.targetTexture = rt;
            //set texture format 
            Texture2D screenShot = new Texture2D( resWidth, resHeight, TextureFormat.RGB24, false );
            //render 
            screenCamera.Render( );
            // not sure 
            RenderTexture.active = rt;
            screenShot.ReadPixels( new Rect( 0, 0, resWidth, resHeight ), 0, 0 );
            // reset  
            screenCamera.targetTexture = null;
            RenderTexture.active = null;

            //create filename
            string filename = BuildFileName( resWidth, resHeight );

            byte[ ] fileHeader = null;
            byte[ ] fileData = null;

            if ( format == Format.RAW ) {
                fileData = screenShot.GetRawTextureData( );
            } else if ( format == Format.PNG ) {
                fileData = screenShot.EncodeToPNG( );
            } else if ( format == Format.JPG ) {
                fileData = screenShot.EncodeToJPG( );
            } else {
                string headerStr = string.Format( "P6\n{0} {1}\n255\n", resWidth, resHeight );
                fileHeader = Encoding.ASCII.GetBytes( headerStr );
                fileData = screenShot.GetRawTextureData( );
            }

            new Thread( ( ) => {
                var f = File.Create( filename );
                if ( fileHeader != null ) f.Write( fileHeader, 0, fileHeader.Length );
                f.Write( fileData, 0, fileData.Length );
                f.Close( );
                Debug.Log( $"Wrote screenshot {filename} of size {fileData.Length}" );
            } ).Start( );

            if ( optimizeForManyScreenshots == false ) {
                Destroy( rt );
                rt = null;
                screenShot = null;
                _captureImage = false;
            }
        }

}