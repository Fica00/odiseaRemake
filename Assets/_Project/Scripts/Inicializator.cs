using UnityEngine;

public class Inicializator : MonoBehaviour
{
   [SerializeField] private LanguageManager languageManager;
   [SerializeField] private LogoAnimation logoAnimation;

   private void Start()
   {
      SelectLanguage();
   }

   private void SelectLanguage()
   {
      languageManager.Setup(LogoAnimation);
   }

   private void LogoAnimation()
   {
      logoAnimation.Setup(LoadSignIn);
   }

   private void LoadSignIn()
   {
      SceneManager.Instance.LoadScene(SceneManager.SIGN_IN);  
   }
}