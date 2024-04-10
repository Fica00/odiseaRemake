using System;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace FirebaseAuthHandler
{
    public class FirebaseAuthentication
    {
        private FirebaseAuth auth;
        private FirebaseUser firebaseUser;

        public bool IsSignedIn => firebaseUser != null;
        public FirebaseUser FirebaseUser => firebaseUser;

        public void Init(FirebaseAuth _auth)
        {
            auth = _auth;
            firebaseUser = auth.CurrentUser;
        }

        public void SignInAnonymous(Action<SignInResult> _callBack)
        {
            (bool _canSignIn, SignInResult _signInResult) = CanSignIn();

            if (!_canSignIn)
            {
                _callBack?.Invoke(_signInResult);
                return;
            }

            auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(_task =>
            {
                SignInResult _result = new();
                if (_task.IsCanceled)
                {
                    _result.Message = "SignIn was canceled";
                }
                else if (_task.IsFaulted)
                {
                    _result.Message = "SignIn encountered an error: " + _task.Exception;
                }
                else
                {
                    _result.IsSuccessful = true;
                    firebaseUser = _task.Result.User;
                }

                _callBack?.Invoke(_result);
            });
        }

        public void SignInGoogle(Action<SignInResult> _callBack)
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _callBack?.Invoke(new SignInResult { IsSuccessful = false, Message = "Unsupported platform" });
                return;
            }

            (bool _canSignIn, SignInResult _signInResult) = CanSignIn();

            if (!_canSignIn)
            {
                _callBack?.Invoke(_signInResult);
                return;
            }

            GoogleSignInHandler.SignIn(_result => { HandleGoogleSignIn(_result, _callBack); });
        }

        private void HandleGoogleSignIn(GoogleSignInResult _result, Action<SignInResult> _callBack)
        {
            if (!_result.IsSuccessful)
            {
                _callBack?.Invoke(new SignInResult { IsSuccessful = false, Message = _result.Message });
                return;
            }

            LoginWithCredentials(_result.Credential, _callBack);
        }

        private void LoginWithCredentials(Credential _credential, Action<SignInResult> _callBack)
        {
            auth.SignInWithCredentialAsync(_credential).ContinueWithOnMainThread(_task =>
            {
                SignInResult _result = new();
                if (_task.Exception != null)
                {
                    _result.Message = "Failed to login: " + _task.Exception.Message;
                }
                else
                {
                    _result.IsSuccessful = true;
                    firebaseUser = _task.Result;
                }

                _callBack?.Invoke(_result);
            });
        }

        public void SignInEmail(string _email, string _password, Action<SignInResult> _callBack)
        {
            (bool _canSignIn, SignInResult _signInResult) = CanSignIn();

            if (!_canSignIn)
            {
                _callBack?.Invoke(_signInResult);
                return;
            }

            auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(_task =>
            {
                SignInResult _result = new();
                if (_task.IsCanceled)
                {
                    _result.Message = "SignIn was canceled";
                }
                else if (_task.IsFaulted)
                {
                    _result.Message = "SignIn encountered an error: " + _task.Exception;
                }
                else
                {
                    _result.IsSuccessful = true;
                    firebaseUser = _task.Result.User;
                }

                _callBack?.Invoke(_result);
            });
        }
        
        public void SignUpEmail(string _email, string _password, Action<SignInResult> _callBack)
        {
            (bool _canSignIn, SignInResult _signInResult) = CanSignIn();

            if (!_canSignIn)
            {
                _callBack?.Invoke(_signInResult);
                return;
            }

            auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(_task =>
            {
                SignInResult _result = new();
                if (_task.IsCanceled)
                {
                    _result.Message = "SignUp was canceled";
                }
                else if (_task.IsFaulted)
                {
                    _result.Message = "SignUp encountered an error: " + _task.Exception;
                }
                else
                {
                    _result.IsSuccessful = true;
                    firebaseUser = _task.Result.User;
                }

                _callBack?.Invoke(_result);
            });
        }
        
        private (bool, SignInResult) CanSignIn()
        {
            SignInResult _result = new();
            bool _canSignIn;

            if (firebaseUser != null)
            {
                _result.Message = "Already signed in";
                _result.IsSuccessful = true;
                _canSignIn = false;
            }
            else
            {
                _canSignIn = true;
            }

            return (_canSignIn, _result);
        }

        public SignOutResult SignOut()
        {
            SignOutResult _result = new();
            if (firebaseUser == null)
            {
                _result.Message = "Not signed in";
            }
            else
            {
                auth.SignOut();
                firebaseUser = null;
                _result.Message = "Successfully signed out";
                _result.IsSuccessful = true;
            }

            return _result;
        }
    }
}