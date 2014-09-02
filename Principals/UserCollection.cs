using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using SourceCode.Hosting.Server.Interfaces;

namespace K2Community.CSP.CRM.Principals
{
    class UserCollection : IUserCollection
    {
        #region Class Level Fields

        #region Private Fields
        /// <summary>
        /// Local list variable used to hold all the items in this collection.
        /// </summary>
        private List<IUser> list = new List<IUser>();
        #endregion

        #endregion

        #region Indexers

        #region IUser this[int index]
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public IUser this[int index]
        {
            get
            {
                // Get the element at the specified index.
                return list[index];
            }
        }
        #endregion

        #endregion

        #region Default Constructor
        /// <summary>
        /// Instantiates a new UserCollection.
        /// </summary>
        public UserCollection()
        {
            // No implementation necessary.
        }
        #endregion

        #region Methods

        #region void Add(IUser user)
        /// <summary>
        /// Adds an IUser to the collection.
        /// </summary>
        /// <param name="user">An IUser representing the user to add to the collection.</param>
        public void Add(IUser user)
        {
            // Add the user to the collection.
            list.Add(user);
        }
        #endregion

        #region IEnumerator GetEnumerator()
        /// <summary>
        /// Returns an enumerator that iterates through the UserCollection.
        /// </summary>
        /// <returns>An IEnumerator for the UserCollection.</returns>
        public IEnumerator GetEnumerator()
        {
            // Get an enumerator.
            return list.GetEnumerator();
        }
        #endregion

        #endregion
    }
}