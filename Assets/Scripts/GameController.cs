using Prime31.MetroEssentials;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject _pumpkin;
    private int _score;
    //how many pumpkins per wave?
    [SerializeField]
    private int _waveCount = 8;
    //warmup time before spawns start
    [SerializeField]
    private float _startWait;
    //30 seconds before the next wave
    [SerializeField]
    private float _waveWait = 30;

    //Where's our cross to attack.
    private Transform _tombstonePosition;

    [SerializeField]
    private GameObject[] _spawnPoints;

    [SerializeField]
    private Text _scoreText;

    private int _crossHealth = InitialHealth;

    [SerializeField]
    private RectTransform _scoreMask;

    [SerializeField]
    private Color[] _hitColors;

    ///We blink the cross red on damage, need its renderer component
    private Renderer _crossRenderer;
    /// <summary>
    /// For sizing out the health bar.
    /// </summary>
    private RectTransform _healthMaskTransform;

    private float _initialHealthMaskWidth;
    private const int InitialHealth = 10;
    private float _healthMaskHeight;
    private GameObject _resurrectDialog;
    void Start()
    {
#if UNITY_METRO
        SettingsPane.registerSettingsCommand("Privacy Policy", "This is my privacy policy. Consider a url as well to bring you to the site.");
#endif

        //Find the cross
        _tombstonePosition = GameObject.FindGameObjectWithTag("Cross").transform;
        _healthMaskTransform = GameObject.Find("HealthMask").GetComponent<RectTransform>();
        _resurrectDialog = GameObject.Find("ResurrectDialog");
        _resurrectDialog.SetActive(false);

        //Store the height/width of the mask. We'll move the mask back to adjust health later.
        _initialHealthMaskWidth = _healthMaskTransform.sizeDelta.x;
        _healthMaskHeight = _healthMaskTransform.sizeDelta.y;

        _crossRenderer = _tombstonePosition.transform.Find("polySurface1").gameObject.GetComponent<Renderer>();
        var trashMan = GameObject.Find("TrashMan");
        if (!trashMan)
        {
            Debug.LogError("A trashman component wasn't found (an object pool) in this scene. Create one or comment this code out and use the SpawnWithoutPooling below instead of SpawnWitPooling");
        }
        StartCoroutine(SpawnWaves());
        
    }

    public void ShowResurrectPurchase()
    {
        _resurrectDialog.SetActive(true);
    }

    public void HideResurrectPurchase()
    {
        _resurrectDialog.SetActive(false);
    }

    public void IncreaseScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
    
    IEnumerator SpawnWaves()
    {
        return SpawnWithPooling();
        //return SpawnWithoutPooling();
    }

    IEnumerator SpawnWithPooling()
    {
        yield return new WaitForSeconds(_startWait);
        for (int i = 0; i < _waveCount; i++)
        {
            //ask the object pool for an available pumpkin. We've limited to 8
            //active at once to help avoid overload of too many tris in our scene.
            var pumpkin = TrashMan.spawn("Pumpkin", _spawnPoints[Random.Range(0, 2)].transform.position);

            //pumpkin may be null if one isn't available from the pool
            if (pumpkin)
            {
                //Reset its properties
                pumpkin.GetComponent<PumpkinController>().Reset();
                //Look at the cross (so we move towards it)
                pumpkin.transform.LookAt(_tombstonePosition);
            }
            else
            {
                Debug.Log(string.Format("A pumpkin was requested from the pool but not available, " +
                                        "potentially a higher wavecount ({0}) than allowed in the pool?", 
                                        _waveCount));
            }

            //Sleep
            yield return new WaitForSeconds(Random.Range(.5f, 2.5f));
        }
        yield return new WaitForSeconds(_waveWait);
    }

    IEnumerator SpawnWithoutPooling()
    {
        yield return new WaitForSeconds(_startWait);
        while (true)
        {
            for (int i = 0; i < _waveCount; i++)
            {
                //Choos a random spawn point
                var spawnPosition = _spawnPoints[Random.Range(0, 2)].transform.position;

                //Create the pumpkin
                Quaternion spawnRotation = Quaternion.identity;
                var newPumpkin = (GameObject)Instantiate(_pumpkin, spawnPosition, spawnRotation);

                //Look at the cross (so we move towards it)
                newPumpkin.transform.LookAt(_tombstonePosition);

                //Pause for .5 - 2.5 seconds
                yield return new WaitForSeconds(Random.Range(.5f, 2.5f));
            }
            yield return new WaitForSeconds(_waveWait);
        }
    }

    public void DecrementCrossHealth()
    {
        if (_crossHealth <= 0) return;
        _crossHealth--;

        StartCoroutine(CoApplyCrossHit());
        ShowResurrectPurchase();
        //Apply mask to health bar to make it smaller. This will in turn show less of the health bar.
        //The mask simply covers the underlying gameobject. Where the mask is, it shows through to the health bar.
        //If the mask is smaller, we see less of the health bar.
        
        //We are essentally multiplying the width by the current % of health. IF we have 75% health left,
        //we are shrinking the mask to 75% of its initial width (and keeping height the same)
       // _healthMaskTransform.sizeDelta = new Vector2(_initialHealthMaskWidth * (_crossHealth /  (float)InitialHealth), _healthMaskHeight);
    }

    private IEnumerator CoApplyCrossHit()
    {

        //Swap out the color for a damage color
        _crossRenderer.material.color = _hitColors[0];
        yield return new WaitForSeconds(.2f);
        _crossRenderer.material.color = _hitColors[1];
    }


}