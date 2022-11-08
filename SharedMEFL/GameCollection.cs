using MEFL.APIData;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace MEFL
{
    public class GameInfoCollection:ObservableCollection<GameInfoBase>
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
                if (item== APIModel.CurretGame)
                {
                    if (!Refresher.Refreshing)
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
            try
            {
                var res = "";
                foreach (var item in this.Items)
                {
                    try
                    {
                        res += $"{item.Name}\n";
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return base.ToString();
            }
        }
    }
}
