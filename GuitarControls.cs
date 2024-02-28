using UnityEngine;

public class GuitarControls : MonoBehaviour
{
    private AudioListener _listener;

    public GameObject self;
    // note inputs, keyboard 'q' -> 'u'
    // c,c#,d,d#,e,f,f#,g,g#,a,a#,b,
    private readonly KeyCode[] _notes= new KeyCode[] {
        KeyCode.Q,
        //KeyCode.Alpha2,
        KeyCode.W,
        //KeyCode.Alpha3,
        KeyCode.E,
        KeyCode.R,
        //KeyCode.Alpha5,
        KeyCode.T,
        //KeyCode.Alpha6,
        KeyCode.Y,
        //KeyCode.Alpha7,
        KeyCode.U,
    };
    // chord mods: Barr, Major, Minor, 7th, Sus
    private readonly KeyCode[] _modes = new KeyCode[] {
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G
    };

    public AudioSource source;
    // 12 notes with 5 modes each means 60 audio clips
    public AudioClip[] clips;

    // state of the note to be played
    private int _note;
    private int _mode;
    
    // debug
    public bool debug;
    private float _timer;
    
    void Start()
    {
        // in the playback play note*5+mode
        // default chord is a C maj. chord
        _note = 0;
        _mode = 1;
        _timer = 0f;
    }
    
    /*
     * controls:
     *      hitting a note key will set the note to be played
     *      hitting a mode key will set the type of clip to be played
     *      hitting the space bar will play the note
     */
    void Update()
    {
        // note and mode are active until a new one is set
        // check active note
        for (int i = 0; i < 7; i++)
        {
            if (Input.GetKey(_notes[i]))
            {
                _note = i;
            }
        }

        // check active mode
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKey(_modes[i]))
            {
                _mode = i;
            }
        }

        // by default jump is space bar
        // play note
        if (Input.GetButtonDown("Jump"))
        {
            source.PlayOneShot(clips[_note*5+_mode]);
        }
        if (Input.GetMouseButtonDown(0))
        {
            self.SetActive(false);
        }
        
        // debug
        if (debug)
        {
            _timer += Time.deltaTime;
            if (_timer > 1f)
            {
                _timer = 0f;
                Debug.Log(_note+"  |  " + _mode);
            }
        }
    }
}
