using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinTier : MonoBehaviour
{
    [SerializeReference] private DataManager dataManager;

    [SerializeField] private float animationSpeed;

    [SerializeField] private Color32 selectedColour;
    [SerializeField] private Color32 deselectedColour;

    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    [SerializeField] public string[] weatherNames;
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
        dataManager.SetWeatherNames(weatherNames);
        InitialiseDictionaries();

        // for testing purposes
        UnlockTier("Water", 2);
        UnlockTier("Water", 3);
        //UnlockTier("Snow", 2);
        UnlockTier("Snow", 3);
        UnlockTier("Lightning", 2);
        //UnlockTier("Lightning", 3);
        //UnlockTier("Sun", 3);
        //UnlockTier("Sun", 2);

        UpdateLockImages();
        selectedTierNumber = 1;
        currentlyAnimating = false;
    }

    private void InitialiseDictionaries()
    {
        /*
        tierUnlocked = new Dictionary<string, Dictionary<int, bool>>();
        foreach (string name in weatherNames)
        {
            Dictionary<int, bool> newDict = new Dictionary<int, bool>();
            newDict[1] = true;
            newDict[2] = false;
            newDict[3] = false;

            tierUnlocked.Add(name, newDict);
        } */
        // Get the saved progress from data manager:
        tierUnlocked = dataManager.GetUnlockedTiers();
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
            IncreaseTier(); // increase tier level
        }
    }

    private IEnumerator SpinAnimation(Image previousTierObject, float angleRotate)
    {
        currentlyAnimating = true;
        float currentTime = 0f;
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = currentAngle - angleRotate;

        // for getting the right ratio for lerping
        float angleDiff = currentAngle - targetAngle;
        float currentAngleDiff = 0f;

        while (currentAngle >= targetAngle)
        {
            currentTime += Time.deltaTime * animationSpeed;

            // animate rotation
            transform.eulerAngles = new Vector3(0.0f, 0.0f, currentAngle);
            currentAngle -= 10f;
            currentAngleDiff += 10f;

            // animate colours
            previousTierObject.color = Color.Lerp(selectedColour, deselectedColour, currentAngleDiff / angleDiff);
            selectedTierObject.color = Color.Lerp(deselectedColour, selectedColour, currentAngleDiff / angleDiff);
            yield return null;
        }

        currentlyAnimating = false;
    }

    private IEnumerator BudgeAnimation() // called when current tier is unchangable
    {
        currentlyAnimating = true;
        float currentTime = 0f;
        float angleOffset = 15f;
        float currentAngle = transform.eulerAngles.z;
        int switchAnimation = 3;
        bool turn = false;

        while (switchAnimation >= 0)
        {
            currentTime += Time.deltaTime * 3;

            if (switchAnimation == 3)
            {
                currentAngle -= angleOffset / 2;
            }
            else if (switchAnimation == 0)
            {
                currentAngle += angleOffset / 2;
            }
            else
            {
                if (turn)
                {
                    currentAngle -= angleOffset;
                    turn = false;
                }
                else
                {
                    currentAngle += angleOffset;
                    turn = true;
                }
            }

            transform.eulerAngles = new Vector3(0.0f, 0.0f, currentAngle);
            // animate rotation
            switchAnimation--;

            yield return null;
        }

        currentlyAnimating = false;
    }

    private void IncreaseTier()
    {
        if (!currentlyAnimating)
        {
            int newTier = selectedTierNumber;
            bool nextTierUnlocked = false;

            while (!nextTierUnlocked) // keep rotating until next tier is unlocked
            {
                newTier++;
                if (newTier > 3)
                    newTier = 1;

                nextTierUnlocked = CheckTierUnlocked(newTier); // will be true if next tier is unlocked, breaking the loop
            }

            if (newTier != selectedTierNumber) // doesn't need to spin if it is the same tier
                SetToTier(newTier);
            else
                StartCoroutine(BudgeAnimation());
        }
    }

    private void SetToTier(int tierNum)
    {
        if (!currentlyAnimating) // if wheel is not currently in animation
        {
            Image previousTierObject = selectedTierObject;
            StartCoroutine(SpinAnimation(previousTierObject, FindAngleToTier(tierNum))); // execute animation

            selectedTierNumber = tierNum;
            selectedTierObject = tierObjects[selectedTierNumber - 1];

            ChangeProjectile();
        }
    }

    private bool CheckTierUnlocked(int checkTier)
    {
        return tierUnlocked[currentWeather][checkTier];
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
            projectileScript.SwitchWeapon(currentWeather + "Projectile" + selectedTierNumber);
    }

    public void SetNewWeather(string newWeather)
    {
        currentWeather = newWeather;
        UpdateLockImages();
        if (selectedTierNumber != 1)
            SetToTier(1);
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
