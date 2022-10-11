using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinTier : MonoBehaviour
{
    [SerializeField] private float animationSpeed;

    [SerializeField] private Color32 selectedColour;
    [SerializeField] private Color32 deselectedColour;

    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    [SerializeField] private string[] weatherNames;
    [SerializeField] private string currentWeather;

    [SerializeReference] private Image[] tierObjects;
    [SerializeReference] private Image selectedTierObject;
    [SerializeReference] ProjectileHandler projectileScript;
    private Dictionary<string, Dictionary<int, bool>> tierUnlocked;
    private int selectedTierNumber;
    private bool currentlyAnimating;


    // Start is called before the first frame update
    void Start()
    {
        InitialiseDictionaries();

        // for testing purposes
        UnlockTier("Water", 2);
        UnlockTier("Snow", 2);

        UpdateLockImages();
        selectedTierNumber = 1;
        currentlyAnimating = false;
    }

    private void InitialiseDictionaries()
    {
        tierUnlocked = new Dictionary<string, Dictionary<int, bool>>();
        foreach (string name in weatherNames)
        {
            Dictionary<int, bool> newDict = new Dictionary<int, bool>();
            newDict[1] = true;
            newDict[2] = false;
            newDict[3] = false;

            tierUnlocked.Add(name, newDict);
        }
    }

    private void UpdateLockImages()
    {
        foreach (KeyValuePair<int, bool> pair in tierUnlocked[currentWeather])
        {
            if (pair.Value == true) // if tier is unlocked
            {
                tierObjects[pair.Key - 1].sprite = unlockedSprite;
                tierObjects[pair.Key - 1].transform.GetChild(0).gameObject.SetActive(true); // getting the only child which is text
            }
            else
            {
                tierObjects[pair.Key - 1].sprite = lockedSprite;
                tierObjects[pair.Key - 1].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void SpinRevolver()
    {
        if (!currentlyAnimating) // if wheel is not currently in animation
        {
            Image previousTierObject = selectedTierObject;
            increaseTier(); // increase tier level
            ChangeProjectile();
            StartCoroutine(SpinAnimation(previousTierObject, FindAngleToTier(selectedTierNumber))); // execute animation
        }
    }

    private IEnumerator SpinAnimation(Image previousTierObject, float angleRotate)
    {
        currentlyAnimating = true;
        float currentTime = 0f;
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = currentAngle - angleRotate;

        while (currentAngle >= targetAngle)
        {
            currentTime += Time.deltaTime * animationSpeed;

            // animate rotation
            transform.eulerAngles = new Vector3(0.0f, 0.0f, currentAngle);
            currentAngle -= 10f;

            // animate colours
            previousTierObject.color = Color.Lerp(selectedColour, deselectedColour, currentTime);
            selectedTierObject.color = Color.Lerp(deselectedColour, selectedColour, currentTime);
            yield return null;
        }

        currentlyAnimating = false;
    }

    private void increaseTier()
    {
        selectedTierObject.color = deselectedColour; // deselect colour

        // increase tier, if max then set to 1
        selectedTierNumber++;
        if (selectedTierNumber > 3)
            selectedTierNumber = 1;
        
        selectedTierObject = tierObjects[selectedTierNumber - 1];
        selectedTierObject.color = selectedColour; // selected colour
    }
    
    public int GetCurrentTier()
    {
        int returningTier = 0; // 0 is for locked tier
        if (tierUnlocked[currentWeather][selectedTierNumber])
            returningTier = selectedTierNumber;

        return returningTier;
    }

    public void UnlockTier(string weatherName, int tier)
    {
        tierUnlocked[weatherName][tier] = true;
    }

    private void ChangeProjectile()
    {
        if (GetCurrentTier() != 0)
            projectileScript.SwitchWeapon(currentWeather + "Projectile"+selectedTierNumber);
    }

    public void SetNewWeather(string newWeather)
    {
        currentWeather = newWeather;
        UpdateLockImages();
        if (selectedTierNumber != 1)
            SetToTierOne();
    }

    private void SetToTierOne()
    {
        if (!currentlyAnimating) // if wheel is not currently in animation
        {
            Image previousTierObject = selectedTierObject;
            StartCoroutine(SpinAnimation(previousTierObject, FindAngleToTier(1))); // execute animation
            selectedTierNumber = 1;

            selectedTierObject = tierObjects[selectedTierNumber - 1];
            selectedTierObject.color = selectedColour; // selected colour
        }
    }

    private float FindAngleToTier(int targetTier)
    {
        int tierDifference = selectedTierNumber - targetTier;
        float returningAngle = 120f;

        if (tierDifference == -2 || tierDifference == 1)
            returningAngle = 240f;

        return returningAngle;
    }
}
