using UnityEngine;
using UnityEngine.UI;
/* http://www.Mousawi.Dev By @AbdullaMousawi*/
public class Health : MonoBehaviour
{
    public Player player;
    public Image healthBar, ringHealthBar;
    public Image[] healthPoints;


    float lerpSpeed;

    private void Start()
    {

    }

    private void Update()
    {



        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (Player.boostGauge / Player.boostGaugeMax), lerpSpeed);
        ringHealthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (Player.boostGauge / Player.boostGaugeMax), lerpSpeed);

        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(Player.boostGauge, i);
        }
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.blue, Color.cyan, (Player.boostGauge / Player.boostGaugeMax));
        healthBar.color = healthColor;
        ringHealthBar.color = healthColor;
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

}
