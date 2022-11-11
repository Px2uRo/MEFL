using MEFL.APIData;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace MEFL
{
    public class GameInfoCollection : ObservableCollection<GameInfoBase>
    {

        protected override void RemoveItem(int index)
        {
            if (this[index] == APIModel.CurretGame)
            {
                APIModel.CurretGame = null;
            }
            this[index].Dispose();
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item == APIModel.CurretGame)
                {
                    if (!GameRefresher.Refreshing)
                    {
                        APIModel.CurretGame = null;
                    }
                }
                item.Dispose();
            }
            base.ClearItems();
        }

        public override string ToString()
        {
            var res = "";
                foreach (var item in this.Items)
                {
                    if (item == null)
                    {
                    res += $"空值\n";
                }
                else
                {
                    res += $"{item.Name}: {item.GameTypeFriendlyName}\n";
                }
            }
            return res;
        }
    }
    public class AccountCollection : ObservableCollection<AccountBase>
    {

        protected override void RemoveItem(int index)
        {
            if (this[index] == APIModel.SelectedAccount)
            {
                APIModel.SelectedAccount = null;
            }
            this[index].Dispose();
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                if (item == APIModel.SelectedAccount)
                {
                    if (!GameRefresher.Refreshing)
                    {
                        APIModel.SelectedAccount = null;
                    }
                }
                item.Dispose();
            }
            base.ClearItems();
        }

        public override string ToString()
        {
            var res = "";
            foreach (var item in this.Items)
            {
                if (item == null)
                {
                    res += $"空值\n";
                }
                else
                {
                    res += $"{item.UserName}:{item.Uuid}\n";
                }
            }
            return res;
        }
    }
}
