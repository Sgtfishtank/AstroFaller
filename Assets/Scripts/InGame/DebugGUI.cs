using UnityEngine;
using System.Collections;

public class DebugGUI : MonoBehaviour
{
    public int mFpsUpdateFrames;

    private int mDebugGUISizeY;
    private int mF;
    private float mFT;
    private float mFps;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool touching2 = (Input.touchCount >= 2) && (Input.touches[1].phase == TouchPhase.Began);
        bool touching3 = (Input.touchCount >= 3) && (Input.touches[2].phase == TouchPhase.Began) && (!touching2);

        if (Application.loadedLevelName != "MainMenuLevel")
        {
            Player pl = InGame.Instance.Player();

            if (Input.GetKeyDown(KeyCode.I))
            {
                pl.mInvulnerable = !pl.mInvulnerable;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                pl.Dash();
            }

            if (Input.GetKeyDown(KeyCode.Plus))
            {
                pl.PlayerHeal(1);
            }

            if (Input.GetKeyDown(KeyCode.Minus))
            {
                pl.PlayerDamage(1);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                pl.mUseAirReg = !pl.mUseAirReg;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                pl.mUseAirDrain = !pl.mUseAirDrain;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                InGameGUICanvas.Instance.ToggleShowButtons();
            }

            if (Input.GetKeyDown(KeyCode.M) || touching3)
            {
                Light a = GameObject.Find("InGame Camera/Point light").GetComponent<Light>();
                a.enabled = !a.enabled;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M) || touching3)
            {
                Light a = GameObject.Find("Menu Camera/Perk light").GetComponent<Light>();
                Light b = GameObject.Find("Menu Camera/Point light").GetComponent<Light>();
                a.enabled = !a.enabled;
                b.enabled = a.enabled;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                MenuGUICanvas.Instance.ToggleShowButtons();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerData.Instance.depositBolts(9999);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData.Instance.withdrawBolts(PlayerData.Instance.bolts());
        }

        mF++;
        if (mF >= mFpsUpdateFrames)
        {
            mFps = mF / (Time.time - mFT);
            mFT = Time.time;
            mF -= mFpsUpdateFrames;
        }
    }
    
    private Rect lastBounds;
    private string lastString = "";

    void OnGUI()
    {
        GUI.skin.label.fontSize = 16;
        int size = 16;

        Rect bounds = new Rect(10, 10, 200, mDebugGUISizeY);
        GUI.Box(bounds, "Debug Window");
        bounds.x += 10;
        bounds.y += size;
        bounds.height = 10 + size;
        bounds.height += size;

        if (mF == 0)
        {
            string allText = "";

            if (Application.loadedLevelName != "MainMenuLevel")
            {
                allText += "Level: " + InGame.Instance.CurrentLevel() + "\n";
                bounds.height += size;

                allText += "Time on level: " + WorldGen.Instance.LevelRunTime() + "\n";
                bounds.height += size;

                allText += "Bolts on level: " + WorldGen.Instance.Player().colectedBolts() + "\n";
                bounds.height += size;

                allText += "Cyrstals on level: " + WorldGen.Instance.Player().colectedCrystals() + "\n";
                bounds.height += size;

                allText += "Distance on level: " + WorldGen.Instance.Player().Distance() + "\n";
                bounds.height += size;
            }

            allText += "Bolts: " + PlayerData.Instance.bolts() + "\n";
            bounds.height += size;

            allText += "Cyrstals: " + PlayerData.Instance.crystals() + "\n";
            bounds.height += size;

            allText += "Air: " + PlayerData.Instance.mAirPerkUnlockedLevel + "\n";
            bounds.height += size;

            allText += "Burst: " + PlayerData.Instance.mBurstPerkUnlockedLevel + "\n";
            bounds.height += size;

            allText += "Live: " + PlayerData.Instance.mLifePerkUnlockedLevel + "\n";
            bounds.height += size;

            if (Application.loadedLevelName != "MainMenuLevel")
            {
                allText += "Player Air: " + InGame.Instance.Player().AirAmount() + "\n";
                bounds.height += size;

                allText += "Player HP: " + InGame.Instance.Player().mLife + "\n";
                bounds.height += size;

                allText += "Player Invurable: " + InGame.Instance.Player().mInvulnerable + "\n";
                bounds.height += size;

                allText += "Player Reg Air: " + InGame.Instance.Player().mUseAirReg + "\n";
                bounds.height += size;

                allText += "Player Drain Air: " + InGame.Instance.Player().mUseAirDrain + "\n";
                bounds.height += size;

                allText += "Acc: " + Input.acceleration.x + "\n";
                bounds.height += size;
            }
            else
            {
                allText += "Bolts Total: " + PlayerData.Instance.totalBolts() + "\n";
                bounds.height += size;

                allText += "Cyrstals Total: " + PlayerData.Instance.totalCrystals() + "\n";
                bounds.height += size;

                allText += "Distance total: " + PlayerData.Instance.totalDistance() + "\n";
                bounds.height += size;

            }

            allText += "FPS: " + (int)mFps + "\n";
            bounds.height += size;

            lastBounds = bounds;
            lastString = allText; 
        }

        GUI.Label(lastBounds, lastString);

        mDebugGUISizeY = (int)lastBounds.height;
    }
}
