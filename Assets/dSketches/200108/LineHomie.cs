using dScripts;
using UnityEngine;
using Random = UnityEngine.Random;
public class LineHomie : MonoBehaviour {

    public int pointCount = 10;
    public int lineCount = 10;
    public float scale = 0.1f;
    public Material pointMaterial;
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
        }


    public void BuildCloud( )
        {
            _cloud = LineCreator.CreateRandomCloud( transform.position, pointCount, cloudDensity, size );

            var pointContainer = new GameObject( "container" );

            Vector3[ ] tempPoints = new Vector3[ pointCount ];
            int index = 0;
            foreach ( var point in _cloud.Points ) {
                var p = (GameObject) Doodler.DoodlePoint( point.Position, "point", pointMaterial, Vector3.one * scale );

                p.transform.SetParent( pointContainer.transform );
                tempPoints[ index ] = point.Position;
                index++;
            }
            _renderer.positionCount = tempPoints.Length;
            _renderer.SetPositions( tempPoints );

            tempPoints = null;
        }

}