using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerSound : MonoBehaviour
{
    [Header("FMOD Event References")]
    [FMODUnity.EventRef]
    public string rollEventRef;
    [FMODUnity.EventRef]
    public string jumpEventRef;
    [FMODUnity.EventRef]
    public string landEventRef;
    [FMODUnity.EventRef]
    public string collectEventRef;
    [FMODUnity.EventRef]
    public string musicEventRef;

    [Header("Speed Parameters")]
    [SerializeField]
    private float _minSpeed;
    [SerializeField]
    private float _maxSpeed;

    [Header("Triggers")]
    [SerializeField]
    private InstantaneousTrigger _musicTrigger;

    private FMOD.Studio.EventInstance _roll;
    private FMOD.Studio.EventInstance _music;

    private Rigidbody _rigidbody;
    private PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<PlayerController>();
        try
        {
            _roll = FMODUnity.RuntimeManager.CreateInstance(rollEventRef);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(_roll, transform, _rigidbody);
            _roll.start();

            throw new System.Exception("We done goofed");
        }
        catch 
        {
            Debug.Log("we ungoofed");
        }

        try
        {
            _music = FMODUnity.RuntimeManager.CreateInstance(musicEventRef);
            _music.start();
        }
        catch { }

        _player.OnJump += PlayJump;
        _player.OnLand += PlayLand;
        _player.OnCollect += PlayCollect;

        _musicTrigger.OnTargetEnter += BeginSlope;
    }

    // Update is called once per frame
    void Update()
    {
        // get speed
        // set roll speed
        //FMODUnity.SetP
        float speed = 0;
        if(!_player.InAir)
        {
            speed = Mathf.Clamp((_rigidbody.velocity.magnitude - _minSpeed) / (_maxSpeed - _minSpeed), 0f, 1f);
        }

        _roll.setParameterByName("Speed", speed);
        _music.setParameterByName("Speed", speed);
    }

    private void PlayJump()
    {
        try
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(jumpEventRef, gameObject);
        }
        catch
        {

        }
    }

    private void PlayLand()
    {
        try
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(landEventRef, gameObject);
        }
        catch
        {

        }
    }

    private void PlayCollect()
    {
        try
        {
            FMODUnity.RuntimeManager.PlayOneShot(collectEventRef, transform.position);
        }
        catch
        {

        }
    }

    private void BeginSlope()
    {
        Debug.Log("here");
        _music.setParameterByName("Slope", 1);
    }
}
