using System;
using System.Collections.Generic;
using System.Text;

using SourceCode.Hosting.Server.Interfaces;

namespace K2Community.CSP.CRM.Principals
{
    class User : IUser
    {
        #region Class Level Fields

        #region Private Fields
        /// <summary>
        /// Local userID variable used to represent the user's ID.
        /// </summary>
        private string _userID = string.Empty;
        /// <summary>
        /// Local userName variable used to represent the user's username.
        /// </summary>
        private string _userName = string.Empty;
        /// <summary>
        /// Local properties variable used to represent the user's properties.
        /// </summary>
        private IDictionary<string, object> _properties = new Dictionary<string, object>();
        #endregion

        #endregion

        #region Default Constructor
        /// <summary>
        /// Instantiates a new User.
        /// </summary>
        public User()
        {
            // No implementation necessary.
        }
        #endregion

        #region Constructor with Properties
        public User(string securityLabel, string userName, string userDescription, string userEmail, string userManager, string sipAccount, string displayName)
        {
            _userName = userName;
            _userID = securityLabel + K2Utils.SecurityLabelSeperator + userName; //fully-qualified name (FQN)

            _properties = new Dictionary<string, object>();
            _properties.Add(K2Utils.K2UserNamePropertyName, userName);
            _properties.Add(K2Utils.K2UserDescriptionPropertyName, userDescription);
            _properties.Add(K2Utils.K2UserEmailPropertyName, userEmail);
            _properties.Add(K2Utils.K2UserManagerPropertyName, userManager);
            _properties.Add(K2Utils.K2UserSipAccountPropertyName, sipAccount);
            _properties.Add(K2Utils.K2UserDisplayNamePropertyName, displayName);
        }
        #endregion

        #region Properties

        #region string UserID
        /// <summary>
        /// Gets or sets UserID.
        /// </summary>
        public string UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }
        #endregion

        #region string UserName
        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                value = _userName;
            }
        }
        #endregion

        #region IDictionary<string, object> Properties
        /// <summary>
        /// Gets or sets Properties.
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                _properties = value;
            }
        }
        #endregion

        #endregion
    }
}