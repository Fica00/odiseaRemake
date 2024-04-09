using UnityEngine;
using UnityEngine.UI;

public class CheckMark : MonoBehaviour
{
   [SerializeField] private Sprite on;
   [SerializeField] private Sprite off;
   [SerializeField] private Button toggle;
   
   private bool isOn;

   public bool IsOn => isOn;

   private void OnEnable()
   {
      toggle.onClick.AddListener(Toggle);
   }

   private void OnDisable()
   {
      toggle.onClick.RemoveListener(Toggle);
   }

   private void Toggle()
   {
      isOn = !isOn;
      toggle.image.sprite = toggle ? on : off;
   }
}
