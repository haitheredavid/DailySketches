using System.Collections.Generic;
using UnityEngine;

namespace dScripts {
    public abstract class Doodle {

        public Vector3 Position;

        protected Doodle( Vector3 pos ) {
            Position = pos;
        }

    }
    public class Point : Doodle {

        public Point( Vector3 pos ) : base( pos ) {
            Position = pos;
        }

    }
    public class Cloud : Doodle {

        public Point[ ] Points { get; set; }

        public Cloud( Vector3 pos ) : base( pos ) {
            Position = pos;
        }

    }
    public struct Line {

        public Vector3 Start;
        public Vector3 End;

        public Line( Vector3 start, Vector3 end ) {
            Start = start;
            End = end;
        }

    }
    public class LineCreator {

        public Line[ ] DrawLine( Cloud pointCloud, int lineCount, int index ) {
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

        public static Cloud CreateRandomCloud( Vector3 position, int count, int density, Vector3 size ) {
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
    public static class Doodler {

        public static Vector3[ ] CreateBox( Vector3 pos, Vector3 bounds ) {
            return LocateBox( CreateBoundingBox( bounds ), pos );
        }

        public static Line[ ] DrawBox( Vector3[ ] boxPoints ) {
            Line[ ] temp = {
                new Line( boxPoints[ 0 ], boxPoints[ 1 ] ),
                new Line( boxPoints[ 1 ], boxPoints[ 2 ] ),
                new Line( boxPoints[ 2 ], boxPoints[ 3 ] ),
                new Line( boxPoints[ 3 ], boxPoints[ 0 ] ),
                new Line( boxPoints[ 4 ], boxPoints[ 5 ] ),
                new Line( boxPoints[ 5 ], boxPoints[ 6 ] ),
                new Line( boxPoints[ 6 ], boxPoints[ 7 ] ),
                new Line( boxPoints[ 7 ], boxPoints[ 4 ] ),
                new Line( boxPoints[ 0 ], boxPoints[ 4 ] ),
                new Line( boxPoints[ 1 ], boxPoints[ 5 ] ),
                new Line( boxPoints[ 2 ], boxPoints[ 6 ] ),
                new Line( boxPoints[ 3 ], boxPoints[ 7 ] )
            };
            return temp;
        }

        private static Vector3[ ] CreateBoundingBox( Vector3 bounds ) {
            float xHalf = bounds.x / 2f;
            float yHalf = bounds.y / 2f;
            float zHalf = bounds.z / 2f;
            // start box creation 
            Vector3[ ] temp = new Vector3[ 8 ];
            // top box lines
            temp[ 1 ] = new Vector3( xHalf, yHalf, zHalf );
            temp[ 0 ] = new Vector3( xHalf, yHalf, -zHalf );
            temp[ 2 ] = new Vector3( -xHalf, yHalf, zHalf );
            temp[ 3 ] = new Vector3( -xHalf, yHalf, -zHalf );
            // top box lines
            temp[ 4 ] = new Vector3( xHalf, -yHalf, -zHalf );
            temp[ 5 ] = new Vector3( xHalf, -yHalf, zHalf );
            temp[ 6 ] = new Vector3( -xHalf, -yHalf, zHalf );
            temp[ 7 ] = new Vector3( -xHalf, -yHalf, -zHalf );
            return temp;
        }

        private static Vector3[ ] LocateBox( IReadOnlyList<Vector3> box, Vector3 pos ) {
            Vector3[ ] temp = new Vector3[ box.Count ];

            for ( int i = 0; i < box.Count; i++ ) {
                temp[ i ] = box[ i ] + pos;
            }
            return temp;
        }

        public static Object DoodlePoint( Vector3 pPos, string pName, Material pMat, Vector3 pScale ) {
            GameObject point = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            point.GetComponent<MeshRenderer>( ).material = pMat;
            point.transform.position = pPos;
            point.transform.localScale = pScale;
            point.name = pName;
            return point;
        }

        public static GameObject[ ] DoodlePoint( Vector3[ ] pPos, string pName, Material pMat, Vector3 pScale ) {
            GameObject point = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            point.GetComponent<MeshRenderer>( ).sharedMaterial = pMat;
            point.transform.localScale = pScale;
            point.name = pName;
            GameObject[ ] points = new GameObject[ pPos.Length ];

            for ( int i = 0; i < pPos.Length; i++ ) {
                points[ i ] = Object.Instantiate( point );
                points[ i ].transform.position = pPos[ i ];
            }
            Object.Destroy( point );
            return points;
        }

    }
}