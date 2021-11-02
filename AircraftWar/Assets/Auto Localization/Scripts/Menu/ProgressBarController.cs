using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LanguageTranslator
{
    public class ProgressBarController : MonoBehaviour
    {

        [SerializeField]
        private Image progressbar;
        [SerializeField]
        private Text progressValue;

        public void ValueChange(int value, int totalvalue)
        {
            progressbar.fillAmount = (float)value / totalvalue;
            progressValue.text = (((float)value / totalvalue) * 100).ToString() + "%";

        }

        public void ProgressComplete()
        {
            progressbar.fillAmount = 1f;
            progressValue.text = "100%";
        }
    }
}
