using System.Collections;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [SerializeField] private Transform info;
    [SerializeField] private Transform register;
    [SerializeField] private Transform logIn;
    [SerializeField] private Transform menu;
    [SerializeField] private Transform account;
    [SerializeField] private Transform instructions;
    [SerializeField] private Transform firstImage;
    [SerializeField] private Transform forgotPassword;

    [SerializeField] private TMP_InputField registerEmail;
    [SerializeField] private TMP_InputField registerPassword;

    [SerializeField] private TMP_InputField logInEmail;
    [SerializeField] private TMP_InputField logInPassword;

    [SerializeField] private TMP_InputField resetPasswordEmailInput;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI accountEmailText;

    [SerializeField] private Button vrCinema;
    [SerializeField] private Button touchCinema;
    [SerializeField] private Button logOutButton;
    [SerializeField] private Button resetPasswordButtonLoginUI;

    private const string MATCH_EMAIL_PATTERN = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void OnEnable()
    {
        vrCinema.onClick.AddListener(LoadVRCinema);
        touchCinema.onClick.AddListener(LoadTouchCinema);
        logOutButton.onClick.AddListener(LogOut);
        resetPasswordButtonLoginUI.onClick.AddListener(ForgotPasswordUI);
    }

    private void OnDisable()
    {
        vrCinema.onClick.RemoveListener(LoadVRCinema);
        touchCinema.onClick.RemoveListener(LoadTouchCinema);
        logOutButton.onClick.RemoveListener(LogOut);
        resetPasswordButtonLoginUI.onClick.RemoveListener(ForgotPasswordUI);
    }

    private void LoadVRCinema()
    {
        StartCoroutine(LoadVrRoutine());

        IEnumerator LoadVrRoutine()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            yield return null;
            SceneManager.Instance.LoadScene(SceneManager.CINEMA_VR);
        }
    }

    private void LoadTouchCinema()
    {
        SceneManager.Instance.LoadScene(SceneManager.CINEMA_TOUCH);
    }

    public void InfoUI()
    {
        Hide();
        info.gameObject.SetActive(true);
    }

    public void FirstImage()
    {
        Hide();
        firstImage.gameObject.SetActive(true);
    }

    public void RegisterUi()
    {
        Hide();
        register.gameObject.SetActive(true);
    }

    public void LogInUI()
    {
        Hide();
        logIn.gameObject.SetActive(true);
    }

    public void Menu()
    {
        Hide();
        menu.gameObject.SetActive(true);
    }

    public void AccountUI()
    {
        Hide();
        accountEmailText.text = PlayerPrefs.GetString("email");
        account.gameObject.SetActive(true);
    }

    public void InstructionsUI()
    {
        Hide();
        instructions.gameObject.SetActive(true);
    }

    public void ForgotPasswordUI()
    {
        Hide();
        forgotPassword.gameObject.SetActive(true);
    }

    private void Hide()
    {
        info.gameObject.SetActive(false);
        register.gameObject.SetActive(false);
        logIn.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        account.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);
        firstImage.gameObject.SetActive(false);
        forgotPassword.gameObject.SetActive(false);
    }

    public void Register(TMP_InputField _mail)
    {
        FirebaseController.Instance.Register(registerEmail.text, registerPassword.text, Finished);
    }

    public void Login(TMP_InputField _mail)
    {
        FirebaseController.Instance.TryLoginAndGetData(logInEmail.text, logInPassword.text, Finished);
    }

    private void Finished(bool _status)
    {
        Debug.Log("Finished with result: " + _status);
    }

    public void ResetPassword()
    {
        if (FirebaseController.Instance.IsLoggedIn)
        {
            Debug.Log(PlayerPrefs.GetString("email"));
            FirebaseController.Instance.ResetPassword(PlayerPrefs.GetString("email"), Finished);
            return;
        }

        FirebaseController.Instance.ResetPassword(resetPasswordEmailInput.text, Finished);
    }

    public bool IsEmail(string _email)
    {
        return Regex.IsMatch(_email, MATCH_EMAIL_PATTERN);
    }

    public void SetStatusText(string _text)
    {
        statusText.text = _text;
    }

    private void LogOut()
    {
        LogInUI();
        FirebaseController.Instance.DeleteLocalUserData();
    }
}