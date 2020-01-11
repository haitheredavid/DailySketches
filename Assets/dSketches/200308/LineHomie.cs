using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Doodle {

    protected Doodle( Vector3 pos )
        {
            Position = pos;
        }

    public Vector3 Position;

}

public class Point : Doodle {

    public Point( Vector3 pos ) : base( pos )
        {
            Position = pos;
        }

}

public struct Line {

    public Vector3 Start;
    public Vector3 End;

    public Line( Vector3 start, Vector3 end )
        {
            Start = start;
            End = end;
        }

}

public class Cloud : Doodle {

    public Point[ ] Points { get; set; }

    public Cloud( Vector3 pos ) : base( pos )
        {
            Position = pos;
        }

}

public class LineCreator {

    public Line[ ] DrawLine( Cloud pointCloud, int lineCount, int index )
        {
            if ( pointCloud.Points.Length == 0 || pointCloud.Points == null ) return null;

            int a = index;
            int b = index + 1;
            Line[ ] temp = new Line[ pointCloud.Points.Length ];
            for ( var i = 0; i < lineCount; i++ ) {
                if ( a > pointCloud.Points.Length ) {
                    a = 0;
                    b = 1;
                } else if ( b > pointCloud.Points.Length ) {
                    a = pointCloud.Points.Length - 1;
                    b = 0;
                }

                temp[ i ] = new Line {
                    Start = pointCloud.Points[ a ].Position,
                    End = pointCloud.Points[ b ].Position
                };

                a++;
                b++;
            }

            return temp;
        }

    public Cloud CreateRandomCloud( Vector3 position, int count, int density, Vector3 size )
        {
            Cloud temp = new Cloud( position ) {Points = new Point[ count ]};

            for ( int i = 0; i < temp.Points.Length; i++ ) {
                float x = Random.Range( 0, size.x / density );
                float y = Random.Range( 0, size.y / density );
                float z = Random.Range( 0, size.z / density );
                temp.Points[ i ] = new Point( new Vector3( x, y, z ) );
            }
            return temp;
        }

}

public class LineHomie : MonoBehaviour {

    public int pointCount = 10;
    public int lineCount = 10;
    public int cloudDensity = 1;
    public Vector3 size = new Vector3( 3, 3, 3 );

    private Cloud _cloud;
    private GameObject _lineObj;
    private Line[ ] _lines;
    private LineRenderer _renderer;
    private readonly LineCreator _creator = new LineCreator( );

    public void Awake( )
        {
            _renderer = GetComponent<LineRenderer>( );
            // _lineObj = new GameObject( "Line" );
            // var temp = _lineObj.AddComponent<LineRenderer>( );
            // temp = _renderer
            // _line.GetComponent<LineRenderer>( ) = _renderer;
        }

    public void Start( )
        {
            _cloud = _creator.CreateRandomCloud( transform.position, pointCount, cloudDensity, size );
            var pointContainer = new GameObject("container");
            var pointMesh = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            pointMesh.transform.localScale = Vector3.one * 0.2f;

            
            Vector3[] tempPoints = new Vector3[pointCount];
            int index = 0;
            foreach ( var point in _cloud.Points ) {
                var p = Instantiate( pointMesh, pointContainer.transform );
                p.name = "point";
                p.transform.position = point.Position;
                tempPoints[ index ] = point.Position;
                index++;
                
            }
            Destroy( pointMesh );
            
            _renderer.positionCount = tempPoints.Length;
            _renderer.SetPositions( tempPoints );

            tempPoints = null;


        }

}