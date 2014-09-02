using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Community.CSP.CRM.Principals
{
    internal static class K2Utils
    {


        #region K2 Group Properties 
        //TODO: if needed, define additional properties to the Group. Remember to add any 
        //additional properties to the Group lookup methods
        public static string K2GroupNamePropertyName { get { return "Name"; } }
        public static string K2GroupDescriptionPropertyName { get { return "Description"; } }
        public static string K2GroupEmailPropertyName { get { return "Email"; } }
        public static Dictionary<string, string> K2GroupPropertyDefinitions
        {
            get
            {
                Dictionary<string, string> _groupProperties = new Dictionary<string, string>();
                _groupProperties.Add(K2Utils.K2GroupNamePropertyName, typeof(string).FullName);
                _groupProperties.Add(K2Utils.K2GroupDescriptionPropertyName, typeof(string).FullName);
                _groupProperties.Add(K2Utils.K2GroupEmailPropertyName, typeof(string).FullName);
                return _groupProperties;
            }
        }

        #endregion

        #region K2 User Properties
        //TODO: if needed, define additional properties to the User. Remember to add any 
        //additional properties to the User lookup methods
        public static string K2UserNamePropertyName { get { return "Name"; } }
        public static string K2UserDescriptionPropertyName { get { return "Description"; } }
        public static string K2UserEmailPropertyName { get { return "Email"; } }
        public static string K2UserManagerPropertyName { get { return "Manager"; } }
        public static string K2UserSipAccountPropertyName { get { return "SipAccount"; } }
        public static string K2UserDisplayNamePropertyName { get { return "DisplayName"; } }

        public static Dictionary<string, string> K2UserPropertyDefinitions
        {
            get
            {
                Dictionary<string, string> _userProperties = new Dictionary<string, string>();
                _userProperties.Add(K2Utils.K2UserNamePropertyName, typeof(string).FullName);
                _userProperties.Add(K2Utils.K2UserDescriptionPropertyName, typeof(string).FullName);
                _userProperties.Add(K2Utils.K2UserEmailPropertyName, typeof(string).FullName);
                _userProperties.Add(K2Utils.K2UserManagerPropertyName, typeof(string).FullName);
                _userProperties.Add(K2Utils.K2UserSipAccountPropertyName, typeof(string).FullName);
                _userProperties.Add(K2Utils.K2UserDisplayNamePropertyName, typeof(string).FullName);
                return _userProperties;
            }
        }

        #endregion

        //security label seperator
        public static string SecurityLabelSeperator { get { return ":"; } }

        //remove FQN label
        public static string RemoveLabelFromFQN(string FQN)
        {
            return FQN.Contains(":") ? FQN.Substring(FQN.IndexOf(':') + 1) : FQN;
        }

    }
}
