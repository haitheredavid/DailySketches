using UnityEngine;

public class Line {

    public Vector3 Start;
    public Vector3 End;
    public Color StartColor;
    public Color EndColor;

}

[ExecuteAlways]
public class LineSketch : MonoBehaviour {

    [Range( 0, 100 )]
    public int density;

    public int xBound = 1;
    public int yBound = 1;
    public int zBound = 1;

    public int pointCount;

    public bool rebuildLines;

    public Color startColor;
    public Color endColor;

    private Line[ ] _lines;
    private Vector3[ ] _points;

    public Line[ ] Lines => _lines;

    public void Awake( )
        {
            _lines = BuildLinesAndPoints( 1 );
        }

    public void Update( )
        {
            if ( rebuildLines || _lines == null ) {
                _lines = BuildLinesAndPoints( 1 );
                rebuildLines = false;
            }

            for ( var i = 0; i < _lines.Length - 1; i++ ) {
                Debug.DrawLine( _lines[ i ].Start, _lines[ i ].End, Color.green );
            }
        }

    private Line[ ] BuildLinesAndPoints( int ratio )
        {
            _points = CreatePoints( pointCount );
            return CreateLines( _points, ratio );
        }

    private Line[ ] CreateLines( Vector3[ ] points, int ratio )
        {
            Line[ ] temp = new Line[ points.Length / ratio ];

            int a = 0;
            int b = 1;
            for ( int i = 0; i < temp.Length - 1; i++ ) {
                if ( a > points.Length ) {
                    a = 0;
                    b = 1;
                } else if ( b > points.Length ) {
                    a = points.Length - 1;
                    b = 0;
                }

                temp[ i ] = new Line {
                    Start = points[ a ],
                    End = points[ b ],
                    StartColor = startColor,
                    EndColor = endColor
                };

                a++;
                b++;
            }

            return temp;
        }

    private Vector3[ ] CreatePoints( int count )
        {
            Vector3[ ] temp = new Vector3[ count ];

            for ( int i = 0; i < temp.Length; i++ ) {
                float x = Random.Range( 0, xBound / density );
                float y = Random.Range( 0, yBound / density );
                float z = Random.Range( 0, zBound / density );
                temp[ i ] = new Vector3( x, y, z );
            }
            return temp;
        }

}