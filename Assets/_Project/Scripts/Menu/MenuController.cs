using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class MenuController : MonoBehaviour
{
	[SerializeField] private Transform info;
	[SerializeField] private Transform register;
	[SerializeField] private Transform logIn;
	[SerializeField] private Transform menu;
	[SerializeField] private Transform account;
	[SerializeField] private Transform instructions;
	[SerializeField] private Transform firstImage;
	[SerializeField] private Transform forgotPassword;
	[SerializeField] private Transform forgotPassword2;

	[SerializeField] private TMP_InputField registerEmail;
	[SerializeField] private TMP_InputField registerPassword;

    [SerializeField] private TMP_InputField logInEmail;
    [SerializeField] private TMP_InputField logInPassword;


    [SerializeField] private TMP_InputField resetPassword1;
    [SerializeField] private TMP_InputField resetPassword2;

    public const string MatchEmailPattern =
          @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
   + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
   + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
   + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

    public static MenuController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
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
		account.gameObject.SetActive(true);
    }
	public void InstructionsUI()
	{
		Hide();
		instructions.gameObject.SetActive(true);
	}
	public void frgotPasswordUI()
	{
		Hide();
		forgotPassword.gameObject.SetActive(true);
	}
    public void frgotPassword2()
    {
        Hide();
        forgotPassword2.gameObject.SetActive(true);
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
        forgotPassword2.gameObject.SetActive(false);

    }

	public void Register(TMP_InputField mail)
	{
        if (IsEmail(mail.text))
        {
			FirebaseController.Instance.Register(registerEmail.text, registerPassword.text,Finished );
        } 
    }
    public void Login(TMP_InputField mail)
    {
        FirebaseController.Instance.TryLoginAndGetData(logInEmail.text, logInPassword.text, Finished);
    }

    private void Finished(bool _status)
    {
	    Debug.Log("Finished auth with result: "+_status);

    }
    public void ResetPassword1()
    {
        FirebaseController.Instance.ResetPassword(resetPassword1.text, Finished);
        Debug.Log("Mailsend to:" + resetPassword1.text);

    }
    public void ResetPassword2()
    {
        FirebaseController.Instance.ResetPassword(resetPassword2.text, Finished);
        Debug.Log("Mailsend to:" + resetPassword2.text);

    }

    public bool IsEmail(string email)
    {
        if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
        else return false;
    }
}