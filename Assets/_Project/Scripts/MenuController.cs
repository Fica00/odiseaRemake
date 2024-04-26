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

	[SerializeField] private TMP_InputField registerEmail;
	[SerializeField] private TMP_InputField registerPassword;

    [SerializeField] private TMP_InputField logInEmail;
    [SerializeField] private TMP_InputField logInPassword;


    public const string MatchEmailPattern =
          @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
   + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
   + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
   + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

	[SerializeField] private FirebaseController fbController;

    private void FixedUpdate()
    {

        logInPassword.contentType = TMP_InputField.ContentType.Password;
        registerPassword.contentType = TMP_InputField.ContentType.Password;
    }

    public void InfoUI()
	{
		Hide();
		info.gameObject.SetActive(true);
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

	private void Hide()
	{
		info.gameObject.SetActive(false);
		register.gameObject.SetActive(false);
		logIn.gameObject.SetActive(false);
		menu.gameObject.SetActive(false);
		account.gameObject.SetActive(false);
		instructions.gameObject.SetActive(false);
	}

	public void Register(TMP_InputField mail)
	{
        if (IsEmail(mail.text))
        {
			fbController.TryLoginAndGetData(registerEmail.text, registerPassword.text,Finished );
			Debug.Log("Account Created");
        } 
		else Debug.Log("Invalid email");
    }
    public void Login(TMP_InputField mail)
    {
            fbController.TryLoginAndGetData(logInEmail.text, logInPassword.text, Finished);
    }

    private void Finished(bool _status)
    {
	    Debug.Log("Finished auth with result: "+_status);

    }

    public static bool IsEmail(string email)
    {
        if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
        else return false;
    }
}