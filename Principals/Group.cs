using System;
using System.Collections.Generic;
using System.Text;

using SourceCode.Hosting.Server.Interfaces;

namespace K2Community.CSP.CRM.Principals
{
    class Group : IGroup
    {
        #region Class Level Fields

        #region Private Fields
        /// <summary>
        /// Local groupID variable used to represent the group's ID.
        /// </summary>
        private string _groupID = string.Empty;
        /// <summary>
        /// Local groupName variable used to represent the group's name.
        /// </summary>
        private string _groupName = string.Empty;
        /// <summary>
        /// Local properties variable used to represent the group's properties.
        /// </summary>
        private IDictionary<string, object> _properties = new Dictionary<string, object>();
        #endregion

        #endregion

        #region Default Constructor
        /// <summary>
        /// Instantiates a new Group.
        /// </summary>
        public Group()
        {
            // No implementation necessary.
        }
        #endregion

        #region Constructor with properties
        public Group(string securityLabel, string groupName, string groupDescription, string groupEmail)
        {
            _groupName = groupName;
            _groupID = securityLabel + K2Utils.SecurityLabelSeperator +_groupName; //fully-qualified name (FQN)

            _properties = new Dictionary<string, object>();
            _properties.Add(K2Utils.K2GroupNamePropertyName, _groupID);
            _properties.Add(K2Utils.K2GroupDescriptionPropertyName, groupDescription);
            _properties.Add(K2Utils.K2GroupEmailPropertyName, groupEmail);
        }
        #endregion

        #region Properties

        #region string GroupID
        /// <summary>
        /// Gets or sets GroupID.
        /// </summary>
        public string GroupID
        {
            get
            {
                return _groupID;
            }
            set
            {
                _groupID = value;
            }
        }
        #endregion

        #region string GroupName
        /// <summary>
        /// Gets or sets GroupName.
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                value = _groupName;
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