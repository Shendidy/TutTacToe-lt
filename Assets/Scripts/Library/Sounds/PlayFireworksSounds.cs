//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Hellmade.Sound;

//[RequireComponent(typeof(ParticleSystem))]
//public class PlayFireworksSounds : MonoBehaviour
//{
//    private ParticleSystem _parentParticleSystem;

//    private int _currentNumberOfParticles = 0;

//    public AudioSource rocketSound;
//    public AudioSource explosionSound;
//    // Start is called before the first frame update
//    void Start()
//    {
//        _parentParticleSystem = this.GetComponent<ParticleSystem>();
//        if (_parentParticleSystem == null)
//            Debug.Log("Missing ParticleSystem! ", this);

//        EazySoundManager.IgnoreDuplicateSounds = false;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Debug.Log("inside play fireworks sounds!");
//        if (_parentParticleSystem.particleCount < _currentNumberOfParticles)
//        {
//            explosionSound.Play();
//        }

//        if (_parentParticleSystem.particleCount > _currentNumberOfParticles)
//        {
//            rocketSound.Play();
//        }

//        _currentNumberOfParticles = _parentParticleSystem.particleCount;
//    }

//}
