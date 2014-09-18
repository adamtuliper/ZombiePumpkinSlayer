using UnityEngine;
using System.Collections;

public class PumpkinController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0f;
    [SerializeField]
    private Color[] _hitColors;

    private const int InitialHealth = 4;

    private int _health = InitialHealth;
    private Renderer _renderer;
    private Animator _animator;
    private Vector3 _forward;
    private Transform _particleTransform;

    [SerializeField]
    private GameObject _particleSystem;

    private static GameController _gameController;

    // Use this for initialization
    void Start()
    {
        //Find the grave.
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<Renderer>();
        _forward = Vector3.forward * _speed;
        if (!_gameController)
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        if (!_gameController)
        {
            Debug.LogError("Could not find the GameController");
        }
    }

    void FixedUpdate()
    {
        //Move pumpkin towards cross
        rigidbody.velocity = transform.TransformDirection(_forward);
    }

    private IEnumerator Die()
    {
        //Instantiate particles
        Instantiate(_particleSystem, this.transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        TrashMan.despawn(gameObject);
        _gameController.IncreaseScore();
        yield break;
    }

    private IEnumerator CoApplyDamage()
    {

        //Swap out the color for a damage color
        _renderer.material.color = _hitColors[0];
        yield return new WaitForSeconds(.2f);
        _renderer.material.color = _hitColors[1];
    }

    public void ApplyDamage()
    {
        if (_health <= 0) return;

        _health--;
        if (_health <= 0)
        {
            StartCoroutine(Die());
            return;
        }
        StartCoroutine(CoApplyDamage());
    }

    public void AttackCross()
    {
        _animator.SetTrigger("Bite");
        _gameController.DecrementCrossHealth();
        //StartCoroutine(CoAttackCross());
    }

    private void DecrementCrossHealthFromAnimation()
    {
        _gameController.DecrementCrossHealth();
    }
    private IEnumerator CoAttackCross()
    {
        //make the pumpkin bite
        _animator.SetTrigger("Bite");

        //float randomTime = Random.Range(.1f, 2f) * .1f;
        //Debug.Log("RandomTime:" + randomTime);
        yield return new WaitForSeconds(Random.Range(1, 4));
        yield return null; //we loop until I'm dead.

    }

    /// <summary>
    /// Reset the values of this pumpkin to its default state ready for reuse
    /// </summary>
    public void Reset()
    {
        if (_animator)
        {
            //If animator has been initialized, reset it. First time pool loads it may not be available.
            _animator.enabled = false;
            _animator.enabled = true;
        }
        //When we die, we're turned inactive.
        gameObject.SetActive(true);
        _health = InitialHealth;
        StopAllCoroutines();
    }


}
