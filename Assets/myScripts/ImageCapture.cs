using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageCapture : MonoBehaviour {

    public int resWidth = 1920;
    public int resHeight = 1080;

    public Camera screenCamera;

    public ImageType format = ImageType.PNG;

    public enum ImageType {

        JPG,
        PNG

    }

    public string folder;

    private bool _captureScreen = false;
    private int counter = 0;



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
            _captureScreen = true;
        }

    private void Update( )
        {
            if ( Input.GetKeyDown( KeyCode.A ) ) {
                TakeScreenShot( );
            }
        }

    private void LateUpdate( )
        {
            if ( !_captureScreen ) return;

            RenderTexture rt = new RenderTexture( resWidth, resHeight, 24 );
            screenCamera.targetTexture = rt;
            Texture2D screenShot = new Texture2D( resWidth, resHeight, TextureFormat.RGB24, false );
            screenCamera.Render( );
            RenderTexture.active = rt;
            screenShot.ReadPixels( new Rect( 0, 0, resWidth, resHeight ), 0, 0 );
            // reset  
            screenCamera.targetTexture = null;
            RenderTexture.active = null;

            //create filename
            string filename = BuildFileName( resWidth, resHeight );

            byte[ ] fileHeader = null;
            byte[ ] fileData = null;

            if ( format == ImageType.PNG ) {
                fileData = screenShot.EncodeToPNG( );
            } else if ( format == ImageType.JPG ) {
                fileData = screenShot.EncodeToJPG( );
            }

            new System.Threading.Thread( ( ) => {
                var f = System.IO.File.Create( filename );
                if ( fileHeader != null ) f.Write( fileHeader, 0, fileHeader.Length );
                f.Write( fileData, 0, fileData.Length );
                f.Close( );
                Debug.Log( $"Wrote screenshot {filename} of size {fileData.Length}" );
            } ).Start( );

            Destroy( rt );
            rt = null;
            screenShot = null;
            _captureScreen = false;
        }

}