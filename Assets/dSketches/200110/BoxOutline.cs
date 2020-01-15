using System;
using System.Collections.Generic;
using dScripts;
using UnityEngine;

namespace dSketches {
    public class BoxOutline : MonoBehaviour {

        public Vector3 bounds;
        private LineRenderer _renderer;
        private Vector3[ ] _locations;
        private Vector3[ ] _box;
        private Dictionary<int, Vector3[ ]> _boxes;
        private int _boxCount = 6;

        private void Awake( ) {
            _locations = new Vector3[ _boxCount ];

            for ( int i = 0; i < _locations.Length; i++ ) {
                _locations[ i ] = new Vector3( i, 0, i );
            }
            _renderer = GetComponent<LineRenderer>( );

            if ( _renderer == null )
                _renderer = gameObject.AddComponent<LineRenderer>( );
        }

        private void Update( ) {
            
            for ( int i = 0; i < _locations.Length; i++ ) {
                _box = Doodler.CreateBox( _locations[ i ], bounds * i );
            }
        }

    }
}