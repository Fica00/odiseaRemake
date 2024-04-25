using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	[SerializeField] private Transform info;
	[SerializeField] private Transform register;
	[SerializeField] private Transform logIn;
	[SerializeField] private Transform menu;
	[SerializeField] private Transform account;
	[SerializeField] private Transform instructions;

	[SerializeField] private TMP_InputField email;
	[SerializeField] private TMP_InputField password;

	public static event Action<bool> loginSucceededEvent;

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


	public void LogIn() 
	{
		FirebaseController fb = new FirebaseController();

		fb.TryLoginAndGetData(email.text, password.text, loginSucceededEvent);
	
	}

}