using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	[SerializeField] private Transform info;
	[SerializeField] private Transform register;
	[SerializeField] private Transform logIn;
	[SerializeField] private Transform menu;
	[SerializeField] private Transform account;
	[SerializeField] private Transform instructions;


	public void Info()
	{
		Hide();
		info.gameObject.SetActive(true);
	}
	public void Register()
	{
		Hide();
		register.gameObject.SetActive(true);
	}
	public void LogIn()
	{
		Hide();
		logIn.gameObject.SetActive(true);
	}
	public void Menu()
	{
		Hide();
		menu.gameObject.SetActive(true);
	}
	public void Account()
	{
		Hide();
		account.gameObject.SetActive(true);
	}
	public void Instructions()
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
}
