using System.Collections.Generic;
using dScripts;
using UnityEngine;

namespace dSketches {
    public class BoxSketch : MonoBehaviour {

        public Vector3 boxBounds;
        public float effect = 1f;
        private Vector3[ ] _boxPoints;

        private void Update( ) {
            _boxPoints = Doodler.CreateBox( transform.position, boxBounds );
            if ( _boxPoints == null ) return;

            DrawBox( );
        }

        private void DrawBox( ) {
            // var s = Mathf.Sin( Time.time ) * effect;
            // // temp[ i ] = ( box[ i ] + pos ) * ( s * effect );
            // temp[ i ] = new Vector3( box[ i ].x - s, box[ i ].y, box[ i ].z - s ) + pos;

            // top of the box
            Debug.DrawLine( _boxPoints[ 0 ], _boxPoints[ 1 ], Color.cyan );
            Debug.DrawLine( _boxPoints[ 1 ], _boxPoints[ 2 ], Color.green );
            Debug.DrawLine( _boxPoints[ 2 ], _boxPoints[ 3 ], Color.red );
            Debug.DrawLine( _boxPoints[ 3 ], _boxPoints[ 0 ], Color.yellow );
            // bottom of the box
            Debug.DrawLine( _boxPoints[ 4 ], _boxPoints[ 5 ], Color.cyan );
            Debug.DrawLine( _boxPoints[ 5 ], _boxPoints[ 6 ], Color.green );
            Debug.DrawLine( _boxPoints[ 6 ], _boxPoints[ 7 ], Color.red );
            Debug.DrawLine( _boxPoints[ 7 ], _boxPoints[ 4 ], Color.yellow );
            // side of box
            Debug.DrawLine( _boxPoints[ 0 ], _boxPoints[ 4 ], Color.cyan );
            Debug.DrawLine( _boxPoints[ 1 ], _boxPoints[ 5 ], Color.green );
            Debug.DrawLine( _boxPoints[ 2 ], _boxPoints[ 6 ], Color.red );
            Debug.DrawLine( _boxPoints[ 3 ], _boxPoints[ 7 ], Color.yellow );
        }

    }
}