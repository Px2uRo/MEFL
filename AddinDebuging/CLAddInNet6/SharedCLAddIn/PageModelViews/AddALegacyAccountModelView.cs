using CoreLaunching.JsonTemplates;
using MEFL.CLAddIn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;
#if WPF
using System.Windows.Controls;
using System.Windows.Data;
using MEFL.Controls;
#elif AVALONIA

#endif

namespace MEFL.CLAddIn
{
    public class AddALegacyAccountCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //var account = new MEFLLegacyAccount(GenerlAddAccountModel.Account.UserName, GenerlAddAccountModel.Account.Uuid);
            //UserManageModel.ModelView.SelectedAccount = account;
            //MEFL.APIData.APIModel.LegacyAccounts.Add(account);
            //RegManager.Write("LegacyAccounts", JsonConvert.SerializeObject(APIData.APIModel.LegacyAccounts));
            //MEFL.APIData.APIModel.AccountConfigs.Add(account);
            //UserManageModel.ModelView.Invoke("Accounts");
        }
    }
    /*public class AuthUuid : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = false;
            var errorText = "反正有问题，我也不知道发生了什么";
            try
            {
                Guid.Parse(value as String);
                foreach (var item in APIData.APIModel.LegacyAccounts)
                {
                    if (item.Uuid as String == value as String)
                    {
                        errorText = "已存在此Uuid";
                        isValid = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            GenerlAddAccountModel.ModelView.Account.Uuid = value as String;
            GenerlAddAccountModel.ModelView.Invoke("Account");
            return new ValidationResult(isValid, errorText);
        }
    }
    public class AuthManageUuid : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = false;
            var errorText = "反正有问题，我也不知道发生了什么";
            try
            {
                Guid.Parse(value as String);
                foreach (var item in APIData.APIModel.LegacyAccounts)
                {
                    if (item.Uuid as String == value as String&& item.Uuid as String!=GenerlManageAccountModelView.CurrectUuid as String)
                    {
                        errorText = "已存在此Uuid";
                        isValid = false;
                        break;
                    }
                }
                isValid = true;
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
            }
            GenerlManageAccountModel.ModelView.Account.Uuid = value as String;
            GenerlManageAccountModel.ModelView.Invoke("Account");
            return new ValidationResult(isValid, errorText);
        }
    }

    public class IsThatLegacyInfoValid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter as String == "Adding")
            {
                bool Yes = true;
                try
                {
                    Guid.Parse(value as String);
                    foreach (var item in APIData.APIModel.LegacyAccounts)
                    {
                        if (item.Uuid as String == value as String)
                        {
                            Yes = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Yes = false;
                }
                if (String.IsNullOrEmpty(GenerlAddAccountModel.Account.UserName))
                {
                    Yes = false;
                }
                else
                {
                    Yes = true;
                    foreach (var item in APIData.APIModel.LegacyAccounts)
                    {
                        if (item.UserName as String == GenerlAddAccountModel.Account.UserName as String)
                        {
                            Yes = false;
                            break;
                        }
                    }
                }
                return Yes;
            }
            else
            {
                bool Yes = true;
                try
                {
                    Guid.Parse(value as String);
                    foreach (var item in APIData.APIModel.LegacyAccounts)
                    {
                        if (item.Uuid as String == value as String)
                        {
                            Yes = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Yes = false;
                }
                if (String.IsNullOrEmpty(GenerlManageAccountModel.Account.UserName))
                {
                    Yes = false;
                }
                else
                {
                    Yes = true;
                    foreach (var item in APIData.APIModel.LegacyAccounts)
                    {
                        if (item.UserName as String == GenerlManageAccountModel.Account.UserName as String)
                        {
                            Yes = false;
                            break;
                        }
                    }
                }
                return Yes;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SorryBut : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //todo i18N
            var isValid = false;
            var errorText = "反正有问题，我也不知道发生了什么";
            if(value == null || value == String.Empty)
            {
                errorText ="不许为空！";
            }
            else
            {
                isValid = true;
                foreach (var item in APIData.APIModel.LegacyAccounts)
                {
                    if(item.UserName as String==value as String)
                    {
                        errorText = "已存在此用户";
                        isValid = false;
                        break;
                    }
                }
            }
            GenerlAddAccountModel.ModelView.Account.UserName = value as String;
            GenerlAddAccountModel.ModelView.Invoke("Account");
            return new ValidationResult(isValid, errorText);
        }
    }

    public class SorryButForManage : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //todo i18N
            var isValid = false;
            var errorText = "反正有问题，我也不知道发生了什么";
            if (value == null || value == String.Empty)
            {
                errorText = "不许为空！";
            }
            else
            {
                isValid = true;
                foreach (var item in APIData.APIModel.LegacyAccounts)
                {
                    if (item.UserName as String == value as String&&item.UserName as String!=GenerlManageAccountModelView.CurrectName as String)
                    {
                        errorText = "已存在此用户";
                        isValid = false;
                        break;
                    }
                }
            }
            GenerlManageAccountModel.ModelView.Account.UserName = value as String;
            GenerlManageAccountModel.ModelView.Invoke("Account");
            return new ValidationResult(isValid, errorText);
        }
    }*/
}
