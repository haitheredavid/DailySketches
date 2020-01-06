using System;
using UnityEngine;

public class Visualizer : MonoBehaviour {

    public Material material;
    public LineSketch sketch;

    private void OnPostRender( )
        {
            // if ( material == null || sketch == null ) return;

            Debug.Log(  "drawing new lines");
            
            GL.PushMatrix( );
            material.SetPass( 0 );
            GL.Begin( GL.LINES );

            foreach ( var line in sketch.Lines ) {
                GL.Color( line.StartColor );
                GL.Vertex( line.Start );
                GL.Color( line.EndColor );
                GL.Vertex( line.End );
            }

            GL.End( );
            GL.PopMatrix(  );
        }

}