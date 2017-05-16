using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoLogin
{
    public class Options
    {
        public bool SetRealm = true,
                    SetResolution = true,
                    SetAccountName = true,
                    SetAccountList = true,
                    SetGraphicsQuality = true,
                    SetLastCharacterIndex = true;

        Account account;

        public void SetAccount(Account account)
        {
            this.account = account;
        }

        public string[] CompiledList()
        {
            List<string> list = new List<string>();

            list.Add("SET readTOS \"1\"");
            list.Add("SET readEULA \"1\"");

            if (SetRealm && account.SetRealm)
            {
                list.Add("SET realmName \"" + account.Realm + "\"");
            }

            if (SetAccountName)
            {
                list.Add("SET accountName \"" + account.Email + "\"");
            }

            if (SetAccountList && account.Multiple)
            {
                string accountList = "";
                int i = 0;
                foreach (string s in account.AccountNames)
                {
                    if (i == account.SelectedAccount)
                    {
                        accountList += "!" + s.Replace(" ", string.Empty) + "|";
                    }
                    else
                    {
                        accountList += s.Replace(" ", string.Empty) + "|";
                    }
                    i++;
                }
                list.Add("SET accountList \"" + accountList + "\"");
            }

            if (SetLastCharacterIndex && account.SetCharacter)
            {
                list.Add("SET lastCharacterIndex \"" + account.CharacterSlot + "\"");
            }

            return list.ToArray();
        }

        public static List<string> GetRealms()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] realmlist = File.ReadAllLines(path + @"\realmlist");
            List<string> Realms = new List<string>(realmlist.
                Where(realm => !string.IsNullOrWhiteSpace(realm)));

            Realms.Sort();
            return Realms;
        }
    }
}
